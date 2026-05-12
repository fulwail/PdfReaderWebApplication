@echo off
set /p "name=Enter MigrationName: "
dotnet ef migrations --startup-project ../PdfReader.Api add %name% --verbose  --context PdfReaderDbContext
 set /p "name=Migration Done:"