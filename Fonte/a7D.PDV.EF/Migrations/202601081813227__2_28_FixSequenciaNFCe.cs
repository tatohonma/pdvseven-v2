namespace a7D.PDV.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_28_FixSequenciaNFCe : DbMigration
    {
    public override void Up()
        {
            // 1) Trigger: quando mudar a série, NFCe_AtualNotaFiscal vira MAX da série (espelho)
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
                        DECLARE @SerieAtual INT = NULL;
                        DECLARE @MaxNum INT = 0;

                        SELECT TOP 1 @SerieAtual = TRY_CONVERT(INT, i.Valor)
                        FROM inserted i
                        INNER JOIN deleted d ON i.IDConfiguracaoBD = d.IDConfiguracaoBD
                        WHERE i.Chave     = 'NFCe_Serie'
                          AND i.IDTipoPDV IS NULL
                          AND i.IDPDV     IS NULL
                          AND ISNULL(i.Valor, '') <> ISNULL(d.Valor, '');

                        IF @SerieAtual IS NULL SET @SerieAtual = 1;

                        -- Atualiza NFCe_UltimaSerieUsada
                        IF EXISTS (
                            SELECT 1 
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = 'NFCe_UltimaSerieUsada'
                              AND IDTipoPDV IS NULL
                              AND IDPDV     IS NULL
                        )
                        BEGIN
                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                             WHERE Chave = 'NFCe_UltimaSerieUsada'
                               AND IDTipoPDV IS NULL
                               AND IDPDV     IS NULL;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, 'NFCe_UltimaSerieUsada', CONVERT(NVARCHAR(1000), @SerieAtual), NULL, 1, 'Última série usada para NFC-e');
                        END

                        -- Define NFCe_AtualNotaFiscal como MAX(NumeroFiscalSequencial) da NOVA série
                        IF OBJECT_ID('dbo.tbProcessamentoSAT') IS NOT NULL
                           AND COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                           AND COL_LENGTH('dbo.tbProcessamentoSAT', 'SerieFiscal') IS NOT NULL
                        BEGIN
                            SELECT @MaxNum = ISNULL(MAX(NumeroFiscalSequencial), 0)
                            FROM dbo.tbProcessamentoSAT WITH (NOLOCK)
                            WHERE SerieFiscal = @SerieAtual
                              AND NumeroFiscalSequencial IS NOT NULL;
                        END
                        ELSE
                        BEGIN
                            SET @MaxNum = 0;
                        END

                        IF EXISTS (
                            SELECT 1 
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = 'NFCe_AtualNotaFiscal'
                              AND IDTipoPDV IS NULL
                              AND IDPDV     IS NULL
                        )
                        BEGIN
                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @MaxNum)
                             WHERE Chave = 'NFCe_AtualNotaFiscal'
                               AND IDTipoPDV IS NULL
                               AND IDPDV     IS NULL;
                        END
                        ELSE
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, 'NFCe_AtualNotaFiscal', CONVERT(NVARCHAR(1000), @MaxNum), NULL, 1, 'Número atual da nota fiscal');
                        END
                    END
                END
            ");

            // 2) Procedure: para NFCE pega "depois do salto" (último bloco após o último gap)
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

                    DECLARE @Chave      NVARCHAR(100);
                    DECLARE @ValorAtual INT = 0;
                    DECLARE @Candidate  INT = 0;
                    DECLARE @SerieAtual INT = NULL;
                    DECLARE @NovaSerie  INT = NULL;

                    IF UPPER(@TipoDocumento) = 'NFCE'
                        SET @Chave = 'NFCe_AtualNotaFiscal';
                    ELSE
                        SET @Chave = @TipoDocumento + '_Sequencia';

                    BEGIN TRAN;

                        -- garante chave existe (espelho)
                        IF NOT EXISTS (
                            SELECT 1
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = @Chave
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL
                        )
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, @Chave, '0', NULL, 1, 'Número atual do documento');
                        END

           
                        IF UPPER(@TipoDocumento) = 'NFCE'
                        BEGIN
                            -- garante NFCe_Serie
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

                            -- lock na série atual
                            SELECT @SerieAtual = TRY_CONVERT(INT, Valor)
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, ROWLOCK)
                            WHERE Chave = 'NFCe_Serie'
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL;

                            IF @SerieAtual IS NULL SET @SerieAtual = 1;

                            IF OBJECT_ID('dbo.tbProcessamentoSAT') IS NOT NULL
                               AND COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                               AND COL_LENGTH('dbo.tbProcessamentoSAT', 'SerieFiscal') IS NOT NULL
                            BEGIN
                              ;WITH nums AS
                                (
                                    SELECT DISTINCT
                                        p.NumeroFiscalSequencial AS n
                                    FROM dbo.tbProcessamentoSAT p WITH (UPDLOCK, ROWLOCK)
                                    WHERE p.SerieFiscal = @SerieAtual
                                      AND p.NumeroFiscalSequencial IS NOT NULL
                                      AND p.IDStatusProcessamentoSAT = 30 
                                ),

                                ordered AS
                                (
                                    SELECT
                                        n,
                                        LAG(n) OVER (ORDER BY n) AS prev_n
                                    FROM nums
                                ),
                                last_gap AS
                                (
                                    SELECT MAX(n) AS start_after_gap
                                    FROM ordered
                                    WHERE prev_n IS NOT NULL
                                      AND n > prev_n + 1
                                ),
                                tail AS
                                (
                                    -- se teve salto, pega só o bloco final (>= start_after_gap)
                                    -- se não teve salto, pega tudo (start_after_gap NULL -> condição vira TRUE)
                                    SELECT n
                                    FROM ordered
                                    WHERE (SELECT start_after_gap FROM last_gap) IS NULL
                                       OR n >= (SELECT start_after_gap FROM last_gap)
                                )
                                SELECT @ValorAtual = ISNULL(MAX(n), 0)
                                FROM tail;

                                SET @Candidate = @ValorAtual + 1;
                            END
                            ELSE
                            BEGIN
                                -- fallback: usa config
                                SELECT @ValorAtual = TRY_CONVERT(INT, Valor)
                                FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, ROWLOCK)
                                WHERE Chave = @Chave
                                  AND IDTipoPDV IS NULL
                                  AND IDPDV IS NULL;

                                IF @ValorAtual IS NULL SET @ValorAtual = 0;

                                SET @Candidate = @ValorAtual + 1;
                            END

                            -- limite, gira série
                            IF @Candidate > 999999999
                            BEGIN
                                SET @NovaSerie = @SerieAtual + 1;
                                IF @NovaSerie > 999 SET @NovaSerie = 1;

                                SET @SerieAtual = @NovaSerie;

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

                                -- recalcula na nova série
                                IF OBJECT_ID('dbo.tbProcessamentoSAT') IS NOT NULL
                                   AND COL_LENGTH('dbo.tbProcessamentoSAT', 'NumeroFiscalSequencial') IS NOT NULL
                                   AND COL_LENGTH('dbo.tbProcessamentoSAT', 'SerieFiscal') IS NOT NULL
                                BEGIN
                                    SELECT @ValorAtual = ISNULL(MAX(NumeroFiscalSequencial), 0)
                                    FROM dbo.tbProcessamentoSAT WITH (UPDLOCK, ROWLOCK)
                                    WHERE SerieFiscal = @SerieAtual
                                      AND NumeroFiscalSequencial IS NOT NULL;
                                END
                                ELSE
                                BEGIN
                                    SET @ValorAtual = 0;
                                END

                                SET @Candidate = @ValorAtual + 1;
                            END

                            SET @ProximoNumero = @Candidate;

                            -- espelha no config (não é fonte de verdade)
                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @Candidate)
                             WHERE Chave = @Chave
                               AND IDTipoPDV IS NULL
                               AND IDPDV IS NULL;
                        END
                        --------------------------------------------------------
                        -- OUTROS DOCUMENTOS: incremental simples
                        --------------------------------------------------------
                        ELSE
                        BEGIN
                            SELECT @ValorAtual = TRY_CONVERT(INT, Valor)
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, ROWLOCK)
                            WHERE Chave = @Chave
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL;

                            IF @ValorAtual IS NULL SET @ValorAtual = 0;

                            SET @Candidate = @ValorAtual + 1;
                            SET @ProximoNumero = @Candidate;

                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @Candidate)
                             WHERE Chave = @Chave
                               AND IDTipoPDV IS NULL
                               AND IDPDV IS NULL;
                        END

                    COMMIT;
                END
            ");
        }

        public override void Down()
        {
            // Mantém seu Down atual (como você já tinha)
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

                    DECLARE @Chave NVARCHAR(100);
                    DECLARE @ValorAtual INT = 0;

                    IF UPPER(@TipoDocumento) = 'NFCE'
                        SET @Chave = 'NFCe_AtualNotaFiscal';
                    ELSE
                        SET @Chave = @TipoDocumento + '_Sequencia';

                    BEGIN TRAN;

                        IF NOT EXISTS (
                            SELECT 1
                            FROM dbo.tbConfiguracaoBD
                            WHERE Chave = @Chave
                              AND IDTipoPDV IS NULL
                              AND IDPDV IS NULL
                        )
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, @Chave, '0', NULL, 1, 'Número atual do documento');
                        END

                        SELECT @ValorAtual = TRY_CONVERT(INT, Valor)
                        FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, ROWLOCK)
                        WHERE Chave = @Chave
                          AND IDTipoPDV IS NULL
                          AND IDPDV IS NULL;

                        IF @ValorAtual IS NULL SET @ValorAtual = 0;

                        SET @ProximoNumero = @ValorAtual + 1;

                        UPDATE dbo.tbConfiguracaoBD
                           SET Valor = CONVERT(NVARCHAR(1000), @ProximoNumero)
                         WHERE Chave = @Chave
                           AND IDTipoPDV IS NULL
                           AND IDPDV IS NULL;

                    COMMIT;
                END
            ");
        }
    }
}
