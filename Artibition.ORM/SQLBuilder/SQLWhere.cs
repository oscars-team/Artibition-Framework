using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLWhere : ISQLWhere
    {
        public SQL Sql { get; set; }
        public LambdaExpression Lambda { get; private set; }
        public Expression GetExpression()
        {
            return Lambda.Body;
        }
        public SQLWhere(SQL sql, LambdaExpression where)
        {
            Sql = sql;
            Lambda = where;
            Sql.Where = this;
        }
        public ISQLWhere And<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            Lambda = Expression.Lambda(Expression.AndAlso((Lambda as LambdaExpression).Body, (where as LambdaExpression).Body));
            return this;
        }
        public ISQLWhere Or<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            Lambda = Expression.Lambda(Expression.OrElse((Lambda as LambdaExpression).Body, (where as LambdaExpression).Body));
            return this;
        }

        public ISQLOrderBy OrderBy<TEntity>(Expression<Func<TEntity, object>> order, OrderBy by)
        {
            if (Sql.Orders == null)
                Sql.Orders = new List<ISQLOrderBy>();
            SQLOrderBy sqlOrder = new SQLOrderBy(Sql, order, by);
            Sql.Orders.Add(sqlOrder);
            return sqlOrder;
        }

        public ISQLOrderBy OrderByAscending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            if (Sql.Orders == null)
                Sql.Orders = new List<ISQLOrderBy>();
            SQLOrderBy sqlOrder = new SQLOrderBy(Sql, order, SQLBuilder.OrderBy.ASC);
            Sql.Orders.Add(sqlOrder);
            return sqlOrder;
        }
        public ISQLOrderBy OrderByDescending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            if (Sql.Orders == null)
                Sql.Orders = new List<ISQLOrderBy>();
            SQLOrderBy sqlOrder = new SQLOrderBy(Sql, order, SQLBuilder.OrderBy.DESC);
            Sql.Orders.Add(sqlOrder);
            return sqlOrder;
        }
        public string Compile()
        {
            return Sql.Compile();
        }
    }
}
