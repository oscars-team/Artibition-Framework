using Artibition.ORM.SQLBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.Repository
{
    public class ArtiRepository : IRepository
    {
        public IDbConnection Connection { get; }

        public TEntity GetSingle<TEntity>(params object[] keyValues)
        {
            Expression<Func<TEntity, bool>> selector = p => 1 == 1;
            //var lambda = Expression.Lambda<TEntity>()
            //SQL sql = new SQL().Select<TEntity>().Where(
        }
    }
}
