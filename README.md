# SQLHelper With ADO.NET
Use pure ADO.NET with SQLHelper for performance critical applications

If you working with .NET framework from a long time then you must have used **SQL Helper** or a similar library to work with SQL Server or if you working with some performance-critical applications and want to directly work with ADO.NET then you can use your ADO.NET with this SQL helper. I tried to convert it so that it can work with **ADO.NET** as well as achieve async feature as well in **.NET Core 2.x.**

So this SQL Helper Library basically has two main class file, that can be used easily for DML operations of SQL Database. A one just has to create an object and start using it.
The two base class are,

* SQLHandler
* SQLHandlerAsync

And **DML Class** like,
*SQLAddUpdate
*SQLGet
*SQLGetList
*SQLRaw
*SQLAddUpdateAsync
*SQLGetAsync
*SQLGetListAsync
*SQLRawAsync

And **helper classes** are

* DataSourceHelper
* Null

So these classes of DML derived from **SQLHandler and SQLHandlerAsync** classes and have pure **ADO.NET methods** like,

* ExecuteNonQuery and  ExecuteNonQueryAsync,
* ExecuteAsDataSet and ExecuteAsDataSetAsync,
* ExecuteAsDataReader and ExecuteAsDataReaderAsync,
* ExecuteAsObject and ExecuteAsObjectAsync,
* ExecuteAsList and ExecuteAsListAsync

Which can be used directly and can provide the best performance as required and handled from SQL Stored Procedures.

![ADO.NET COMPONENTS](https://github.com/alokkumarpandey/SQLHelper-With-ADO.NET/blob/master/ADODOTNETCOMPONENTS.png
)

This SQLHandler and SQLHandlerAsync use NuGetPackage **System.Data.SqlClient** to support pure ADO.NET features like **DataTable and DataSet** etc.

Examples
**How to use SQLAddUpdateAsync for ExecuteNonQueryAsync**

```
public async Task<int> AddProjectAsync(ProjectInfo objInfo)
{
    List<SQLParam> Param = new List<SQLParam>();
    Param.Add(new SQLParam("@ProjectName", objInfo.ProjectName));
    Param.Add(new SQLParam("@ProjectCode", objInfo.ProjectCode));
    string strSpName = "usp_AddProject";
    SQLAddUpdateAsync sqlHAsy = new SQLAddUpdateAsync();
    return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
}
```

**How to use SQLGetAsync for ExecuteAsObjectAsync**

```
public async Task<ProjectInfo> GetProjectAsync(ProjectInfo objInfo)
{
    List<SQLParam> Param = new List<SQLParam>();
    Param.Add(new SQLParam("@ProjectID", objInfo.ProjectID));
    string strSpName = "usp_GetProject";
    SQLGetAsync sqlHAsy = new SQLGetAsync();            
    return await sqlHAsy.ExecuteAsObjectAsync<ProjectInfo>(strSpName, Param);
}
```

**How to use SQLGetListAsync for ExecuteAsListAsync**

```
public async Task<IList<ProjectInfo>> GetProjectAsListAsync()
{
    string strSpName = "usp_GetProjectList";
    SQLGetListAsync sqlHAsy = new SQLGetListAsync();
    return await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName);
}
```


This repository has 3 projects, but SQL Helper is the main the rest two projects are just to test and shown as an example to use it.

A SQLHandler and SQLHandlerAsync both are partial classes so if required it can also accept more options too.

As these classes are for MS SQL, it would be great if some friends can extend it for the mysql and mongodb etc too.
