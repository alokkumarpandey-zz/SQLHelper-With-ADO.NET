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
    public partial class SQLRaw  : SQLHandler
    {
        public SQLRaw()
        {
            _connectionString = Connectionconfig;
        }

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

        public int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, List<KeyValuePair<string, object>> ParaMeterCollection, string outParamName)
        {
            //create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText);

            for (int i = 0; i < ParaMeterCollection.Count; i++)
            {
                SqlParameter sqlParaMeter = new SqlParameter();
                sqlParaMeter.IsNullable = true;
                sqlParaMeter.ParameterName = ParaMeterCollection[i].Key;
                sqlParaMeter.Value = ParaMeterCollection[i].Value;
                cmd.Parameters.Add(sqlParaMeter);
            }
            cmd.Parameters.Add(new SqlParameter(outParamName, SqlDbType.Int));
            cmd.Parameters[outParamName].Direction = ParameterDirection.Output;

            //finally, execute the command.
            cmd.ExecuteNonQuery();
            int id = (int)cmd.Parameters[outParamName].Value;

            // detach the OracleParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return id;
        }

        public void ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            //create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText);

            for (int i = 0; i < ParaMeterCollection.Count; i++)
            {
                SqlParameter sqlParaMeter = new SqlParameter();
                sqlParaMeter.IsNullable = true;
                sqlParaMeter.ParameterName = ParaMeterCollection[i].Key;
                sqlParaMeter.Value = ParaMeterCollection[i].Value;
                cmd.Parameters.Add(sqlParaMeter);
            }

            //finally, execute the command.
            cmd.ExecuteNonQuery();

            // detach the OracleParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();

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
        public void ModulesRollBack(int ModuleID, int PortalID)
        {
            try
            {
                SqlConnection SQLConn = new SqlConnection(this._connectionString);
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = "dbo.sp_ModulesRollBack";
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLCmd.Parameters.Add(new SqlParameter("@ModuleID", ModuleID));
                SQLCmd.Parameters.Add(new SqlParameter("@PortalID", PortalID));
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute As DataReader
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <returns></returns>
        public SqlDataReader ExecuteAsDataReader(string StroredProcedureName)
        {
            try
            {
                SqlConnection SQLConn = new SqlConnection(this._connectionString);
                SqlDataReader SQLReader;
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                SQLConn.Open();
                SQLReader = SQLCmd.ExecuteReader(CommandBehavior.CloseConnection);

                return SQLReader;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Execute As DataReader
        /// </summary>
        /// <param name="StroredProcedureName">Store Procedure Name </param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public SqlDataReader ExecuteAsDataReader(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            try
            {
                SqlConnection SQLConn = new SqlConnection(this._connectionString);
                SqlDataReader SQLReader;
                SqlCommand SQLCmd = new SqlCommand();
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
                SQLConn.Open();
                SQLReader = SQLCmd.ExecuteReader(CommandBehavior.CloseConnection);
                return SQLReader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        #endregion
    }
}
