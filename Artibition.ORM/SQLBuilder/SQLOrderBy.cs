using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLOrderBy : ISQLOrderBy
    {
        public Expression Expression { get; private set; }
        public OrderBy By { get; private set; }
        public SQL Sql { get; private set; }
        public SQLOrderBy(SQL sql, Expression order, OrderBy by)
        {
            Sql = sql;
            Expression = order;
            By = by;
        }
        public string Compile()
        {
            return Sql.Compile();
        }
        public ISQLOrderBy OrderByAscending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            Sql.Orders.Add(new SQLOrderBy(Sql, order, OrderBy.ASC));
            return this;
        }
        public ISQLOrderBy OrderByDescending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            Sql.Orders.Add(new SQLOrderBy(Sql, order, OrderBy.DESC));
            return this;
        }
    }
}
