using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4Ext
{
    public abstract class CommandBase: IDisposable
    {
        //程序名
        public string programe;
        //参数
        StringBuilder parameter = new StringBuilder();
        Process process = null;

        public CommandBase(string programe)
        {
            this.programe = programe;
        }

        public CommandBase AddParameter(string para)
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
