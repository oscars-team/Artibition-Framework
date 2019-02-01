using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public partial class SQL : ISQL
    {
        private SQLWhere _where;
        private Type _lastAliasType;
        //private List<SQLJOIN> _joins;
        public SQLAlias Alias { get; private set; }
        public IEntityMapper SelectorMapper { get; private set; }
        public Expression SelectorExpression { get; private set; }
        public Expression WhereExpression { get => _where?.GetExpression(); }
        public SQL()
        {
            EntityMapper.RegisterEntityMappers();
            Alias = new SQLAlias();
        }

        #region -- Select 语句处理 --
        public SQL Select<TEntity>(Expression<Func<TEntity, object>> selector = null)
        {
            SelectorMapper = EntityMapper.GetMapper<TEntity>();
            SelectorExpression = selector;
            _lastAliasType = typeof(TEntity);
            return this;
        }

        public SQL As(string alias)
        {
            if (!string.IsNullOrEmpty(alias))
                Alias.Add(alias, _lastAliasType);
            return this;
        }

        #endregion

        #region -- Where 语句处理 --
        public SQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            _where = new SQLWhere(this, where);
            return _where;
        }
        #endregion

        public string Compile()
        {
            var compiler = new SQLCompiler(this);
            return compiler.Compile();
        }

    }


}
