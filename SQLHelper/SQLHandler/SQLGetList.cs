#region "References"
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
#endregion

namespace SQLHelper
{
    public partial class SQLGetList : SQLHandler
    {
        public SQLGetList()
        {
            _connectionString = Connectionconfig;
        }

        #region Public Methods
        /// <summary>
        /// Execute As list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection"></param>
        /// <returns></returns>
        public List<T> ExecuteAsList<T>(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

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
                SQLReader = SQLCmd.ExecuteReader(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
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

        /// <summary>
        /// Execute As List
        /// </summary>
        /// <typeparam name="T"><T></typeparam>
        /// <param name="StroredProcedureName">Storedprocedure Name</param>
        /// <returns></returns>
        public List<T> ExecuteAsList<T>(string StroredProcedureName)
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
                SQLReader = SQLCmd.ExecuteReader(CommandBehavior.CloseConnection); //datareader automatically closes the SQL connection
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


        /// <summary>
        /// Execute As DataSet
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <returns></returns>
        public DataSet ExecuteAsDataSet(string StroredProcedureName)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {

                SqlCommand SQLCmd = new SqlCommand();
                SqlDataAdapter SQLAdapter = new SqlDataAdapter();
                DataSet SQLds = new DataSet();
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
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

        /// <summary>
        /// Execute As DataSet
        /// </summary>
        /// <param name="StroredProcedureName">StoreProcedure Name</param>
        /// <param name="ParaMeterCollection">Accept Key Value Collection For Parameters</param>
        /// <returns></returns>
        public DataSet ExecuteAsDataSet(string StroredProcedureName, List<KeyValuePair<string, object>> ParaMeterCollection)
        {
            SqlConnection SQLConn = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand SQLCmd = new SqlCommand();
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



        #endregion
    }
}
