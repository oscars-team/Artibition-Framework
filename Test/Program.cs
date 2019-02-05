using Artibition.ORM;
using Artibition.ORM.SQLBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqlConnection("server=.;database=fxgapjq;trusted_connection=true")) {
                var windows = DapperExtention.Query<Window>(conn, new SQL().Select<Window>().As("w").Sql);
            }
        }
    }
}
