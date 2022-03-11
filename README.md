# 使用T4模板自动生成需要的数据库实体类、仓储类、仓储接口

提供的项目
1. AutoInfrastructGenerate - 提供基础生成实体(实体基础结构-不读取数据库-可手动扩展读取特定数据库实体类型)、仓储 -T4模板
2. GenEntities-Mysql - 读取mysql中的表自动生成实体及仓储
3. OracleGenerate - 读取oracle中的表自动生成实体 -读取oracle基表

### AutoInfrastructGenerate
1. Entity.tt中提供了主要的逻辑
2. Manager.ttinclude中提供了文件相关操作

### QA 
如果遇到类型“DTE”同时存在于“EnvDTE, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a”和“Microsoft.VisualStudio.Interop, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a”中	
，可以移除Manager.ttinclude内的<#@ assembly name="EnvDTE" #>


### OracleGenerate
配置文件 - 数据库连接字符串
![image](https://user-images.githubusercontent.com/13193677/148872674-41a57622-7744-48b6-b5c5-981fce7340ea.png)
![image](https://user-images.githubusercontent.com/13193677/149716899-fcac5a1b-d499-4e74-b7de-b25885cc96f3.png)
![image](Imgimage.png)

生成架构按钮将调用OracleGenerate目录下的Infrastruct.tt模板生成基础的架构  
需要调整对应的配置如下图
![输入图片说明](config.png)
后续将调整为配置--暂未处理

### 原理
```
-- 获取用户创建的表
select table_name from user_tables 
select * from user_tables where Table_Name = 'T_USER';

--获取表字段
select* from user_tab_columns where Table_Name = 'T_USER';

--获取表注释
select* from user_tab_comments user_tab_comments where Table_name='T_USER';

--获取字段注释
select * from user_col_comments where Table_name='T_USER';

--获取主键
select * --col.column_name 
from user_constraints con,  user_cons_columns col 
where con.constraint_name = col.constraint_name 
and con.constraint_type='P' 
and col.table_name = 'T_USER'
```


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
