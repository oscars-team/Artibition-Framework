using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLOrderBy : ISQLComponent
    {
        Expression Expression { get; }
        OrderBy By { get; }
        ISQLOrderBy OrderByAscending<TEntity>(Expression<Func<TEntity, object>> order);
        ISQLOrderBy OrderByDescending<TEntity>(Expression<Func<TEntity, object>> order);
    }

    public enum OrderBy
    {
        ASC = 0,
        DESC = 1
    }
}
