using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLWhere : ISQLComponent
    {
        ISQLWhere And<TEntity>(string alias, Expression<Func<TEntity, bool>> where);
        ISQLWhere And<TEntity>(Expression<Func<TEntity, bool>> where);
        ISQLWhere Or<TEntity>(string alias, Expression<Func<TEntity, bool>> where);
        ISQLWhere Or<TEntity>(Expression<Func<TEntity, bool>> where);
        ISQLOrderBy OrderBy<TEntity>(Expression<Func<TEntity, object>> order, OrderBy by);
        ISQLOrderBy OrderByAscending<TEntity>(Expression<Func<TEntity, object>> order);
        ISQLOrderBy OrderByDescending<TEntity>(Expression<Func<TEntity, object>> order);
    }
}
