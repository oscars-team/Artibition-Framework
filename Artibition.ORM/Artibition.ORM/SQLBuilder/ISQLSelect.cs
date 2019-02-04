using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public interface ISQLSelect : ISQLComponent
    {
        string Alias { get; set; }
        Type Type { get; }
        IEntityMapper Mapper { get; }
        Expression Expression { get; }
        ISQLSelect Select(Expression selector);
        ISQLSelect As(string alias);
        ISQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where);
        ISQLJoin Join<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join, JoinType type);
        ISQLJoin LeftJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin RightJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin InnerJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin CrossJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        ISQLJoin FullJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join);
        string ComposeSelectKey(string keyName);
        string ComposeTableName();
    }
}
