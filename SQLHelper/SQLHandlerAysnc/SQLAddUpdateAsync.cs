using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace SQLHelper
{
    public partial class SQLAddUpdateAsync : SQLHandlerAsync
    {

        #region Constructor
        public SQLAddUpdateAsync()
        {
            //Note there is lot of way to set this, please use your needed way to do this connections string vallue setting
            _connectionString = Connectionconfig;
        }
        #endregion

        #region Using Transaction Method
        public int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, List<SQLParam> ParaMeterCollection, string outParamName)
        {
            //create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText);

            SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
            cmd.Parameters.AddRange(sqlParameters);
            cmd.Parameters.Add(new SqlParameter(outParamName, SqlDbType.Int));
            cmd.Parameters[outParamName].Direction = ParameterDirection.Output;

            //finally, execute the command.
            cmd.ExecuteNonQuery();
            int id = (int)cmd.Parameters[outParamName].Value;

            // detach the OracleParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return id;
        }

        public void ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, List<SQLParam> ParaMeterCollection)
        {
            //create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText);

            SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
            cmd.Parameters.AddRange(sqlParameters);

            //finally, execute the command.
            cmd.ExecuteNonQuery();

            // detach the OracleParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();

        }
        #endregion

        #region "Public Methods"


        /// <summary>
        /// Returning Bool After Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="ParaMeterCollection"> Parameter Collection</param>
        /// <param name="OutPutParamerterName">Out Parameter Collection</param>
        /// <returns>Bool</returns>
        public async Task<bool> ExecuteNonQueryAsBoolAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection, string OutPutParamerterName)
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
                    SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                    SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                    try
                    {
                        await SQLConn.OpenAsync();
                        await SQLCmd.ExecuteNonQueryAsync();
                        bool ReturnValue = (bool)SQLCmd.Parameters[OutPutParamerterName].Value;

                        return ReturnValue;
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
        /// Returning Bool After Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="ParaMeterCollection">Parameter Collection</param>
        /// <param name="OutPutParamerterName">OutPut Parameter Name</param>
        /// <param name="OutPutParamerterValue">OutPut Parameter Value</param>
        /// <returns>Bool</returns>
        public async Task<bool> ExecuteNonQueryAsBoolAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection, string OutPutParamerterName, object OutPutParamerterValue)
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
                    SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                    SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                    SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;
                    try
                    {
                        await SQLConn.OpenAsync();
                        await SQLCmd.ExecuteNonQueryAsync();
                        bool ReturnValue = (bool)SQLCmd.Parameters[OutPutParamerterName].Value;
                        return ReturnValue;
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
        /// Accept only int, Int16, long, DateTime, string (NVarcha of size  50),
        /// bool, decimal ( of size 16,2), float
        /// </summary>
        /// <typeparam name="T">Return the given type of object</typeparam>
        /// <param name="StroredProcedureName">Accet SQL procedure name in string</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection for parameters</param>
        /// <param name="OutPutParamerterName">Accept Output parameter for the stored procedures</param>
        /// <returns></returns>
        public async Task<T> ExecuteNonQueryAsGivenTypeAsync<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection, string OutPutParamerterName)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLCmd = AddOutPutParametrofGivenType<T>(SQLCmd, OutPutParamerterName);
                try
                {
                    await SQLConn.OpenAsync();
                    await SQLCmd.ExecuteNonQueryAsync();
                    return (T)SQLCmd.Parameters[OutPutParamerterName].Value;
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

        /// <summary>
        /// Accept only int, Int16, long, DateTime, string (NVarcha of size  50),
        /// bool, decimal ( of size 16,2), float
        /// </summary>
        /// <typeparam name="T">Return the given type of object</typeparam>
        /// <param name="StroredProcedureName">Accet SQL procedure name in string</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection for parameters</param>
        /// <param name="OutPutParamerterName">Accept Output parameter for the stored procedures</param>
        /// <returns></returns>
        public async Task<T> ExecuteNonQueryAsGivenTypeAsync<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection, string OutPutParamerterName, object OutPutParamerterValue)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLCmd = AddOutPutParametrofGivenType<T>(SQLCmd, OutPutParamerterName, OutPutParamerterValue);
                try
                {
                    await SQLConn.OpenAsync();
                    await SQLCmd.ExecuteNonQueryAsync();
                    return (T)SQLCmd.Parameters[OutPutParamerterName].Value; ;
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


        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        public async Task<int> ExecuteNonQueryAsync(string StroredProcedureName)
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
                        int effectedRows = 0;
                        await SQLConn.OpenAsync();
                        effectedRows = await SQLCmd.ExecuteNonQueryAsync();
                        return effectedRows;
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
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name In String</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters<SQLParam> </param>
        public async Task<int> ExecuteNonQueryAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
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
                        int effectedRows = 0;
                        await SQLConn.OpenAsync();
                        effectedRows = await SQLCmd.ExecuteNonQueryAsync();
                        return effectedRows;
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
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name In String</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <param name="OutPutParamerterName">Accept OutPut Key Value Collection For Parameters</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string StroredProcedureName, List<SQLParam> ParaMeterCollection, string OutPutParamerterName)
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
                    SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                    SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                    try
                    {
                        await SQLConn.OpenAsync();
                        await SQLCmd.ExecuteNonQueryAsync();
                        int ReturnValue = (int)SQLCmd.Parameters[OutPutParamerterName].Value;
                        SQLConn.Close();
                        return ReturnValue;
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
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="OutPutParamerterName">Accept Output For Parameters Name</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string StroredProcedureName, string OutPutParamerterName)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                    SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;

                    try
                    {
                        await SQLConn.OpenAsync();
                        await SQLCmd.ExecuteNonQueryAsync();
                        int ReturnValue = (int)SQLCmd.Parameters[OutPutParamerterName].Value;
                        SQLConn.Close();
                        return ReturnValue;
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
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="OutPutParamerterName">Accept Output For Parameter Name</param>
        /// <param name="OutPutParamerterValue">Accept Output For Parameter Value</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string StroredProcedureName, string OutPutParamerterName, object OutPutParamerterValue)
        {
            using (SqlConnection SQLConn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand SQLCmd = new SqlCommand())
                {
                    SQLCmd.Connection = SQLConn;
                    SQLCmd.CommandText = StroredProcedureName;
                    SQLCmd.CommandType = CommandType.StoredProcedure;
                    SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                    SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                    SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;

                    try
                    {
                        await SQLConn.OpenAsync();
                        await SQLCmd.ExecuteNonQueryAsync();
                        int ReturnValue = (int)SQLCmd.Parameters[OutPutParamerterName].Value;
                        SQLConn.Close();
                        return ReturnValue;
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
