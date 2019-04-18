#region "References"
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Reflection;
#endregion

namespace SQLHelper
{
    public partial class SQLAddUpdate : SQLHandler
    {
        public SQLAddUpdate()
        {
            _connectionString = Connectionconfig;
        }

        #region "Public Method"
        /// <summary>
        /// Returning Bool After Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="ParaMeterCollection"> Parameter Collection</param>
        /// <param name="OutPutParamerterName">Out Parameter Collection</param>
        /// <returns>Bool</returns>
        public bool ExecuteNonQueryAsBool(string StroredProcedureName, IList<SQLParam> ParaMeterCollection, string OutPutParamerterName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Returning Bool After Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="ParaMeterCollection">Parameter Collection</param>
        /// <param name="OutPutParamerterName">OutPut Parameter Name</param>
        /// <param name="OutPutParamerterValue">OutPut Parameter Value</param>
        /// <returns>Bool</returns>
        public bool ExecuteNonQueryAsBool(string StroredProcedureName, IList<SQLParam> ParaMeterCollection, string OutPutParamerterName, object OutPutParamerterValue)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                //End of for loop
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name In String</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters<SQLParam> </param>
        public void ExecuteNonQuery(string StroredProcedureName, IList<SQLParam> ParaMeterCollection)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();

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

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        public void ExecuteNonQuery(string StroredProcedureName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Accept only int, Int16, long, DateTime, string (NVarcha of size  50),
        /// bool, decimal ( of size 16,2), float
        /// </summary>
        /// <typeparam name="T">Return the given type of object</typeparam>
        /// <param name="StroredProcedureName">Accet SQL procedure name in string</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection for parameters</param>
        /// <param name="OutPutParamerterName">Accept Output parameter for the stored procedures</param>
        /// <returns></returns>
        public T ExecuteNonQueryAsGivenType<T>(string StroredProcedureName, IList<SQLParam> ParaMeterCollection, string OutPutParamerterName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLCmd = AddOutPutParametrofGivenType<T>(SQLCmd, OutPutParamerterName);
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Accept only int, Int16, long, DateTime, string (NVarcha of size  50),
        /// bool, decimal ( of size 16,2), float
        /// </summary>
        /// <typeparam name="T">Return the given type of object</typeparam>
        /// <param name="StroredProcedureName">Accet SQL procedure name in string</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection for parameters</param>
        /// <param name="OutPutParamerterName">Accept Output parameter for the stored procedures</param>
        /// <returns></returns>
        public T ExecuteNonQueryAsGivenType<T>(string StroredProcedureName, IList<SQLParam> ParaMeterCollection, string OutPutParamerterName, object OutPutParamerterValue)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets

                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                
                //End of for loop                
                SQLCmd = AddOutPutParametrofGivenType<T>(SQLCmd, OutPutParamerterName, OutPutParamerterValue);
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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


        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name In String</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <param name="OutPutParamerterName">Accept OutPut Key Value Collection For Parameters</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string StroredProcedureName, IList<SQLParam> ParaMeterCollection, string OutPutParamerterName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name</param>
        /// <param name="OutPutParamerterName">Accept Output For Parameters Name</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string StroredProcedureName, string OutPutParamerterName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Execute Non Query
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="OutPutParamerterName">Accept Output For Parameter Name</param>
        /// <param name="OutPutParamerterValue">Accept Output For Parameter Value</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string StroredProcedureName, string OutPutParamerterName, object OutPutParamerterValue)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
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

        #endregion
    }
}
