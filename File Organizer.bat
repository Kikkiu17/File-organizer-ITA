@echo off
for /f "delims=" %%i in ('dir /b /a-d /s .\organizer-main.py') do python %%i
pause