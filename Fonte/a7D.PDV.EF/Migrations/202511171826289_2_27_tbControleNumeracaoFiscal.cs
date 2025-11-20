namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_27_tbControleNumeracaoFiscal : DbMigration
    {
        public override void Up()
        {
            // 1) Garante a coluna NumeroFiscalSequencial em tbProcessamentoSAT
            Sql(@"
                IF COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NULL
                BEGIN
                    ALTER TABLE dbo.tbProcessamentoSAT
                        ADD NumeroFiscalSequencial INT NULL;
                END;
            ");

            Sql(@"
                IF COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                BEGIN
                    UPDATE dbo.tbProcessamentoSAT
                    SET NumeroFiscalSequencial = IDProcessamentoSAT
                    WHERE NumeroFiscalSequencial IS NULL;
                END;
            ");

            // 1.2) Nova coluna SerieFiscal (série da nota fiscal) em tbProcessamentoSAT
            Sql(@"
                IF COL_LENGTH('dbo.tbProcessamentoSAT', 'SerieFiscal') IS NULL
                BEGIN
                    ALTER TABLE dbo.tbProcessamentoSAT
                        ADD SerieFiscal INT NULL;
                END;
            ");

            // Preenche SerieFiscal para registros antigos com a série atual configurada
            Sql(@"
                DECLARE @SerieAtual INT;

                SELECT @SerieAtual = TRY_CONVERT(INT, Valor)
                FROM dbo.tbConfiguracaoBD
                WHERE Chave = 'NFCe_Serie'
                  AND IDTipoPDV IS NULL
                  AND IDPDV IS NULL;

                IF @SerieAtual IS NULL
                    SET @SerieAtual = 1;

                UPDATE dbo.tbProcessamentoSAT
                SET SerieFiscal = @SerieAtual
                WHERE SerieFiscal IS NULL;
            ");

            // 1.3) Trigger: ao inserir em tbProcessamentoSAT, preenche SerieFiscal com a série atual
            Sql(@"
                IF OBJECT_ID('dbo.trg_SetSerieFiscalOnInsert', 'TR') IS NOT NULL
                    DROP TRIGGER dbo.trg_SetSerieFiscalOnInsert;
            ");

            Sql(@"
                CREATE TRIGGER dbo.trg_SetSerieFiscalOnInsert
                ON dbo.tbProcessamentoSAT
                AFTER INSERT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @SerieAtual INT;

                    SELECT @SerieAtual = TRY_CONVERT(INT, Valor)
                    FROM dbo.tbConfiguracaoBD
                    WHERE Chave = 'NFCe_Serie'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL;

                    IF @SerieAtual IS NULL
                        SET @SerieAtual = 1;

                    UPDATE s
                       SET s.SerieFiscal = @SerieAtual
                    FROM dbo.tbProcessamentoSAT s
                    INNER JOIN inserted i
                            ON s.IDProcessamentoSAT = i.IDProcessamentoSAT
                    WHERE s.SerieFiscal IS NULL;
                END
            ");

            // Trigger que zera o contador quando a série é alterada manualmente
            Sql(@"
                IF OBJECT_ID('dbo.trg_ResetNFCeNumeroOnSerieChange', 'TR') IS NOT NULL
                    DROP TRIGGER dbo.trg_ResetNFCeNumeroOnSerieChange;
            ");

            Sql(@"
                CREATE TRIGGER dbo.trg_ResetNFCeNumeroOnSerieChange
                ON dbo.tbConfiguracaoBD
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF EXISTS (
                        SELECT 1
                        FROM inserted i
                        INNER JOIN deleted d ON i.IDConfiguracaoBD = d.IDConfiguracaoBD
                        WHERE i.Chave     = 'NFCe_Serie'
                          AND i.IDTipoPDV IS NULL
                          AND i.IDPDV     IS NULL
                          AND ISNULL(i.Valor, '') <> ISNULL(d.Valor, '')
                    )
                    BEGIN
                        DECLARE @SerieAtual NVARCHAR(100);

                        SELECT TOP 1 @SerieAtual = i.Valor
                        FROM inserted i
                        INNER JOIN deleted d ON i.IDConfiguracaoBD = d.IDConfiguracaoBD
                        WHERE i.Chave     = 'NFCe_Serie'
                          AND i.IDTipoPDV IS NULL
                          AND i.IDPDV     IS NULL
                          AND ISNULL(i.Valor, '') <> ISNULL(d.Valor, '');

                        IF @SerieAtual IS NULL
                            SET @SerieAtual = '1';

                        UPDATE cfg
                           SET cfg.Valor = '0'
                        FROM dbo.tbConfiguracaoBD cfg
                        WHERE cfg.Chave     = 'NFCe_AtualNotaFiscal'
                          AND cfg.IDTipoPDV IS NULL
                          AND cfg.IDPDV     IS NULL;

                        IF EXISTS (
                            SELECT 1 
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = 'NFCe_UltimaSerieUsada'
                              AND IDTipoPDV IS NULL
                              AND IDPDV     IS NULL
                        )
                        BEGIN
                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = @SerieAtual
                             WHERE Chave = 'NFCe_UltimaSerieUsada'
                               AND IDTipoPDV IS NULL
                               AND IDPDV     IS NULL;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, 'NFCe_UltimaSerieUsada', @SerieAtual, NULL, 1, 'Última série usada para NFC-e');
                        END
                    END
                END
            ");

            // Seed/ajuste de NFCe_AtualNotaFiscal e NFCe_UltimaSerieUsada
            Sql(@"
                DECLARE @LastNota       INT = 0;
                DECLARE @LastContinuous INT = 0;
                DECLARE @SerieAtualCfg  NVARCHAR(100);

                IF EXISTS (
                    SELECT 1 
                    FROM dbo.tbConfiguracaoBD
                    WHERE Chave = 'NFCe_AtualNotaFiscal'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL
                )
                BEGIN
                    SELECT @LastNota = TRY_CONVERT(INT, Valor)
                    FROM dbo.tbConfiguracaoBD
                    WHERE Chave = 'NFCe_AtualNotaFiscal'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL;
                END

                IF @LastNota IS NULL
                    SET @LastNota = 0;

                IF @LastNota = 0 AND OBJECT_ID('dbo.tbProcessamentoSAT') IS NOT NULL
                BEGIN
                    ;WITH Ordered AS
                    (
                        SELECT 
                            IDProcessamentoSAT,
                            ROW_NUMBER() OVER (ORDER BY IDProcessamentoSAT) AS rn
                        FROM dbo.tbProcessamentoSAT WITH (NOLOCK)
                    )
                    SELECT @LastContinuous = MAX(IDProcessamentoSAT)
                    FROM Ordered
                    WHERE IDProcessamentoSAT = rn;

                    IF @LastContinuous IS NULL
                        SET @LastContinuous = 0;

                    SET @LastNota = @LastContinuous;
                END

                IF NOT EXISTS (
                    SELECT 1 
                    FROM dbo.tbConfiguracaoBD
                    WHERE Chave = 'NFCe_AtualNotaFiscal'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL
                )
                BEGIN
                    INSERT INTO dbo.tbConfiguracaoBD
                        (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                    VALUES
                        (NULL, NULL, 'NFCe_AtualNotaFiscal', CONVERT(NVARCHAR(1000), @LastNota), NULL, 1, 'Número atual da nota fiscal');
                END
                ELSE
                BEGIN
                    UPDATE dbo.tbConfiguracaoBD
                    SET Valor = CONVERT(NVARCHAR(1000), @LastNota)
                    WHERE Chave = 'NFCe_AtualNotaFiscal'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL
                      AND (Valor IS NULL OR Valor = '');
                END

                SELECT @SerieAtualCfg = Valor
                FROM dbo.tbConfiguracaoBD
                WHERE Chave = 'NFCe_Serie'
                  AND IDTipoPDV IS NULL
                  AND IDPDV IS NULL;

                IF @SerieAtualCfg IS NULL
                    SET @SerieAtualCfg = '1';

                IF NOT EXISTS (
                    SELECT 1 
                    FROM dbo.tbConfiguracaoBD
                    WHERE Chave = 'NFCe_UltimaSerieUsada'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL
                )
                BEGIN
                    INSERT INTO dbo.tbConfiguracaoBD
                        (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                    VALUES
                        (NULL, NULL, 'NFCe_UltimaSerieUsada', @SerieAtualCfg, NULL, 1, 'Última série usada para NFC-e');
                END
                ELSE
                BEGIN
                    UPDATE dbo.tbConfiguracaoBD
                    SET Valor = @SerieAtualCfg
                    WHERE Chave = 'NFCe_UltimaSerieUsada'
                      AND IDTipoPDV IS NULL
                      AND IDPDV IS NULL
                      AND (Valor IS NULL OR Valor = '');
                END

                DELETE FROM dbo.tbConfiguracaoBD
                WHERE Chave = 'NFCe_Sequencia'
                  AND IDTipoPDV IS NULL
                  AND IDPDV IS NULL;
            ");

            // 2) (Re)cria procedure com controle de limite + série
            Sql(@"
                IF OBJECT_ID('dbo.sp_ObterProximoNumeroFiscal') IS NOT NULL
                    DROP PROCEDURE dbo.sp_ObterProximoNumeroFiscal;
            ");

            Sql(@"
                CREATE PROCEDURE dbo.sp_ObterProximoNumeroFiscal
                    @TipoDocumento NVARCHAR(20),
                    @ProximoNumero INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @Chave        NVARCHAR(100);
                    DECLARE @ValorAtual   INT = 0;
                    DECLARE @Candidate    INT = 0;
                    DECLARE @LastSeed     INT = 0;
                    DECLARE @SerieAtual   INT = NULL;
                    DECLARE @NovaSerie    INT = NULL;

                    IF UPPER(@TipoDocumento) = 'NFCE'
                        SET @Chave = 'NFCe_AtualNotaFiscal';
                    ELSE
                        SET @Chave = @TipoDocumento + '_Sequencia';

                    BEGIN TRAN;

                        -- Garante que a chave existe
                        IF NOT EXISTS (
                            SELECT 1 
                            FROM dbo.tbConfiguracaoBD 
                            WHERE Chave = @Chave
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL
                        )
                        BEGIN
                            SET @LastSeed = 0;

                            IF UPPER(@TipoDocumento) = 'NFCE'
                               AND OBJECT_ID('dbo.tbProcessamentoSAT') IS NOT NULL
                            BEGIN
                                ;WITH Ordered AS
                                (
                                    SELECT 
                                        IDProcessamentoSAT,
                                        ROW_NUMBER() OVER (ORDER BY IDProcessamentoSAT) AS rn
                                    FROM dbo.tbProcessamentoSAT WITH (NOLOCK)
                                )
                                SELECT @LastSeed = MAX(IDProcessamentoSAT)
                                FROM Ordered
                                WHERE IDProcessamentoSAT = rn;

                                IF @LastSeed IS NULL
                                    SET @LastSeed = 0;
                            END

                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, @Chave, CONVERT(NVARCHAR(1000), @LastSeed), NULL, 1, 'Número atual da nota fiscal');
                        END

                        -- Lê valor atual
                        SELECT @ValorAtual = TRY_CONVERT(INT, Valor)
                        FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, ROWLOCK)
                        WHERE Chave = @Chave
                          AND IDTipoPDV IS NULL
                          AND IDPDV IS NULL;

                        IF @ValorAtual IS NULL
                            SET @ValorAtual = 0;

                        --------------------------------------------------------
                        -- RAMO NFCE: controla limite + série
                        --------------------------------------------------------
                        IF UPPER(@TipoDocumento) = 'NFCE'
                        BEGIN
                            -- Garante NFCe_Serie
                            IF NOT EXISTS (
                                SELECT 1
                                FROM dbo.tbConfiguracaoBD
                                WHERE Chave = 'NFCe_Serie'
                                  AND IDTipoPDV IS NULL
                                  AND IDPDV IS NULL
                            )
                            BEGIN
                                INSERT INTO dbo.tbConfiguracaoBD
                                    (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                                VALUES
                                    (NULL, NULL, 'NFCe_Serie', '1', NULL, 1, 'Nº Série');
                            END

                            SELECT @SerieAtual = TRY_CONVERT(INT, Valor)
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = 'NFCe_Serie'
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL;

                            IF @SerieAtual IS NULL
                                SET @SerieAtual = 1;

                            -- Se valor atual já está no limite, avança série e zera contador
                            IF @ValorAtual >= 999999999
                            BEGIN
                                SET @NovaSerie = @SerieAtual + 1;
                                IF @NovaSerie > 999
                                    SET @NovaSerie = 1;

                                SET @SerieAtual = @NovaSerie;
                                SET @ValorAtual = 0;

                                UPDATE dbo.tbConfiguracaoBD
                                   SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                 WHERE Chave = 'NFCe_Serie'
                                   AND IDTipoPDV IS NULL
                                   AND IDPDV IS NULL;

                                UPDATE dbo.tbConfiguracaoBD
                                   SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                 WHERE Chave = 'NFCe_UltimaSerieUsada'
                                   AND IDTipoPDV IS NULL
                                   AND IDPDV IS NULL;
                            END

                            SET @Candidate = @ValorAtual + 1;

                            IF COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                               AND COL_LENGTH('dbo.tbProcessamentoSAT', 'SerieFiscal') IS NOT NULL
                            BEGIN
                                WHILE 1 = 1
                                BEGIN
                                    -- Se passar do limite, gira série e volta para 1
                                    IF @Candidate > 999999999
                                    BEGIN
                                        SET @SerieAtual = @SerieAtual + 1;
                                        IF @SerieAtual > 999
                                            SET @SerieAtual = 1;

                                        UPDATE dbo.tbConfiguracaoBD
                                           SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                         WHERE Chave = 'NFCe_Serie'
                                           AND IDTipoPDV IS NULL
                                           AND IDPDV IS NULL;

                                        UPDATE dbo.tbConfiguracaoBD
                                           SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                         WHERE Chave = 'NFCe_UltimaSerieUsada'
                                           AND IDTipoPDV IS NULL
                                           AND IDPDV IS NULL;

                                        SET @Candidate = 1;
                                    END

                                    IF EXISTS (
                                        SELECT 1
                                        FROM dbo.tbProcessamentoSAT
                                        WHERE NumeroFiscalSequencial = @Candidate
                                          AND SerieFiscal           = @SerieAtual
                                    )
                                    BEGIN
                                        SET @Candidate = @Candidate + 1;
                                        CONTINUE;
                                    END

                                    BREAK;
                                END
                            END
                        END
                        --------------------------------------------------------
                        -- RAMO OUTROS DOCUMENTOS
                        --------------------------------------------------------
                        ELSE
                        BEGIN
                            SET @Candidate = @ValorAtual + 1;

                            IF COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                            BEGIN
                                WHILE EXISTS (
                                    SELECT 1
                                    FROM dbo.tbProcessamentoSAT
                                    WHERE NumeroFiscalSequencial = @Candidate
                                )
                                BEGIN
                                    SET @Candidate = @Candidate + 1;
                                END
                            END
                        END

                        SET @ProximoNumero = @Candidate;

                        UPDATE dbo.tbConfiguracaoBD
                        SET Valor = CONVERT(NVARCHAR(1000), @Candidate)
                        WHERE Chave = @Chave
                          AND IDTipoPDV IS NULL
                          AND IDPDV IS NULL;

                    COMMIT;
                END
            ");
        }

        public override void Down()
        {
            Sql(@"
                IF OBJECT_ID('dbo.sp_ObterProximoNumeroFiscal') IS NOT NULL
                    DROP PROCEDURE dbo.sp_ObterProximoNumeroFiscal;
            ");

            Sql(@"
                IF OBJECT_ID('dbo.trg_ResetNFCeNumeroOnSerieChange', 'TR') IS NOT NULL
                    DROP TRIGGER dbo.trg_ResetNFCeNumeroOnSerieChange;
            ");

            Sql(@"
                IF OBJECT_ID('dbo.trg_SetSerieFiscalOnInsert', 'TR') IS NOT NULL
                    DROP TRIGGER dbo.trg_SetSerieFiscalOnInsert;
            ");

            Sql(@"
                DELETE FROM dbo.tbConfiguracaoBD
                WHERE Chave = 'NFCe_UltimaSerieUsada'
                  AND IDTipoPDV IS NULL
                  AND IDPDV IS NULL;
            ");

            Sql(@"
                IF COL_LENGTH('dbo.tbProcessamentoSAT','SerieFiscal') IS NOT NULL
                    ALTER TABLE dbo.tbProcessamentoSAT DROP COLUMN SerieFiscal;
            ");

            Sql(@"
                IF COL_LENGTH('dbo.tbProcessamentoSAT','NumeroFiscalSequencial') IS NOT NULL
                    ALTER TABLE dbo.tbProcessamentoSAT DROP COLUMN NumeroFiscalSequencial;
            ");
        }
    }
}
