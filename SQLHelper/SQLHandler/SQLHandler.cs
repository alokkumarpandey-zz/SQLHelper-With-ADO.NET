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
    public partial class SQLHandler
    {
        public static string Connectionconfig;
        #region "Private Members"

        private string _objectQualifier = string.Empty;
        private string _databaseOwner = "[dbo]";
        internal string _connectionString = "";

        #endregion

        #region "Constructors"

        public SQLHandler()
        {
            //Note there is lot of way to set this, please use your needed way to do this connections string vallue setting
            _connectionString = Connectionconfig;
        }

        #endregion

        #region "Properties"

        public string objectQualifier
        {
            get { return _objectQualifier; }
            set { _objectQualifier = value; }
        }

        public string databaseOwner
        {
            get { return _databaseOwner; }
            set { _databaseOwner = value; }
        }

        public string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion


       

        #region "Public Methods"




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SQLCmd"></param>
        /// <param name="OutPutParamerterName"></param>
        /// <returns></returns>
        public SqlCommand AddOutPutParametrofGivenType<T>(SqlCommand SQLCmd, string OutPutParamerterName)
        {
            if (typeof(T) == typeof(int))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(Int16))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(long))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.BigInt));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(DateTime))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.DateTime));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(string))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.NVarChar, 255));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(bool))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(decimal))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Decimal));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Precision = 16;
                SQLCmd.Parameters[OutPutParamerterName].Scale = 2;
            }
            else if (typeof(T) == typeof(float))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Float));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(Guid))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.UniqueIdentifier));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            return SQLCmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SQLCmd"></param>
        /// <param name="OutPutParamerterName"></param>
        /// <param name="OutPutParamerterValue"></param>
        /// <returns></returns>
        public SqlCommand AddOutPutParametrofGivenType<T>(SqlCommand SQLCmd, string OutPutParamerterName, object OutPutParamerterValue)
        {
            if (typeof(T) == typeof(int))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(Int16))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(long))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.BigInt));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(DateTime))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.DateTime));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(string))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.NVarChar, 255));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(bool))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(decimal))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Decimal));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Precision = 16;
                SQLCmd.Parameters[OutPutParamerterName].Scale = 2;
            }
            else if (typeof(T) == typeof(float))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Float));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if(typeof(T) == typeof(Guid))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.UniqueIdentifier));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;
            return SQLCmd;
        }

        /// <summary>
        /// Execute SQL
        /// </summary>
        /// <param name="SQL">SQL query in string</param>
        /// <returns></returns>
        public DataTable ExecuteSQL(string SQL)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SQL = SQL.Replace("{databaseOwner}", this.databaseOwner);
                SQL = SQL.Replace("{objectQualifier}", this.objectQualifier);

                SqlCommand SQLCmd = new SqlCommand();
                SqlDataAdapter SQLAdapter = new SqlDataAdapter();
                DataSet SQLds = new DataSet();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = SQL;
                SQLCmd.CommandType = CommandType.Text;
                SQLAdapter.SelectCommand = SQLCmd;
                SQLConn.Open();
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

        /// <summary>
        /// Bulid Collection of List<SQLParam> for Given object
        /// </summary>
        /// <typeparam name="List">List of Type(string,object)</typeparam>
        /// <param name="paramCollection">List of Type(string,object)</param>
        /// <param name="obj">Object</param>
        /// <param name="excludeNullValue">Set True To Exclude Properties Having Null Value In The Object From Adding To The Collection</param>
        /// <returns> Collection of SQLParam</returns>
        public List<SQLParam> BuildParameterCollection(List<SQLParam> paramCollection, object obj, bool excludeNullValue)
        {
            try
            {
                if (excludeNullValue)
                {
                    foreach (PropertyInfo objProperty in obj.GetType().GetProperties())
                    {
                        if (objProperty.GetValue(obj, null) != null)
                        {
                            paramCollection.Add(new SQLParam("@" + objProperty.Name.ToString(), objProperty.GetValue(obj, null)));
                        }
                    }
                }
                else
                {
                    foreach (PropertyInfo objProperty in obj.GetType().GetProperties())
                    {
                        paramCollection.Add(new SQLParam("@" + objProperty.Name.ToString(), objProperty.GetValue(obj, null)));
                    }
                    return paramCollection;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return paramCollection;
        }



        #endregion
    }
}
