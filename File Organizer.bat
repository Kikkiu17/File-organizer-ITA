@echo off
setlocal EnableDelayedExpansion

echo Cerco il file dello script...
for /f "delims=" %%a in ('dir /s /b organizer-main.py') do set "string=%%a"

rem Do the split:
set i=1
set "fn!i!=%string:organizer-main.py=" & set /A i+=1 & set "fn!i!=%"
set fn
cd %fn1%
python organizer-main.py
pause
