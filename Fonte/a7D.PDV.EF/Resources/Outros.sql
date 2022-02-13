BEGIN TRANSACTION T1

SET IDENTITY_INSERT [dbo].[tbEstado] ON 
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (1, N'Acre', N'AC')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (2, N'Alagoas', N'AL')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (3, N'Amapá', N'AP')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (4, N'Amazonas', N'AM')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (5, N'Bahia', N'BA')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (6, N'Ceará', N'CE')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (7, N'Distrito Federal', N'DF')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (8, N'Espírito Santo', N'ES')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (9, N'Goiás', N'GO')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (10, N'Maranhão', N'MA')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (11, N'Mato Grosso', N'MT')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (12, N'Mato Grosso do Sul', N'MS')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (13, N'Minas Gerais', N'MG')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (14, N'Pará', N'PA')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (15, N'Paraíba', N'PB')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (16, N'Paraná', N'PR')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (17, N'Pernambuco', N'PE')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (18, N'Piauí', N'PI')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (19, N'Rio de Janeiro', N'RJ')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (20, N'Rio Grande do Norte', N'RN')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (21, N'Rio Grande do Sul', N'RS')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (22, N'Rondônia', N'RO')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (23, N'Roraima', N'RR')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (24, N'Santa Catarina', N'SC')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (25, N'São Paulo', N'SP')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (26, N'Sergipe', N'SE')
INSERT [dbo].[tbEstado] ([IDEstado], [Nome], [Sigla]) VALUES (27, N'Tocantins', N'TO')
SET IDENTITY_INSERT [dbo].[tbEstado] OFF

SET IDENTITY_INSERT [dbo].[tbIdioma] ON 
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (1, N'Português', N'pt_BR')
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (2, N'English', N'en_US')
INSERT [dbo].[tbIdioma] ([IDIdioma], [Nome], [Codigo]) VALUES (3, N'Español', N'es_ES')
SET IDENTITY_INSERT [dbo].[tbIdioma] OFF

COMMIT TRANSACTION T1