@echo off
cls
set source=F:\SOFT-MG\bin\

set from=D:\MongoDB_Backup\

set error=D:\Error_Log\

set processor=4

set args=-source %source% -from %from% -processor %processor% -error %error%


dotnet MongodbImporter.dll %args%

if %errorlevel% equ 0 (
color 0A
echo 执行成功！
) else (
color 0c
echo 执行失败！
)
pause >nul