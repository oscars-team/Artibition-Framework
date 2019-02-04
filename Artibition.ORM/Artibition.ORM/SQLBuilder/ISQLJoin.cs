using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLJoin : ISQLComponent
    {
        string Alias { get; set; }
        JoinType JoinType { get; }
        Type Type { get; }
        IEntityMapper Mapper { get; }
        LambdaExpression Expression { get; }
        ISQLJoin Join<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join, JoinType type);
        ISQLJoin LeftJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin RightJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin InnerJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin CrossJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin FullJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where);
        string ComposeTableName();
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
