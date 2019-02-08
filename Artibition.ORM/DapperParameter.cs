using Artibition.ORM.SQLDialect;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Artibition.ORM
{
    public class DapperParameter : SqlMapper.IDynamicParameters
    {
        public List<SQLParameter> SqlParameters { get; private set; }
        public DapperParameter(List<SQLParameter> parameters)
        {
            SqlParameters = parameters;
        }
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            this.SqlParameters?.ForEach(sp => {
                var p = command.CreateParameter();
                p.ParameterName = sp.ParameterName;
                p.Value = sp.Value;
                command.Parameters.Add(p);
            });
        }
    }
}
