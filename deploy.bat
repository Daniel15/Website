@echo off
"%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" deploy.proj /p:Configuration=Release;DestinationDir=dan.cx