using System;
using System.Collections.Generic;
using System.Text;

namespace SQLHelper
{
    public class SQLParam
    {
        private KeyValuePair<string, object> _sqlPara;
        /// <summary>
        /// Initalize keyvalue object with empity key, and null value
        /// </summary>
        public SQLParam()
        {
            _sqlPara = new KeyValuePair<string, object>(string.Empty, null);
        }
        public SQLParam(string paraKey, object objValue)
        {
            _sqlPara = new KeyValuePair<string, object>(paraKey, objValue);
        }
        public string Key
        {
            get { return _sqlPara.Key; }
        }
        public object Value
        {
            get { return _sqlPara.Value; }
        }
        public KeyValuePair<string, object> Add(string paraKey, object objValue)
        {
            Parameter = new KeyValuePair<string, object>(paraKey, objValue);
            return Parameter;
        }
        public KeyValuePair<string, object> Parameter
        {
            get { return _sqlPara; }
            set { _sqlPara = value; }
        }
    }
}
