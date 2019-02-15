using Artibition.ORM;
using Artibition.ORM.Mapper;
using Artibition.ORM.SQLBuilder;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Artibition.Repository
{
    public abstract class ArtiRepository : IRepository
    {
        public IDbConnection Connection { get; }
        public ArtiRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        public ArtiRepository(string connString) : this(new SqlConnection(connString))
        {

        }

        public TEntity GetSingle<TEntity>(object keyValue)
        {
            var sql = new SQL();
            var p = Expression.Parameter(typeof(TEntity), "p");
            var mi = typeof(TEntity).GetMembers().Where(r => r.Name == EntityMapper.GetMapper<TEntity>().GetPrimaryKeys()[0]).FirstOrDefault();
            var left = Expression.MakeMemberAccess(p, mi);
            var right = Expression.Constant(keyValue);
            var equal = Expression.Equal(left, right);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, p);
            sql = sql.Select<TEntity>().Where<TEntity>(lambda as Expression<Func<TEntity, bool>>).Sql;
            return Connection.QuerySingle<TEntity>(sql);
        }
    }
}
