rem @ECHO OFF

RMDIR "..\Fonte\a7D.PDV.Configurador.UI\bin" /S /Q
RMDIR "..\Fonte\a7D.PDV.BackOffice.UI\bin" /S /Q
RMDIR "..\Fonte\a7D.PDV.Caixa.UI\bin" /S /Q
RMDIR "..\Fonte\a7D.PDV.Integracao.Servico.UI\bin" /S /Q
RMDIR "..\Fonte\a7D.PDV.Terminal.UI\bin" /S /Q

RMDIR "..\Publish\AutoAtendimento" /S /Q
RMDIR "..\Publish\PainelMesaComanda" /S /Q
RMDIR "..\Publish\SaidaComanda" /S /Q
RMDIR "..\Publish\www_SAT" /S /Q
RMDIR "..\Publish\www2" /S /Q

RMDIR "..\bin" /S /Q

TIMEOUT /T 10