using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SQLHelper
{
    public partial class SQLRawAsync : SQLHandlerAsync
    {
        public SQLRawAsync()
        {
            _connectionString = Connectionconfig;
        }

        /// <summary>
        /// Execute As DataReader
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <returns></returns>
        public async Task<SqlDataReader> ExecuteAsDataReaderAsync(string StroredProcedureName)
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

                        return SQLReader;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Execute As DataReader
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name </param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<SqlDataReader> ExecuteAsDataReaderAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
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
                        SQLReader = await SQLCmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                        return SQLReader;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

        }


        /// <summary>
        /// Execute SQL
        /// </summary>
        /// <param name="SQL">SQL query in string</param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteSQLAsync(string SQL)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {

                SQL = SQL.Replace("{databaseOwner}", this.databaseOwner);
                SQL = SQL.Replace("{objectQualifier}", this.objectQualifier);

                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SqlDataAdapter SQLAdapter = new SqlDataAdapter();
                    DataSet SQLds = new DataSet();
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = SQL;
                    SQLCmd.CommandType = CommandType.Text;
                    SQLAdapter.SelectCommand = SQLCmd;
                    try
                    {
                        await SQLConn.OpenAsync();
                        SQLAdapter.Fill(SQLds);
                        SQLConn.Close();
                        DataTable dt = null;// = new DataTable();
                        if (SQLds != null && SQLds.Tables != null && SQLds.Tables[0] != null)
                        {
                            dt = SQLds.Tables[0];
                        }
                        return dt;
                    }
                    catch
                    {
                        DataTable dt = null;
                        return dt;
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
