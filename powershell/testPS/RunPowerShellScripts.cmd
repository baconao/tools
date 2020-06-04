powershell -nologo -noprofile  set-executionpolicy RemoteSigned
FOR /f %%A IN ('dir /b "%~dp0*.ps1"') DO (
		powershell -nologo -noprofile -Command ". \"%~dp0%%A""  \
)
pause


