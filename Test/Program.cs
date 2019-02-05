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
            var sql5 = new SQL().Select<User>().As("p")
                                .InnerJoin<User, User>((q, p) => p.age == q.age && p.name == q.name)
                                .Where<User>(q => q.name == "leo")
                                .OrderByDescending<User>(p => p.name);
            Console.WriteLine(sql5.Compile());
            Console.ReadLine();
        }
    }
}
