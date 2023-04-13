rmdir CodeCoverage\files /S /Q
rmdir CodeCoverage\out /S /Q
mkdir CodeCoverage\files
cd ..
cd ..
cd threadit-api-tests
rmdir TestResults /S /Q
dotnet test --collect "Code Coverage"
cd TestResults
rem following line moves into first (and only) directory in the folder
for /d %%a in (*) do cd "%%~a"

ren *.coverage results.coverage
copy results.coverage ..\..\..\devtools\api-code-coverage\CodeCoverage\files\results.coverage
cd ..\..\..\devtools\api-code-coverage\CodeCoverage
CodeCoverage.exe analyze /output:files\report.xml files\results.coverage
reportgenerator -reports:files\report.xml -targetdir:out -assemblyfilters:+ThreaditAPI.dll -classfilters:-ThreaditAPI.Database.DbInitializer;-ThreaditAPI.Database.PostgresDBContext;-ThreaditAPI.OptionalRouteParameterOperationFilter;-ThreaditAPI.Program;-ThreaditAPI.Migrations.AddCommentIsDeleted;-ThreaditAPI.Migrations.InitialCreate;-ThreaditAPI.Migrations.PostgresDbContextModelSnapshot;-ThreaditAPI.Migrations.Interests;-ThreaditAPI.Migrations.ThreadType
out\index.html
cd ..