set /p migrationsname=ÇëÊäÈëÇ¨ÒÆÃû³Æ:
dotnet ef migrations add %migrationsname% --project ../Infrastructure/Infrastructure.csproj
pause