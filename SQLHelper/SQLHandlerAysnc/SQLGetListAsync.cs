using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace SQLHelper
{
    public partial class SQLGetListAsync : SQLHandlerAsync
    {

        #region Constructor
        public SQLGetListAsync()
        {
            //Note there is lot of way to set this, please use your needed way to do this connections string vallue setting
            _connectionString = Connectionconfig;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Execute As list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection"></param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteAsListAsync<T>(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(_connectionString))
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
                        SqlDataReader SQLReader;
                        await SQLConn.OpenAsync();
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
                        List<T> mList = new List<T>();
                        mList = DataSourceHelper.FillCollection<T>(SQLReader);
                        if (SQLReader != null)
                        {
                            SQLReader.Close();
                        }
                        SQLConn.Close();
                        return mList;
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
        /// Execute As List
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">Storedprocedure Name</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteAsListAsync<T>(string StroredProcedureName)
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
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
                        List<T> mList = new List<T>();
                        mList = DataSourceHelper.FillCollection<T>(SQLReader);
                        if (SQLReader != null)
                        {
                            SQLReader.Close();
                        }
                        SQLConn.Close();
                        return mList;
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

        #endregion
    }
}
