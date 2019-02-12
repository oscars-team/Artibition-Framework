using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class Network
    {
        public string id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string images { get; set; }

    }

    public class NetworkMapper : EntityMapper<Network>
    {
        public NetworkMapper()
        {
            this.Table("Networks")
                .PrimaryKeys(p => p.id)
                .Column(p => p.id, "Id")
                .Column(p => p.images, "Images")
                .Column(p => p.name, "Name")
                .Column(p => p.title, "Title")
                .Register();
        }
    }
}
