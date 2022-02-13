/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
USE PDV7
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.tbPainelModificacao SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.tbPainelModificacao', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.tbPainelModificacao', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.tbPainelModificacao', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.tbPainelModificacaoRelacionado
	(
	IDPainelModificacaoRelacionado int NOT NULL IDENTITY (1, 1),
	IDPainelModificacao1 int NOT NULL,
	IDPainelModificacao2 int NOT NULL,
	IgnorarValorItem bit NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tbPainelModificacaoRelacionado ADD CONSTRAINT
	PK_tbPainelModificacaoRelacionado PRIMARY KEY CLUSTERED 
	(
	IDPainelModificacaoRelacionado
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.tbPainelModificacaoRelacionado ADD CONSTRAINT
	FK_PainelModificacaoRelacionado1 FOREIGN KEY
	(
	IDPainelModificacao1
	) REFERENCES dbo.tbPainelModificacao
	(
	IDPainelModificacao
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.tbPainelModificacaoRelacionado ADD CONSTRAINT
	FK_PainelModificacaoRelacionado2 FOREIGN KEY
	(
	IDPainelModificacao2
	) REFERENCES dbo.tbPainelModificacao
	(
	IDPainelModificacao
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.tbPainelModificacaoRelacionado SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.tbPainelModificacaoRelacionado', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.tbPainelModificacaoRelacionado', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.tbPainelModificacaoRelacionado', 'Object', 'CONTROL') as Contr_Per 