rem --
rem -- We can force the version of build and NuGet package with '-p:version=', but it is better to update it in .csproj file
rem --
dotnet build -c Release
dotnet pack CSharpMongoMigrations/CSharpMongoMigrations.csproj /p:NuspecFile=../CSharpMongoMigrations.nuspec --output ../