# GenEntities,使用T4模板自动生成需要的数据库实体类、仓储类、仓储接口

##更新
1. 添加对仓储模式自动生成的支持,可用通过修改Generate.tt文件中的needRepository改为true生成实体对应的仓储  
2. 可以配置仓储对应的实体类、仓储接口、仓储类、上下文类的名称空间以及上下文类的配置
```
//配置默认的名称空间
	var nameSpace="TestNameSpace";
	//实体类名称空间
	var EntityNameSpace="TestEntity";
	//仓储接口名称空间
	var IRepositoryNameSpace="TestRepository.IRepository";
	//仓库名称空间
	var RepositoryNameSpace="TestRepository.Repository";
	//上下文类
	var dbContextType="TestCommonContext";
	//上下文类名称空间
	var DBContext="TestRepository.DbContexts";
```

数据库的相关配置如下 
```
数据库连接
var connectionString = @"Server=127.0.0.1;port=3306;database=tianleclass;charset=utf8;uid=root;password=root";
// 需要解析的数据库
var database = new List<string> { "tianleclass" };
```
如需将生成的文件输出到指定的位置，可以找到manager.StartNewFile方法的调用  
第二个参数为希望生成的目录，基于项目目录的相对路径，尝试一下即可  
另外预留了WCF的配置  
```
// 是否是WCF服务模型
bool serviceModel = false;
```
