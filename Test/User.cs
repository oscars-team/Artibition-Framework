using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace Test
{
    public class User
    {
        public string name { get; set; }
        public int age { get; set; }
    }

    public class UserMapper : EntityMapper
    {
        public UserMapper()
        {
            this.Table("User")
                .PrimaryKeys<User>(p => p.name)
                .Register<User>();
        }
    }
}
