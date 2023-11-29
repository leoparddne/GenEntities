using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonGenerateClient.Win.Helpers
{
    public class ResourceHelper
    {
        public static readonly string _resourcePath = $@"{AppDomain.CurrentDomain.BaseDirectory}CodeDesignerFile";
        /// <summary>
        /// 数据源json文件地址
        /// </summary>
        public static readonly string DataSourcePath = @"\DataSource.json";
        /// <summary>
        /// 功能json文件地址
        /// </summary>
        public static readonly string FunDesignerPath = @"\FunDesigner.json";

        /// <summary>
        /// 生成用的临时文件
        /// </summary>
        public static readonly string TempFilePath = @"\Temp.json";

        /// <summary>
        /// 架构生成临时文件
        /// </summary>
        public static readonly string TempInfrastructPath = "InfrauctTemp.json";

        /// <summary>
        ///运行目录
        /// </summary>
        public static readonly string TargetDir = AppDomain.CurrentDomain.BaseDirectory;



        public static string GetDir(string path)
        {
            return _resourcePath + path;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<T> GetDataSource<T>()
        {
            return GetData<T>(_resourcePath, DataSourcePath);
        }

        /// <summary>
        /// 保存数据源
        /// </summary>
        /// <param name="dataSources"></param>
        public static void SaveDataSource(object dataSources)
        {
            SaveData(_resourcePath, DataSourcePath, dataSources);
        }

        /// <summary>
        /// 保存架构数据源
        /// </summary>
        /// <param name="dataSources"></param>
        public static void SaveInfrastructDataSource(string path, object dataSources)
        {
            SaveData(_resourcePath, path, dataSources);
        }


        /// <summary>
        /// 获取功能列表
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<T> GetFunDesigner<T>()
        {
            return GetData<T>(_resourcePath, FunDesignerPath);
        }

        /// <summary>
        /// 保存功能列表
        /// </summary>
        /// <param name="funDesigner"></param>
        public static void SaveFunDesigner(object funDesigner)
        {
            SaveData(_resourcePath, FunDesignerPath, funDesigner);
        }

        /// <summary>
        /// 保存临时
        /// </summary>
        /// <param name="funDesigner"></param>
        public static void SaveTempFile(object tempFile)
        {
            SaveData(_resourcePath, TempFilePath, tempFile);
        }



        private static void SaveData(string path, string fileName, object funDesigner)
        {
            if (!File.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(path + fileName, JsonConvert.SerializeObject(funDesigner));
        }

        private static ObservableCollection<T> GetData<T>(string path, string fileName)
        {
            ObservableCollection<T> dataList = new();
            if (!File.Exists(path + fileName))
                return dataList;
            using StreamReader file = File.OpenText(path + fileName);
            using JsonTextReader reader = new(file);
            JArray array = (JArray)JToken.ReadFrom(reader);
            dataList = array.ToObject<ObservableCollection<T>>();
            return dataList;
        }
    }
}
