using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLSelect : ISQLSelect
    {
        public string Alias { get; set; }

        public Type Type { get; private set; }

        public SQL Sql { get; private set; }

        public IEntityMapper Mapper { get; private set; }
        public Expression Expression { get; private set; }
        public SQLSelect(SQL sql, Type type)
        {
            Sql = sql;
            Type = type;
            Mapper = EntityMapper.GetMapper(type);
        }

        public ISQLSelect Select(Expression selector)
        {
            Expression = selector;
            return this;
        }

        public ISQLSelect As(string alias)
        {
            Alias = alias;
            return this;
        }

        public ISQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            return new SQLWhere(Sql, where);
        }

        /// <summary>
        /// 根据别名组合选择关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ComposeSelectKey(string key)
        {
            if (string.IsNullOrEmpty(Alias))
                return key;
            return $"{Alias}.{key}";
        }

        public string ComposeTableName()
        {
            if (string.IsNullOrEmpty(Alias))
                return Mapper.TableName;
            return $"{Mapper.TableName} AS {Alias}";
        }
        public string Compile()
        {
            return Sql.Compile();
        }

        public ISQLJoin Join<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join, JoinType type)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, type);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }

        public ISQLJoin LeftJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, JoinType.LeftJoin);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }

        public ISQLJoin RightJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, JoinType.RightJoin);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }

        public ISQLJoin InnerJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, JoinType.InnerJoin);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }

        public ISQLJoin CrossJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, JoinType.CrossJoin);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }

        public ISQLJoin FullJoin<TJoin, TEntityOther>(Expression<Func<TJoin, TEntityOther, bool>> join)
        {
            SQLJoin sqlJoin = new SQLJoin(Sql, join, JoinType.FullJoin);
            sqlJoin.Type = typeof(TJoin);
            sqlJoin.Mapper = EntityMapper.GetMapper<TJoin>();
            Sql.Joins = new List<ISQLJoin>() { sqlJoin };
            return sqlJoin;
        }
    }
}
