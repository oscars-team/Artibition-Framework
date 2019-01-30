using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public class EntityMapper<TEntity> : IEntityMapper
    {
        private string _tableName;
        private List<string> _primaryKeys;
        private List<string> _identityKeys;
        public EntityMapper(string tableName)
        {
            _tableName = tableName;
        }
        public EntityMapper<TEntity> PrimaryKeys(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper<TEntity> IdentityKeys(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper<TEntity> LeftJoin(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }
        public EntityMapper<TEntity> InnerJoin(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }
        public EntityMapper<TEntity> RightJoin(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper<TEntity> Table(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public void Register<TEntity>()
        {
            EntityMapper.map(typeof(TEntity), this);
        }

        public string GetTableName()
        {
            return _tableName;
        }

        public string[] GetPrimaryKeys()
        {
            return _primaryKeys.ToArray();
        }

        public string[] GetIdentityKeys()
        {
            return _identityKeys.ToArray();
        }
    }
}
