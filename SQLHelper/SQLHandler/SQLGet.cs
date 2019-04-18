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
    public partial class SQLGet : SQLHandler
    {
        public SQLGet()
        {
            _connectionString = Connectionconfig;
        }

        #region "Public Method"
        /// <summary>
        /// Execute As Object
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public T ExecuteAsObject<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

                SqlDataReader SQLReader;
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters = new SQLParamCollection(ParaMeterCollection).ParamCollection;
                SQLCmd.Parameters.AddRange(sqlParameters);
                SQLConn.Open();
                SQLReader = SQLCmd.ExecuteReader(CommandBehavior.CloseConnection);
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

        /// <summary>
        /// Execute As Object
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public T ExecuteAsObject<T>(string StroredProcedureName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

                SqlDataReader SQLReader;
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;

                SQLConn.Open();
                SQLReader = SQLCmd.ExecuteReader();
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


        /// <summary>
        /// Execute As Scalar 
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public T ExecuteAsScalar<T>(string StroredProcedureName, List<SQLParam> ParaMeterCollection)
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
                return (T)SQLCmd.ExecuteScalar();
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
        /// Execute As Scalar
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">StoredProcedure Name</param>
        /// <returns></returns>
        public T ExecuteAsScalar<T>(string StroredProcedureName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLConn.Open();
                return (T)SQLCmd.ExecuteScalar();
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
