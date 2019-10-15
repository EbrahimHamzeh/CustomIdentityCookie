REM dotnet tool install --global dotnet-ef --version 3.0.0-*
REM dotnet tool update --global dotnet-ef --version 3.0.0-*
REM dotnet build
dotnet ef --startup-project ../IdentityCookie.App/ database update
pause