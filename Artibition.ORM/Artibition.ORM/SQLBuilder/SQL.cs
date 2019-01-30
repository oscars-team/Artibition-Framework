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
        private SQLAlias _alias;
        private TableCollection _tables;
        private SQLWhere _where;
        private Expression _selectorExpression;
        private List<string> _selectors;
        public SQLAlias Alias { get => _alias; private set => _alias = value; }
        public string[] Selectors => _selectors.ToArray();

        public SQL()
        {
            EntityMapper.RegisterEntityMappers();
            _alias = new SQLAlias();
            _selectors = new List<string>();
            _tables = new TableCollection();
        }

        #region -- Select 语句处理 --
        public SQL Select<TEntity>(string alias = null, Expression<Func<TEntity, object>> selector = null)
        {
            // 添加别名
            _alias.Add(alias, typeof(TEntity));
            // 将选择字段以别名的形式重新选择
            _selectorExpression = selector;
            //_selectors = processSelector(selector).Select(p => $"{_alias.LastCreatedAlias}.{p}").ToList();
            return this;
        }

        public SQL Select<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            // 添加别名
            _alias.Add(null, typeof(TEntity));
            // 将选择字段以别名的形式重新选择
            _selectorExpression = selector;
            //_selectors = processSelector(selector).Select(p => $"{_alias.LastCreatedAlias}.{p}").ToList();
            return this;
        }

        #endregion

        #region -- Where 语句处理 --
        public SQLWhere Where<TEntity>(string alias, Expression<Func<TEntity, bool>> where)
        {
            _alias.Add(alias, typeof(TEntity));
            _where = new SQLWhere(this, where);
            return _where;
        }
        public SQLWhere Where<TEntity>(Expression<Func<TEntity, bool>> where)
        {
            _alias.Add(null, typeof(TEntity));
            _where = new SQLWhere(this, where);
            return _where;
        }
        #endregion

        public string Compile()
        {
            StringBuilder SQLSelect = new StringBuilder("SELECT ");
            foreach (var s in Selectors)
                SQLSelect.Append($"{s},");
            SQLSelect.TrimComma().Append(" ");
            var map = EntityMapper.getMapper(_alias.LastCreatedType);
            SQLSelect.Append($" FROM {map.GetTableName()} AS {_alias.LastCreatedAlias} ");
            SQLSelect.AppendLine();
            return SQLSelect.ToString();
        }


    }


}
