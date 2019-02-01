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
        public Expression WhereExpression { get => _where?.GetExpression(); }
        public SQL()
        {
            EntityMapper.RegisterEntityMappers();
        }

        #region -- Select 语句处理 --
        public SQL Select<TEntity>(string alias = null, Expression<Func<TEntity, object>> selector = null)
        {
            return this;
        }

        public SQL Select<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        #endregion

        #region -- Where 语句处理 --
        public SQLWhere Where<TEntity>(string alias, Expression<Func<TEntity, bool>> where)
        {
            _where = new SQLWhere(this, where);
            return _where;
        }
        public SQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            _where = new SQLWhere(this, where);
            return _where;
        }
        #endregion

        public string Compile()
        {
            var compiler = new SQLCompiler();
            return compiler.Compile(this);
        }


    }


}
