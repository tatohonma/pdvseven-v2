namespace a7D.PDV.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _2_29_CorrigeReservaNumeroNFCe : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                IF OBJECT_ID('dbo.sp_ObterProximoNumeroFiscal', 'P') IS NOT NULL
                    DROP PROCEDURE dbo.sp_ObterProximoNumeroFiscal;
            ");

            Sql(@"
                CREATE PROCEDURE dbo.sp_ObterProximoNumeroFiscal
                    @TipoDocumento NVARCHAR(20),
                    @ProximoNumero INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    DECLARE @Chave NVARCHAR(100);
                    DECLARE @ValorConfigurado INT = 0;
                    DECLARE @MaxReservado INT = 0;
                    DECLARE @Candidate INT = 0;
                    DECLARE @SerieAtual INT = 1;
                    DECLARE @NovaSerie INT;
                    DECLARE @ResultadoLock INT;

                    SET @TipoDocumento = UPPER(LTRIM(RTRIM(@TipoDocumento)));
                    SET @Chave = CASE WHEN @TipoDocumento = 'NFCE'
                                      THEN 'NFCe_AtualNotaFiscal'
                                      ELSE @TipoDocumento + '_Sequencia' END;

                    BEGIN TRY
                        BEGIN TRAN;

                        -- Serializa a reserva. O contador e o registro de processamento são
                        -- gravados em transações diferentes, portanto somente locks nas linhas
                        -- de tbProcessamentoSAT não impedem dois caixas de receberem o mesmo nNF.
                        EXEC @ResultadoLock = sys.sp_getapplock
                            @Resource = 'PDV7_NUMERACAO_FISCAL',
                            @LockMode = 'Exclusive',
                            @LockOwner = 'Transaction',
                            @LockTimeout = 30000;

                        IF @ResultadoLock < 0
                            THROW 51000, 'Não foi possível bloquear a numeração fiscal para reserva.', 1;

                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, HOLDLOCK)
                            WHERE Chave = @Chave AND IDTipoPDV IS NULL AND IDPDV IS NULL
                        )
                        BEGIN
                            INSERT INTO dbo.tbConfiguracaoBD
                                (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                            VALUES
                                (NULL, NULL, @Chave, '0', NULL, 1, 'Número atual do documento');
                        END

                        IF @TipoDocumento = 'NFCE'
                        BEGIN
                            IF NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, HOLDLOCK)
                                WHERE Chave = 'NFCe_Serie' AND IDTipoPDV IS NULL AND IDPDV IS NULL
                            )
                            BEGIN
                                INSERT INTO dbo.tbConfiguracaoBD
                                    (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                                VALUES
                                    (NULL, NULL, 'NFCe_Serie', '1', NULL, 1, 'Nº Série');
                            END

                            SELECT @SerieAtual = ISNULL(TRY_CONVERT(INT, Valor), 1)
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, HOLDLOCK)
                            WHERE Chave = 'NFCe_Serie' AND IDTipoPDV IS NULL AND IDPDV IS NULL;

                            SELECT @ValorConfigurado = ISNULL(TRY_CONVERT(INT, Valor), 0)
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, HOLDLOCK)
                            WHERE Chave = @Chave AND IDTipoPDV IS NULL AND IDPDV IS NULL;

                            -- Considera toda numeração já reservada, independentemente do status.
                            -- PROCESSANDO/ERRO também pode ter chegado à SEFAZ e não pode ser
                            -- entregue simultaneamente a outro pedido.
                            SELECT @MaxReservado = ISNULL(MAX(NumeroFiscalSequencial), 0)
                            FROM dbo.tbProcessamentoSAT WITH (UPDLOCK, HOLDLOCK)
                            WHERE SerieFiscal = @SerieAtual
                              AND NumeroFiscalSequencial IS NOT NULL;

                            SET @Candidate =
                                CASE WHEN @ValorConfigurado > @MaxReservado
                                     THEN @ValorConfigurado ELSE @MaxReservado END + 1;

                            IF @Candidate > 999999999
                            BEGIN
                                SET @NovaSerie = @SerieAtual + 1;
                                IF @NovaSerie > 999 SET @NovaSerie = 1;
                                SET @SerieAtual = @NovaSerie;

                                UPDATE dbo.tbConfiguracaoBD
                                   SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                 WHERE Chave = 'NFCe_Serie'
                                   AND IDTipoPDV IS NULL AND IDPDV IS NULL;

                                SELECT @MaxReservado = ISNULL(MAX(NumeroFiscalSequencial), 0)
                                FROM dbo.tbProcessamentoSAT WITH (UPDLOCK, HOLDLOCK)
                                WHERE SerieFiscal = @SerieAtual
                                  AND NumeroFiscalSequencial IS NOT NULL;

                                SET @Candidate = @MaxReservado + 1;
                            END

                            -- Esta atualização ocorre ainda sob o application lock. A próxima
                            -- chamada verá o número reservado mesmo antes do INSERT/UPDATE do
                            -- respectivo tbProcessamentoSAT.
                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @Candidate)
                             WHERE Chave = @Chave
                               AND IDTipoPDV IS NULL AND IDPDV IS NULL;

                            IF EXISTS
                            (
                                SELECT 1 FROM dbo.tbConfiguracaoBD
                                WHERE Chave = 'NFCe_UltimaSerieUsada'
                                  AND IDTipoPDV IS NULL AND IDPDV IS NULL
                            )
                                UPDATE dbo.tbConfiguracaoBD
                                   SET Valor = CONVERT(NVARCHAR(1000), @SerieAtual)
                                 WHERE Chave = 'NFCe_UltimaSerieUsada'
                                   AND IDTipoPDV IS NULL AND IDPDV IS NULL;
                            ELSE
                                INSERT INTO dbo.tbConfiguracaoBD
                                    (IDTipoPDV, IDPDV, Chave, Valor, ValoresAceitos, Obrigatorio, Titulo)
                                VALUES
                                    (NULL, NULL, 'NFCe_UltimaSerieUsada', CONVERT(NVARCHAR(1000), @SerieAtual), NULL, 1, 'Última série usada para NFC-e');
                        END
                        ELSE
                        BEGIN
                            SELECT @ValorConfigurado = ISNULL(TRY_CONVERT(INT, Valor), 0)
                            FROM dbo.tbConfiguracaoBD WITH (UPDLOCK, HOLDLOCK)
                            WHERE Chave = @Chave AND IDTipoPDV IS NULL AND IDPDV IS NULL;

                            SET @Candidate = @ValorConfigurado + 1;

                            UPDATE dbo.tbConfiguracaoBD
                               SET Valor = CONVERT(NVARCHAR(1000), @Candidate)
                             WHERE Chave = @Chave AND IDTipoPDV IS NULL AND IDPDV IS NULL;
                        END

                        SET @ProximoNumero = @Candidate;
                        COMMIT;
                    END TRY
                    BEGIN CATCH
                        IF @@TRANCOUNT > 0 ROLLBACK;
                        THROW;
                    END CATCH
                END
            ");
        }

        public override void Down()
        {
            // Impede que um rollback silencioso reative a condição de corrida da versão anterior.
            Sql("THROW 51001, 'Rollback bloqueado: restaure explicitamente uma versão segura de sp_ObterProximoNumeroFiscal.', 1;");
        }
    }
}
