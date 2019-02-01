using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public partial class EntityMapper : IEntityMapper
    {

        private string _tableName;
        private List<string> _primaryKeys;
        private List<string> _identityKeys;
        public EntityMapper()
        {
            _primaryKeys = new List<string>();
            _identityKeys = new List<string>();
        }
        public EntityMapper PrimaryKeys<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper IdentityKeys<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper LeftJoin<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }
        public EntityMapper InnerJoin<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }
        public EntityMapper RightJoin<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper Table(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public void Register<TEntity>()
        {
            Map(typeof(TEntity), this);
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
