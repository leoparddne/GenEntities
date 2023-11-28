using System;
using System.Diagnostics;
using System.Text;

namespace T4Ext
{
    public abstract class ProcessCommandBase : IDisposable
    {
        //程序名
        public string programe;
        //参数
        StringBuilder parameter = new StringBuilder();
        Process process = null;

        public ProcessCommandBase(string programe)
        {
            this.programe = programe;
        }

        public ProcessCommandBase AddParameter(string para)
        {
            parameter.Append($" {para} ");

            return this;
        }

        public void Exec()
        {
            //var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            process = new Process();
            process.StartInfo.FileName = programe;
            process.StartInfo.Arguments = parameter.ToString();
            process.StartInfo.CreateNoWindow = true;

            //重定向标准输输出、标准错误流
            process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.RedirectStandardOutput = true;

            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Exited += Process_Exited;
            //process.OutputDataReceived += Process_OutputDataReceived;
            //LogHelper.WriteLogForCustom($"Exe:{programe}");
            //LogHelper.WriteLogForCustom($"Parameter:{parameter.ToString()}");
            process.Start();
            process.BeginErrorReadLine();
            //process.BeginOutputReadLine();
        }

        public abstract void Process_OutputDataReceived(object sender, DataReceivedEventArgs e);

        public abstract void Process_Exited(object sender, EventArgs e);

        public abstract void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e);

        public void Close()
        {
            process?.Close();
            process = null;
        }

        public void Kill()
        {
            process?.Kill();
            process?.Close();
            process = null;
        }

        public void Dispose()
        {
            Kill();
        }
    }
}
