using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongodbImporter
{
    public class Executor
    {
        private ConcurrentQueue<Process> _process;
        private ConcurrentQueue<string> _queue;
        private string _errorRedirectPath;
        private string _source;
        private int _delay;

        /// <summary>
        /// 初始化执行器
        /// </summary>
        /// <param name="source">程序安装路径</param>
        /// <param name="ErrorPath">错误日志路径</param>
        /// <param name="ProcessCount">CPU核数</param>
        /// <param name="delay">延迟</param>
        public Executor(string source, string ErrorPath, int ProcessCount,int delay)
        {
            _queue = new ConcurrentQueue<string>();
            _process = new ConcurrentQueue<Process>();
            _delay = delay;
            _errorRedirectPath = ErrorPath;
            _source = source;

           
            for (int i = 0; i < ProcessCount; i+=1)
            {
                Process tProcess = new Process();
                tProcess.StartInfo.FileName = _source + @"mongorestore.exe";
                tProcess.StartInfo.CreateNoWindow = true;
                tProcess.EnableRaisingEvents = true;
                tProcess.StartInfo.RedirectStandardError = true;
                tProcess.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                tProcess.ErrorDataReceived += Executor_ErrorDataReceived;
                tProcess.Exited += Executor_Exited;
                _process.Enqueue(tProcess);
            }
        }

        /// <summary>
        /// 接受错误信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Executor_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            StringBuilder errorBuidler = new StringBuilder();
            errorBuidler.AppendLine();
            errorBuidler.AppendLine(DateTime.Now.ToString("yyyy - MM - dd HH：mm：ss")+"错误文件："+ ((Process)sender).StartInfo.Arguments);
            errorBuidler.AppendLine(DateTime.Now.ToString("yyyy - MM - dd HH：mm：ss") + "错误信息：" + e.Data);
            errorBuidler.AppendLine();
            _queue.Enqueue(errorBuidler.ToString());
        }

        /// <summary>
        /// 开始还原
        /// </summary>
        /// <param name="files">还原任务集合</param>
        public async void Start(string[] files)
        {
           
            for (int i = 0; i < files.Length; i+=1)
            {
                
                await Execute(files[i]);
            }

            if (!_queue.IsEmpty)
            {
                using (StreamWriter writer = new StreamWriter(_errorRedirectPath + "\\error.log", true, Encoding.UTF8))
                {
                    string error;
                    while (!_queue.IsEmpty)
                    {
                        while (!_queue.TryDequeue(out error)) ;
                        writer.WriteLine(error);
                    }
                }
            }
        }
        /// <summary>
        /// 执行还原任务
        /// </summary>
        /// <param name="name">任务执行参数</param>
        /// <returns></returns>
        public async Task Execute(string name)
        {
            Process executeProcessor;
            while (!_process.TryDequeue(out executeProcessor))
            {
                Thread.Sleep(_delay);
            }
            Console.WriteLine("Import : " + name);
            executeProcessor.StartInfo.Arguments = name;
            executeProcessor.Start();
        }
        /// <summary>
        /// 任务结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Executor_Exited(object sender, EventArgs e)
        {
            _process.Enqueue((Process)sender);
        }
    }
}
