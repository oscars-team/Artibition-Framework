using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Window
    {
        public string id { get; set; }
        public string name { get; set; }
        public string network { get; set; }
    }

    public class WindowMapper : EntityMapper<Window>
    {
        public WindowMapper()
        {
            this.Table("Windows")
                .PrimaryKeys(p => p.id)
                .Column(p => p.id, "Id")
                .Column(p => p.name, "Name")
                .Column(p => p.network, "NetworkId")
                .Register();
        }
    }
}
