using System.ComponentModel;

namespace CommonGenerateClient.Win.ViewModel
{
    /// <summary>
    /// 参数 - 表映射
    /// </summary>
    public class ParameterMapper : INotifyPropertyChanged
    {
        public string Parameter { get; set; }


        public SelectTypeModel SelectTable { get; set; }


        /// <summary>
        /// 表前缀
        /// </summary>
        public string TablePreFix { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
