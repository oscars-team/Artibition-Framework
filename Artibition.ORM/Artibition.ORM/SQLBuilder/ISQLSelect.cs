using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLSelect : ISQLComponent
    {
        string Alias { get; }
        Type Type { get; }
    }
}
