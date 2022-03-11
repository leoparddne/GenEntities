using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace T4Ext
{
    public class T4Command : CommandBase
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
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 外部程序集
        /// </summary>
        public List<string> Assembly { get; set; } = new List<string>();

        /// <summary>
        /// 名称空间
        /// </summary>
        public List<string> Namespace { get; set; } = new List<string>();

        /// <summary>
        /// ttinclude
        /// </summary>
        public List<string> TTIncludeDir { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programePath">D:\VS2022\Enterprise\Common7\IDE\TextTransform.exe</param>
        public T4Command(string programePath) : base(programePath)
        {
        }

        public T4Command SetT4Path(string t4Path)
        {
            this.T4Path = t4Path;
            return this;
        }

        public T4Command SetOutFilePath(string outFilePath)
        {
            this.OutFilePath = outFilePath;
            return this;
        }

        public override void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        public override void Process_Exited(object sender, EventArgs e)
        {

        }

        public override void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        /// <summary>
        /// 生成
        /// </summary>
        public void Generate()
        {
            //添加文件
            AddParameter(T4Path);
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

            AddParameter($"-out {OutFilePath}");

            //ttinclude
            foreach (string item in TTIncludeDir)
            {
                AddParameter($" -I {item}");
            }
            Exec();
        }

    }
}
