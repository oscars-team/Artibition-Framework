using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Script
{
    public interface ISQLScript
    {
        string Insert { get; }
        string Update { get; }
        string Delete { get; }
        string Query { get; }
    }
}
