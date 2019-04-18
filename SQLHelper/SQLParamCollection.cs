using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLHelper
{
    public class SQLParamCollection
    {
        public SqlParameter[] _sqlParameters;
        public SqlParameter[] ParamCollection
        {
            get
            {
                return this._sqlParameters;
            }
            set
            {
                this._sqlParameters = value;
            }
        }

        public SQLParamCollection() { }
        public SQLParamCollection(IList<SQLParam> paraMeterCollection)
        {
            List<SqlParameter> _collection = new List<SqlParameter>();

            for (int i = 0; i < paraMeterCollection.Count; i++)
            {
                SQLParam _param = paraMeterCollection[i];
                SqlParameter sqlParaMeter = new SqlParameter()
                {
                    ParameterName = _param.Key,
                    Value = _param.Value,
                    IsNullable = true
                };
                _collection.Add(sqlParaMeter);
            }

            ParamCollection = _collection.ToArray();
        }

    }
}
