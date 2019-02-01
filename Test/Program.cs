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
            var sql = new SQL().Select<User>().Where<User>(u => u.name == "leo").And<User>(u => u.age == 12).Compile();
            Console.WriteLine(sql);
            Console.ReadLine();
        }
    }
}
