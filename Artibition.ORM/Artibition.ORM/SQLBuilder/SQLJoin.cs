using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLJoin : ISQLJoin
    {
        public string Alias { get; set; }

        public JoinType JoinType { get; private set; }
        public Type Type { get; set; }
        public IEntityMapper Mapper { get; set; }
        public LambdaExpression Expression { get; private set; }

        public SQL Sql { get; private set; }
        public SQLJoin(SQL sql, LambdaExpression joinExpression, JoinType type)
        {
            Sql = sql;
            Expression = joinExpression;
            JoinType = type;
        }
        public string Compile()
        {
            return Sql.Compile();
        }

        public ISQLJoin Join<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join, JoinType type)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, type));
            return this;
        }

        public ISQLJoin LeftJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, JoinType.LeftJoin));
            return this;
        }

        public ISQLJoin RightJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, JoinType.RightJoin));
            return this;
        }

        public ISQLJoin InnerJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, JoinType.InnerJoin));
            return this;
        }

        public ISQLJoin CrossJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, JoinType.CrossJoin));
            return this;
        }

        public ISQLJoin FullJoin<TEntity, TEntityOther>(Expression<Func<TEntity, TEntityOther, bool>> join)
        {
            Sql.Joins.Add(new SQLJoin(Sql, join, JoinType.FullJoin));
            return this;
        }

        public string ComposeTableName()
        {
            if (string.IsNullOrEmpty(Alias))
                return Mapper.TableName;
            return $"{Mapper.TableName} AS {Alias}";
        }

        public ISQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            ISQLWhere sqlWhere = new SQLWhere(Sql, where);
            Sql.Where = sqlWhere;
            return sqlWhere;
        }
    }
}
