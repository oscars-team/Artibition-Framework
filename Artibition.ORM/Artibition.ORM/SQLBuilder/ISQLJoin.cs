using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLJoin : ISQLComponent
    {
        string Alias { get; }

        JoinType JoinType { get; }
    }

    public enum JoinType
    {
        InnerJoin = 1,
        LeftJoin = 2,
        RightJoin = 3,
        FullJoin = 4,
        CrossJoin = 5
    }
}
