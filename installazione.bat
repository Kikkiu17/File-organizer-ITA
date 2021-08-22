@echo off
cls & color 0B
Mode con cols=110 lines=
set Location=C:\Python\Python370
set FileName=python.exe
echo( & cls
echo(  & echo  Sto cercando l'eseguibile di Python ("%FileName%") nella directory di default "%Location%"
TimeOut /T 1 /NoBreak>Nul
cls
IF EXIST "%Location%\%FileName%" ( color 0A && echo Python e' gia' installato. Inizio installazione moduli necessari...
TimeOut /T 3 /NoBreak>Nul
start cmd.exe /k "python -m pip install --upgrade pip && pip install python-magic && pip install inflect && pip install python-magic-bin && pip install requests && pip install pip install pywin32 && echo ----------- Tutti i moduli sono stati installati, si puo' chiudere questa finestra -----------"
) ELSE (
    Color 0C & echo Python non e' installato, inizio dell'installazione automatica...
    TimeOut /T 5 /NoBreak>Nul
Color 07
:errorNoPython
echo.
echo.
echo.
echo Download di Python 3.7.0 in corso...
IF EXIST "%CD%\python-3.7.0.exe" (
  echo Trovato installer in "%CD%\python-3.7.0.exe"
) ELSE (
  powershell -Command "& {[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12, [Net.SecurityProtocolType]::Tls11, [Net.SecurityProtocolType]::Ssl3, [Net.SecurityProtocolType]::Tls; Invoke-WebRequest -Uri 'https://www.python.org/ftp/python/3.7.0/python-3.7.0.exe' -OutFile '%CD%\python-3.7.0.exe';}"
  echo Download completato.
)

echo Installazione di Python in corso... Non chiudere questa finestra
powershell %CD%\python-3.7.0.exe /quiet InstallAllUsers=0 PrependPath=1 Include_test=0 TargetDir=c:\Python\Python370
setx path "%PATH%;C:\Python\Python370\"

timeout /t 15 /nobreak > nul
echo Installazione di Python completata. Inizio installazione dei moduli necessari in corso...
timeout /t 3 /nobreak > nul
start cmd.exe /k "python -m pip install --upgrade pip && pip install python-magic && pip install inflect && pip install python-magic-bin && pip install requests && pip install pip install pywin32 && echo ----------- Tutti i moduli sono stati installati, si puo' chiudere questa finestra -----------"
)