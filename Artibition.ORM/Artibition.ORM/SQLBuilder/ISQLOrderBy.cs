using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLOrderBy : ISQLComponent
    {

    }

    public enum OrderBy
    {
        ASC = 0,
        DESC = 1
    }
}
