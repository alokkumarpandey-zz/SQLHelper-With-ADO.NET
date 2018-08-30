# SQLHelper With ADO.NET
Use pure ADO.NET with SQLHelper for performance critical applications

If you working with .NET framework from a long time then you must have used **SQL Helper** or a similar library to work with SQL Server or if you working with some performance-critical applications and want to directly work with ADO.NET then you can use your ADO.NET with this SQL helper. I tried to convert it so that it can work with **ADO.NET** as well as achieve async feature as well in **.NET Core 2.x.**

So this SQL Helper Library basically has two main class file, that can be used easily for DML operations of SQL Database. A one just has to create an object and start using it.
The two class are,

* SQLHandler
* SQLHandlerAsync

And **helper classes** are

* DataSourceHelper
* Null

So this two **SQLHandler and SQLHandlerAsync** classes have pure **ADO.NET methods** like,

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
**How to use SQLHandlerAsync for ExecuteNonQueryAsync**

```
public async Task<int> AddProject(ProjectInfo objInfo)
{
    List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
    Param.Add(new KeyValuePair<string, object>("@ProjectName", objInfo.ProjectName));
    Param.Add(new KeyValuePair<string, object>("@ProjectCode", objInfo.ProjectCode));
    string strSpName = "usp_AddProject";
    SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
    return await sqlHAsy.ExecuteNonQueryAsync(strSpName, Param);
}
```

**How to use SQLHandlerAsync for ExecuteAsObjectAsync**

```
public async Task<ProjectInfo> GetProject(ProjectInfo objInfo)
{
    List<KeyValuePair<string, object>> Param = new List<KeyValuePair<string, object>>();
    Param.Add(new KeyValuePair<string, object>("@ProjectID", objInfo.ProjectID));            
    string strSpName = "usp_GetProject";
    SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();
    //List<ProjectInfo> objList = await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName, Param);
    return await sqlHAsy.ExecuteAsObjectAsync<ProjectInfo>(strSpName, Param);
}
```

**How to use SQLHandlerAsync for ExecuteAsListAsync**

```
public async Task<List<ProjectInfo>> GetProjectList()
{    
    string strSpName = "usp_GetProjectList";
    SQLHandlerAsync sqlHAsy = new SQLHandlerAsync();    
    return await sqlHAsy.ExecuteAsListAsync<ProjectInfo>(strSpName);
}
```


This repository has 3 projects, but SQL Helper is the main the rest two projects are just to test and shown as an example to use it.

A SQLHandler and SQLHandlerAsync both are partial classes so if required it can also accept more options too.

As these classes are for MS SQL, it would be great if some friends can extend it for the mysql and mongodb etc too.
