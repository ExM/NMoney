# Development
Generate source code by saved list_one.xml 
```
dotnet run --project ./SourceCodeRenderer/SourceCodeRenderer.csproj -- -i=NMoney/Iso4217/doc -o=NMoney/Iso4217
```

Update list_one.xml and generate source code
```
dotnet run --project ./SourceCodeRenderer/SourceCodeRenderer.csproj -- -u -i=NMoney/Iso4217/doc -o=NMoney/Iso4217
```

Restore dependencies
```
dotnet tool restore
dotnet restore
```

Build
```
dotnet build -c:Release ./NMoney.sln
```

Run test
```
dotnet test -c:Release
```

Pack
```
dotnet paket pack ./Release
```