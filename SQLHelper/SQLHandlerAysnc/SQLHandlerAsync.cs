using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace SQLHelper
{
    public partial class SQLHandlerAsync
    {
        public static string Connectionconfig;
        #region "Private Members"

        internal string _objectQualifier = string.Empty;
        internal string _databaseOwner = "[dbo]";
        internal string _connectionString = "";

        #endregion

        #region "Constructors"
        public SQLHandlerAsync()
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

        #region "Transaction Methods"

        public void CommitTransaction(DbTransaction transaction)
        {
            try
            {
                transaction.Commit();
            }
            finally
            {
                if (transaction != null && transaction.Connection != null)
                {
                    transaction.Connection.Close();
                }
            }
        }

        public DbTransaction GetTransaction()
        {
            SqlConnection Conn = new SqlConnection(this.connectionString);
            Conn.Open();
            SqlTransaction transaction = Conn.BeginTransaction();
            return transaction;
        }

        public void RollbackTransaction(DbTransaction transaction)
        {
            try
            {
                transaction.Rollback();
            }
            finally
            {
                if (transaction != null && transaction.Connection != null)
                {
                    transaction.Connection.Close();
                }
            }
        }

        #region Using Transaction Method
        public static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            //associate the connection with the command
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandType = commandType;
            command.CommandText = commandText;
            return;
        }



        #endregion

        #endregion

        #region "SQL Execute Methods"

        private void ExecuteADOScript(SqlTransaction trans, string SQL)
        {
            SqlConnection connection = trans.Connection;
            //Create a new command (with no timeout)
            SqlCommand command = new SqlCommand(SQL, trans.Connection);
            command.Transaction = trans;
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
        }

        private void ExecuteADOScript(string SQL)
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            //Create a new command (with no timeout)
            SqlCommand command = new SqlCommand(SQL, connection);
            connection.Open();
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void ExecuteADOScript(string SQL, string ConnectionString)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            //Create a new command (with no timeout)
            SqlCommand command = new SqlCommand(SQL, connection);
            connection.Open();
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public string ExecuteScript(string Script)
        {
            return ExecuteScript(Script, false);
        }

        public DataSet ExecuteScriptAsDataSet(string SQL)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

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

        public string ExecuteScript(string Script, DbTransaction transaction)
        {
            string SQL = string.Empty;
            string Exceptions = string.Empty;
            string Delimiter = "GO" + Environment.NewLine;
            string[] arrSQL = Script.Split(new[] { Delimiter }, StringSplitOptions.None);
            bool IgnoreErrors;
            foreach (string SQLforeach in arrSQL)
            {
                if (!string.IsNullOrEmpty(SQLforeach))
                {
                    //script dynamic substitution
                    SQL = SQLforeach;
                    SQL = SQL.Replace("{databaseOwner}", this.databaseOwner);
                    SQL = SQL.Replace("{objectQualifier}", this.objectQualifier);

                    IgnoreErrors = false;

                    if (SQL.Trim().StartsWith("{IgnoreError}"))
                    {
                        IgnoreErrors = true;
                        SQL = SQL.Replace("{IgnoreError}", "");
                    }
                    try
                    {
                        ExecuteADOScript(transaction as SqlTransaction, SQL);
                    }
                    catch (Exception ex)
                    {
                        if (!IgnoreErrors)
                        {
                            Exceptions += ex.ToString() + Environment.NewLine + Environment.NewLine + SQL + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
            }
            return Exceptions;
        }

        public string ExecuteScript(string Script, bool UseTransactions)
        {
            string SQL = string.Empty;
            string Exceptions = string.Empty;

            if (UseTransactions)
            {
                DbTransaction transaction = GetTransaction();
                try
                {
                    Exceptions += ExecuteScript(Script, transaction);

                    if (Exceptions.Length == 0)
                    {
                        //No exceptions so go ahead and commit
                        CommitTransaction(transaction);
                    }
                    else
                    {
                        //Found exceptions, so rollback db
                        RollbackTransaction(transaction);
                        Exceptions += "SQL Execution failed.  Database was rolled back" + Environment.NewLine + Environment.NewLine + SQL + Environment.NewLine + Environment.NewLine;
                    }
                }
                finally
                {
                    if (transaction.Connection != null)
                    {

                        transaction.Connection.Close();
                    }
                }
            }
            else
            {
                string Delimiter = "GO" + Environment.NewLine;
                string[] arrSQL = Script.Split(new[] { Delimiter }, StringSplitOptions.None);
                foreach (string SQLforeach in arrSQL)
                {
                    if (!string.IsNullOrEmpty(SQLforeach))
                    {
                        SQL = SQLforeach;
                        SQL = SQL.Replace("{databaseOwner}", this.databaseOwner);
                        SQL = SQL.Replace("{objectQualifier}", this.objectQualifier);
                        try
                        {
                            ExecuteADOScript(SQL);
                        }
                        catch (Exception ex)
                        {
                            Exceptions += ex.ToString() + Environment.NewLine + Environment.NewLine + SQL + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
            }

            return Exceptions;
        }

        public string ExecuteInstallScript(string Script, string ConnectionString)
        {
            string SQL = string.Empty;
            string Exceptions = string.Empty;

            string Delimiter = "GO" + Environment.NewLine;
            string[] arrSQL = Script.Split(new[] { Delimiter }, StringSplitOptions.None);
            foreach (string SQLforeach in arrSQL)
            {
                if (!string.IsNullOrEmpty(SQLforeach))
                {
                    SQL = SQLforeach;
                    SQL = SQL.Replace("{databaseOwner}", this.databaseOwner);
                    SQL = SQL.Replace("{objectQualifier}", this.objectQualifier);
                    try
                    {
                        ExecuteADOScript(SQL, ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        Exceptions += ex.ToString() + Environment.NewLine + Environment.NewLine + SQL + Environment.NewLine + Environment.NewLine;
                    }
                }
            }
            return Exceptions;
        }


        #endregion

        #region "Public Methods"

        /// <summary>
        /// RollBack Module Installation If Error Occur During Module Installation
        /// </summary>
        /// <param name="ModuleID">ModuleID</param>
        /// <param name="PortalID">PortalID</param>

        

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
            if (typeof(T) == typeof(Int16))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(long))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.BigInt));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(DateTime))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.DateTime));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(string))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.NVarChar, 50));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(bool))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(decimal))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Decimal));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Precision = 16;
                SQLCmd.Parameters[OutPutParamerterName].Scale = 2;
            }
            if (typeof(T) == typeof(float))
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
            if (typeof(T) == typeof(Int16))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Int));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(long))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.BigInt));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(DateTime))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.DateTime));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(string))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.NVarChar, 50));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(bool))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Bit));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            if (typeof(T) == typeof(decimal))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Decimal));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
                SQLCmd.Parameters[OutPutParamerterName].Precision = 16;
                SQLCmd.Parameters[OutPutParamerterName].Scale = 2;
            }
            if (typeof(T) == typeof(float))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.Float));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            else if (typeof(T) == typeof(Guid))
            {
                SQLCmd.Parameters.Add(new SqlParameter(OutPutParamerterName, SqlDbType.UniqueIdentifier));
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.Output;
            }
            SQLCmd.Parameters[OutPutParamerterName].Value = OutPutParamerterValue;
            return SQLCmd;
        }




        /// <summary>
        /// Bulid Collection of List<KeyValuePair<string, object>> for Given object
        /// </summary>
        /// <typeparam name="List">List of Type(string,object)</typeparam>
        /// <param name="paramCollection">List of Type(string,object)</param>
        /// <param name="obj">Object</param>
        /// <param name="excludeNullValue">Set True To Exclude Properties Having Null Value In The Object From Adding To The Collection</param>
        /// <returns> Collection of KeyValuePair<string, object></returns>
        public List<KeyValuePair<string, object>> BuildParameterCollection(List<KeyValuePair<string, object>> paramCollection, object obj, bool excludeNullValue)
        {
            try
            {
                if (excludeNullValue)
                {
                    foreach (PropertyInfo objProperty in obj.GetType().GetProperties())
                    {
                        if (objProperty.GetValue(obj, null) != null)
                        {
                            paramCollection.Add(new KeyValuePair<string, object>("@" + objProperty.Name.ToString(), objProperty.GetValue(obj, null)));
                        }
                    }
                }
                else
                {
                    foreach (PropertyInfo objProperty in obj.GetType().GetProperties())
                    {
                        paramCollection.Add(new KeyValuePair<string, object>("@" + objProperty.Name.ToString(), objProperty.GetValue(obj, null)));
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
