# 使用T4模板自动生成需要的数据库实体类、仓储类、仓储接口

提供两种项目
1. AutoInfrastructGenerate - 提供基础生成实体(实体基础结构-不读取数据库-可手动扩展读取特定数据库实体类型)、仓储
2. GenEntities-Mysql - 读取mysql中的表自动生成实体及仓储

### AutoInfrastructGenerate
1. Entity.tt中提供了主要的逻辑
2. Manager.ttinclude中提供了文件相关操作

### 基本数据配置
```
string baseNamespaceName="MES.Server.Manufacture"; //基础名称空间
    string entityNamespaceName="Domain.Entity"; //实体名称空间
    string repositoryNamespaceName="Infrastruct.Repository"; //仓储名称空间
    string iRepositoryNamespaceName="Domain.IRepository"; //仓储接口名称空间
    string tableDesc="测试表";
    string tableName="t_pd_wo_issue_material";
    string entityFileName="WorkorderIssueMaterial"; //实体基本名称
    string entityName=entityFileName+"Entity"; //实体文件名称
    string repositoryName=entityFileName+"Repository"; //仓储名称
    string iRepositoryName="I"+repositoryName; //仓储基类

    var manager = Manager.Create(Host, GenerationEnvironment);

    bool isBaseRepository=true; //是否生成基础仓储模型
```

### 文件相关操作
```
var manager = Manager.Create(Host, GenerationEnvironment);
//开始文件写入,后续输出将写入此文件
manager.StartNewFile(entityName + ".cs","Entities"); //Entities为保存的目录名称

manager.EndBlock(); //结束当前文件写入

manager.Process(true); //解析文件,将生成的数据导入配置的文件中
```

### GenEntities-Mysql
主要逻辑在Generate.tt文件中

另外Manager.ttinclude文件中包含了创建文件的操作,使用此方法需要将Generate.tt文件中hostspecific设置为true
```
<#@ template debug="false" hostspecific="true" language="C#" #>
```

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
