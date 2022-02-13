echo off
set errorlevel=0
echo Verificando se o SQL ja esta Instalado
if exist "C:\Program Files\Microsoft SQL Server\MSSQL12.PDV7" set errorlevel=1
if exist "C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.PDV7" set errorlevel=1
if exist "C:\Program Files\Microsoft SQL Server\MSSQL13.PDV7" set errorlevel=1
if exist "C:\Program Files (x86)\Microsoft SQL Server\MSSQL13.PDV7" set errorlevel=1
if exist "C:\Program Files\Microsoft SQL Server\MSSQL14.PDV7" set errorlevel=1
if exist "C:\Program Files (x86)\Microsoft SQL Server\MSSQL14.PDV7" set errorlevel=1
if "%errorlevel%"=="1" goto erro

echo SQL Nao instalado
FOR /F "tokens=2 delims==" %%A in ('wmic OS get OSArchitecture /value ^| find "="') do SET Arquitetura=%%A
set arquivosql=0
if "%Arquitetura%"=="64 bits" set arquivosql=SQLEXPRWT_x64_PTB
if "%Arquitetura%"=="32-bit" set arquivosql=SQLEXPRWT_x86_PTB
if "%arquivosql%"=="0" set errorlevel=2
if "%errorlevel%"=="2" goto erro
if "%Arquitetura%"=="64 bits" if NOT EXIST %arquivosql%.exe echo baixando arquivo %arquivosql%.exe (259MB)...
if "%Arquitetura%"=="32-bit" if NOT EXIST %arquivosql%.exe echo baixando arquivo %arquivosql%.exe (275MB)...
if NOT EXIST %arquivosql%.exe powershell -command wget -o %arquivosql%.exe https://download.microsoft.com/download/6/c/6/6c63c0d8-b628-4b73-b524-418ed2507977/%arquivosql%.exe

echo Iniciando instalacao: %arquivosql%.exe
%arquivosql%.exe /q /ACTION=Install /FEATURES=SQL,Tools /INSTANCENAME=PDV7 /SQLSVCPASSWORD="pdv@77" /AGTSVCACCOUNT="PDV7" /AGTSVCPASSWORD="pdv@77" /AGTSVCSTARTUPTYPE="automatic" /ASSVCACCOUNT="PDV7" /ASSVCPASSWORD "pdv@77" /ASSYSADMINACCOUNTS="NT AUTHORITY" /SQLSYSADMINACCOUNTS="NT AUTHORITY" /ISSVCACCOUNT="NT AUTHORITY" /ISSVCPASSWORD="pdv@77" /RSSVCACCOUNT="NT AUTHORITY" /RSSVCPASSWORD="pdv@77"  /BROWSERSVCSTARTUPTYPE="automatic" /SQLSVCACCOUNT="PDV7"  /IACCEPTSQLSERVERLICENSETERMS
if exist "C:\Program Files\Microsoft SQL Server\MSSQL12.PDV7" set errorlevel=1
if "%errorlevel%"=="3" goto erro
echo SQL Instalado com sucesso
echo.
timeout /t 10
goto fim

:erro
if "%errorlevel%"=="1" echo SQL ja instalado
if "%errorlevel%"=="2" echo Arquitetura nao detectada
if "%errorlevel%"=="3" echo SQL nao foi instalado com sucesso
echo.
timeout /t 30

:fim