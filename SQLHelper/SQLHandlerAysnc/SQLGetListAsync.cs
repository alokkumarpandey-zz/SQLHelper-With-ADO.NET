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
        public async Task<IList<T>> ExecuteAsListAsync<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                    SQLCmd.Parameters.AddRange(sqlParameters);
                    try
                    {
                        SqlDataReader SQLReader;
                        await SQLConn.OpenAsync();
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
                        IList<T> mList = DataSourceHelper.FillCollection<T>(SQLReader);
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
        public async Task<IList<T>> ExecuteAsListAsync<T>(string StroredProcedureName)
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
                        IList<T> mList = DataSourceHelper.FillCollection<T>(SQLReader);
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
        /// Execute As list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteAsEnumerableAsync<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                    SQLCmd.Parameters.AddRange(sqlParameters);
                    try
                    {
                        SqlDataReader SQLReader;
                        await SQLConn.OpenAsync();
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
                        IEnumerable<T> mList = DataSourceHelper.FillCollection<T>(SQLReader);
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
        public async Task<IEnumerable<T>> ExecuteAsEnumerableAsync<T>(string StroredProcedureName)
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
                        IEnumerable<T> mList = DataSourceHelper.FillCollection<T>(SQLReader);
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
        /// Execute As DataSet
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteAsDataSetAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
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

                    SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                    SQLCmd.Parameters.AddRange(sqlParameters);

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

        #endregion
    }
}
