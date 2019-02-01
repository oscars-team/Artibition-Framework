using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLWhere : ISQLWhere
    {
        public SQL Sql { get; set; }
        private LambdaExpression _where;
        public Expression GetExpression()
        {
            return _where.Body;
        }
        public SQLWhere(SQL sql, LambdaExpression where)
        {
            Sql = sql;
            _where = where;
        }

        public ISQLWhere And<TEntity>(string alias, Expression<Func<TEntity, bool>> where)
        {
            _where = Expression.Lambda(Expression.AndAlso(_where, where));
            return this;
        }

        public ISQLWhere And<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            _where = Expression.Lambda(Expression.AndAlso((_where as LambdaExpression).Body, (where as LambdaExpression).Body));
            return this;
        }

        public string Compile()
        {
            return Sql.Compile();
        }

        public ISQLWhere Or<TEntity>(string alias, Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ISQLWhere Or<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ISQLOrderBy OrderBy<TEntity>(Expression<Func<TEntity, object>> order, OrderBy by)
        {
            throw new NotImplementedException();
        }

        public ISQLOrderBy OrderByAscending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            throw new NotImplementedException();
        }

        public ISQLOrderBy OrderByDescending<TEntity>(Expression<Func<TEntity, object>> order)
        {
            throw new NotImplementedException();
        }

    }

    //public class ParameterVisitor : ExpressionVisitor
    //{
    //    private string _replaceName;
    //    public static Expression ReplaceParameter(Expression p, string replaceName)
    //    {
    //        return new ParameterVisitor(replaceName).Visit(p);
    //    }

    //    public ParameterVisitor(string replaceName)
    //    {
    //        _replaceName = replaceName;
    //    }

    //    protected override Expression VisitParameter(ParameterExpression p)
    //    {
    //        ParameterExpression replacement = Expression.Parameter(p.Type, _replaceName);
    //        return base.VisitParameter(p);
    //    }
    //}


    //public static class ExpressionExtention
    //{
    //    public static void ReplaceParametermeterName(this LambdaExpression expression, string alias)
    //    {
    //        var equal = expression.Body as BinaryExpression;
    //        var member = equal.Left as MemberExpression;
    //        var param = member.Expression as ParameterExpression;
    //        ParameterVisitor.ReplaceParameter(param, alias);
    //    }
    //}
}
