set /p migrationsname=������Ǩ������:
dotnet ef migrations add %migrationsname% --project ../Infrastructure/Infrastructure.csproj
pause