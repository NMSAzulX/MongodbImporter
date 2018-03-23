using System;
using System.Text;

namespace MongodbImporter
{
    class Program
    {
        static void Main(string[] args)
        {

            string source = AppDomain.CurrentDomain.BaseDirectory;
            string from = source;
            string error = source;

            int processor = 0;
            int delay = 200;

            for (int i = 0; i < args.Length; i += 1)
            {
                switch (args[i])
                {
                    case "-source":
                        i += 1;
                        source = args[i];
                        break;
                    case "-error":
                        i += 1;
                        error = args[i];
                        break;
                    case "-from":
                        i += 1;
                        from = args[i];
                        break;
                    case "-processor":
                        i += 1;
                        processor = Convert.ToInt32(args[i]);
                        break;
                    case "-delay":
                        i += 1;
                        delay = Convert.ToInt32(args[i]);
                        break;
                    default:
                        StringBuilder help = new StringBuilder();
                        help.AppendLine();
                        help.AppendLine("\t-source    Mongodb的bin目录,默认当前目录.");
                        help.AppendLine("\t-from      Mongodb的备份目录,默认当前目录.");
                        help.AppendLine("\t-error     该程序的错误日志输出,默认当前目录.");
                        help.AppendLine("\t-processor 启用的进程数,默认为CPU核数.");
                        help.AppendLine("\t-delay     进程池枯竭时轮询等待时间,由于是采用文件附加方式入库,速度较快,默认200毫秒.");
                        Console.WriteLine(help);
                        return;
                }
            }
            if (processor == 0)
            {
                processor = 2;
            }
            FileGetter getter = new FileGetter(from);
            Executor executor = new Executor(source, error, processor,delay);
            executor?.Start(getter.FileCollection);
        }
    }
}
