using System.ComponentModel;

namespace Server.WebAPI.Model.Constants
{
    public enum ResponseEnum
    {
        //
        // 摘要:
        //     成功
        [Description("成功")]
        Success = 0,
        //
        // 摘要:
        //     失败
        [Description("失败")]
        Fail = 1,
        //
        // 摘要:
        //     未授权
        [Description("未授权")]
        Unauthorized = 401,
        //
        // 摘要:
        //     授权失效
        [Description("授权失效")]
        TimeExpired = 430,
        //
        // 摘要:
        //     多语言信息返回
        [Description("多语言信息返回")]
        LangageInfo = 450,
        //
        // 摘要:
        //     导入异常返回
        [Description("导入异常返回")]
        ImportInfo = 452
    }
}
