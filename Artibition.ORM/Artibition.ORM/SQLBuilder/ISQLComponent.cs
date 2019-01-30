using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLComponent
    {
        SQL Sql { get; set; }
        string Compile();

    }
}
