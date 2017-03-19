::: ASP.NET Core view precompilation doesn't work when cross-compiling for Linux 
::: (https://github.com/aspnet/MvcPrecompilation/issues/102). This works around it by building first
::: for Windows, then building again for Debian, then copying across the view assembly.
dotnet restore
dotnet publish Daniel15.Web -o "C:\TempPublish\site_win" -c Release
dotnet restore -r debian-x64
dotnet publish Daniel15.Web -o "C:\TempPublish\site" -c Release -r debian-x64 /p:MvcRazorCompileOnPublish=false
dotnet publish Daniel15.Cron -o "C:\TempPublish\cron" -c Release -r debian-x64

copy c:\TempPublish\site_win\Daniel15.Web.PrecompiledViews.dll c:\TempPublish\site\
rd /s /q c:\TempPublish\site\Views