using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Artibition.ORM.SQLDialect
{
    public class SQLParameter : IDbDataParameter
    {
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable { get; set; }
        public string ParameterName { get; set; }
        public string SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object Value { get; set; }

        public SQLParameter(string name, object value, ParameterDirection direction)
        {
            ParameterName = name;
            Value = value;
            Direction = direction;
        }

        public SQLParameter(string name, object value) : this(name, value, ParameterDirection.Input)
        {

        }
    }
}
