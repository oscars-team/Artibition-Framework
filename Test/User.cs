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
        public bool isMale { get; set; }
    }

    public class UserMapper : EntityMapper<User>
    {
        public UserMapper()
        {
            this.Table("User")
                .PrimaryKeys(p => p.name)
                .Column(p => p.name, "ORM_Name")
                .Column(p => p.age, "ORM_Age")
                .Column(p => p.isMale, "ORM_IsMale")
                .Register();
        }
    }
}
