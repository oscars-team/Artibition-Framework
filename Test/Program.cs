using Artibition.ORM;
using Artibition.ORM.SQLBuilder;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string id = "30f7fe51-dd00-4bdf-9a51-8b7411b3a4aa";
            using (var conn = new SqlConnection("server=.;database=fxgapjq;trusted_connection=true")) {
                //var windows = DapperExtention.Query<Network>(conn, new SQL().Select<Network>()
                //                                                           .Sql);
                var sql = "select * from [Windows] w inner join Networks n on w.NetworkId = n.Id";
                var data = conn.Query<Window, Network, Window>(sql, (w, n) => { w.network = n; return w; }, splitOn: "Id");
                var window = data.First();
            }
        }
    }
}


