# MongodbImporter
Mongodb批量导入工具，采用MIT协议。

使用Mongodb的导入批量文件的时候可能会各种报错，为了快速达到目的，写了这个程序，并支持并行导入。



生成之后, 找到Dll, 使用方法：

        -source    Mongodb的bin目录,默认当前目录.
        -from      Mongodb的备份目录,默认当前目录.
        -error     该程序的错误日志输出,默认当前目录.
        -processor 启用的进程数,默认为CPU核数.
        -delay     进程池枯竭时轮询等待时间,由于是采用文件附加方式入库,速度较快,默认200毫秒.



Windows脚本如下：

    @echo off
    cls
    set source=F:\SOFT-MG\bin\
    set from=D:\MongoDB_Backup\
    set error=D:\Error_Log\
    set processor=4
    set args=-source %source% -from %from% -processor %processor% -error %error%
    
    dotnet C:\Users\Administrator\Desktop\Vs2017Test\MongoDbTest\MongoDbTest\bin\Release\netcoreapp2.0\MongoDbTest.dll %args%
    
    if %errorlevel% equ 0 (
    	color 0A
    	echo 执行成功！
    ) else (
    	color 0c
    	echo 执行失败！
    )
    pause >nul


