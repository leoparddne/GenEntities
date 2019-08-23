# GenEntities
automaticly generate entity object from db
使用T4创建
在Generate.tt中修改要生成的数据库连接信息,添加要生成的数据库
如下：
// 数据库连接
    var connectionString = @"Server=127.0.0.1;port=3306;database=Test;charset=utf8;uid=test;password=test";
 
    // 需要解析的数据库
    var database = new List<string> { "test"};
Ctrl+s保存即可，生成文件的位置在entities文件夹中

如果需要修改生成文件保存的路径，修改Manager.ttinclude文件中的输出目录即可
//输出目录
    private readonly string FinalPath="entities";
