For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c_%%a_%%b)
For /f "tokens=1-2 delims=/:" %%a in ("%TIME: =0%") do (set mytime=%%a%%b)
REM dotnet tool install --global dotnet-ef --version 3.0.0-*
REM dotnet tool update --global dotnet-ef --version 3.0.0-*
REM dotnet build
dotnet ef migrations --startup-project ../IdentityCookie.App/ add V%mydate%_%mytime%
pause