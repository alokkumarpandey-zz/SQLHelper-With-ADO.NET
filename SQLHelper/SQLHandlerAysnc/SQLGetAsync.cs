using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
namespace SQLHelper
{
    public partial class SQLGetAsync : SQLHandlerAsync
    {

        public SQLGetAsync()
        {
            _connectionString = Connectionconfig;
        }

        /// <summary>
        /// Execute As DataSet
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteAsDataSetAsync(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SqlDataAdapter SQLAdapter = new SqlDataAdapter();
                    DataSet SQLds = new DataSet();
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;

                    //Loop for Paramets
                    for (int i = 0; i < ParaMeterCollection.Count; i++)
                    {
                        SqlParameter sqlParaMeter = new SqlParameter();
                        sqlParaMeter.IsNullable = true;
                        sqlParaMeter.ParameterName = ParaMeterCollection[i].Key;
                        sqlParaMeter.Value = ParaMeterCollection[i].Value;
                        SQLCmd.Parameters.Add(sqlParaMeter);
                    }
                    //End of for loop

                    SQLAdapter.SelectCommand = SQLCmd;
                    try
                    {
                        await SQLConn.OpenAsync();
                        SQLAdapter.Fill(SQLds);
                        SQLConn.Close();
                        return SQLds;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }


        /// <summary>
        /// Execute As Object
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsObjectAsync<T>(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    //Loop for Parameters
                    for (int i = 0; i < ParaMeterCollection.Count; i++)
                    {
                        SqlParameter sqlParaMeter = new SqlParameter();
                        sqlParaMeter.IsNullable = true;
                        sqlParaMeter.ParameterName = ParaMeterCollection[i].Key;
                        sqlParaMeter.Value = ParaMeterCollection[i].Value;
                        SQLCmd.Parameters.Add(sqlParaMeter);
                    }
                    //End of for loop
                    try
                    {
                        SqlDataReader SQLReader;
                        await SQLConn.OpenAsync();
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                        ArrayList arrColl = DataSourceHelper.FillCollection(SQLReader, typeof(T));
                        SQLConn.Close();
                        if (SQLReader != null)
                        {
                            SQLReader.Close();
                        }
                        if (arrColl != null && arrColl.Count > 0)
                        {
                            return (T)arrColl[0];
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Execute As Object
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsObjectAsync<T>(string StroredProcedureName)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        SqlDataReader SQLReader;
                        await SQLConn.OpenAsync();
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                        ArrayList arrColl = DataSourceHelper.FillCollection(SQLReader, typeof(T));
                        SQLConn.Close();
                        if (SQLReader != null)
                        {
                            SQLReader.Close();
                        }
                        if (arrColl != null && arrColl.Count > 0)
                        {
                            return (T)arrColl[0];
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Execute As DataSet
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteAsDataSetAsync(string StroredProcedureName)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SqlDataAdapter SQLAdapter = new SqlDataAdapter();
                    DataSet SQLds = new DataSet();
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SQLAdapter.SelectCommand = SQLCmd;
                    try
                    {
                        await SQLConn.OpenAsync();
                        SQLAdapter.Fill(SQLds);
                        SQLConn.Close();
                        return SQLds;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Execute As Scalar 
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsScalarAsync<T>(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    //Loop for Paramets
                    for (int i = 0; i < ParaMeterCollection.Count; i++)
                    {
                        SqlParameter sqlParaMeter = new SqlParameter();
                        sqlParaMeter.IsNullable = true;
                        sqlParaMeter.ParameterName = ParaMeterCollection[i].Key;
                        sqlParaMeter.Value = ParaMeterCollection[i].Value;
                        SQLCmd.Parameters.Add(sqlParaMeter);
                    }
                    //End of for loop
                    try
                    {
                        await SQLConn.OpenAsync();
                        object t = await SQLCmd.ExecuteScalarAsync();
                        return (T)t;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Execute As Scalar
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoredProcedure Name</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsScalarAsync<T>(string StroredProcedureName)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        await SQLConn.OpenAsync();
                        object t = await SQLCmd.ExecuteScalarAsync();
                        return (T)t;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        SQLConn.Close();
                    }
                }
            }
        }
    }
}
