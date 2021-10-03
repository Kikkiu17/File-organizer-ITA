@echo off
SET mypath=%~dp0
echo INSTALLAZIONE DI PYTHON E DEI MODULI NECESSARI, LA FINESTRA SI CHIUDERA' DA SOLA

powershell -command "& {Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force}"

For /F %%A in ('"dir /b /od C:\|findstr ^Python"') do set myVar=%%A

echo C:\%myVar%

IF [%myVar%] == [] (
    echo Python non e' installato, inizio dell'installazione automatica... NON CHIUDERE QUESTA FINESTRA
    TimeOut /T 5 /NoBreak>Nul
powershell -command "%mypath%py-lib.ps1"
) ELSE (
 echo Python e' gia' installato, inizio a installare le librerie necessarie. NON CHIUDERE QUESTA FINESTRA, SI CHIUDERA' IN AUTOMATICO
TimeOut /T 5 /NoBreak>Nul
powershell -command "py -m pip install requests
py -m pip install inflect
py -m pip install PySimpleGUI
py -m pip install python-magic-bin==0.4.14
py -m pip install pywin32
py -m pip install pyinstaller"
)