using Artibition.ORM.SQLBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sql1 = new SQL().Select<User>().As("u")
            //                    .Where<User>(u => u.name == "leo")
            //                    .And<User>(u => u.age == 12)
            //                    .Compile();
            //var sql2 = new SQL().Select<User>()
            //                    .Where<User>(p => p.age == 15)
            //                    .Compile();
            //var sql3 = new SQL().Select<User>(w => new { w.name })
            //                    .Where<User>(w => w.age == 15)
            //                    .Compile();
            //var sql4 = new SQL().Select<User>().As("p")
            //                    .Where<User>(p => p.age == 15)
            //                    .OrderByDescending<User>(p => p.name)
            //                    .OrderByAscending<User>(p => p.name)
            //                    .Compile();
            var sql5 = new SQL().Select<User>().As("p")
                                .InnerJoin<User, User>((q, p) => p.age == q.age && p.name == q.name)
                                .Where<User>(q => q.name == "leo")
                                .Compile();
            //Console.WriteLine(sql1);
            //Console.WriteLine(sql2);
            //Console.WriteLine(sql3);
            //Console.WriteLine(sql4);
            Console.WriteLine(sql5);
            Console.ReadLine();
        }
    }
}
