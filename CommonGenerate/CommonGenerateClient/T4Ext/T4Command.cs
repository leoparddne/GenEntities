using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4Ext
{
    public class T4Command : ProcessCommandBase
    {
        /// <summary>
        /// T4文件路径
        /// </summary>
        public string T4Path { get; set; }

        /// <summary>
        /// 输出文件名称
        /// </summary>
        public string OutFilePath { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// 外部程序集
        /// </summary>
        public List<string> Assembly { get; set; } = new();

        /// <summary>
        /// 名称空间
        /// </summary>
        public List<string> Namespace { get; set; } = new();

        /// <summary>
        /// ttinclude
        /// </summary>
        public List<string> TTIncludeDir { get; set; } = new();

        /// <summary>
        /// 是否已添加T4文件参数到命令行中
        /// </summary>
        public bool AddedT4Path { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programePath">D:\VS2022\Enterprise\Common7\IDE\TextTransform.exe</param>
        public T4Command(string programePath) : base(programePath)
        {
        }

        public T4Command SetT4Path(string t4Path, bool addParameter = false)
        {
            this.T4Path = t4Path;
            if (addParameter)
            {
                AddParameter($"\"{T4Path}\"");
                AddedT4Path = true;
            }
            return this;
        }

        public T4Command SetOutFilePath(string outFilePath)
        {
            this.OutFilePath = outFilePath;
            return this;
        }

        public override void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.Data))
            //    LogHelper.WriteLogForCustom(e.Data?.ToString());
            Trace.WriteLine(e.Data?.ToString());
        }

        public override void Process_Exited(object sender, EventArgs e)
        {
            Trace.WriteLine(e.ToString());
            Trace.WriteLine("ProcessFinish&exit");
        }

        public override void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Trace.WriteLine(e.Data.ToString());

        }

        /// <summary>
        /// 生成
        /// </summary>
        public void Generate()
        {
            //添加文件
            if (!AddedT4Path)
            {
                AddParameter($"\"{T4Path}\"");
            }
            foreach (string item in Assembly)
            {
                AddParameter($"-r \"{item}\"");
            }

            foreach (string item in Namespace)
            {
                AddParameter($"-u \"{item}\"");
            }

            foreach (KeyValuePair<string, object> item in Parameters)
            {
                string parameters = JsonConvert.SerializeObject(item.Value).ToString();
                parameters = parameters.Replace("\"", "\\\"");

                AddParameter($" -a !!{item.Key}!{parameters}");
            }

            if (!string.IsNullOrWhiteSpace(OutFilePath))
            {
                AddParameter($"-out {OutFilePath}");
            }


            //ttinclude
            foreach (string item in TTIncludeDir)
            {
                AddParameter($" -I {item}");
            }
            Exec();
        }

    }
}
