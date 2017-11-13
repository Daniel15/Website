dotnet restore
dotnet publish Daniel15.Web -o "C:\TempPublish\site" -c Release
dotnet publish Daniel15.Cron -o "C:\TempPublish\cron" -c Release
