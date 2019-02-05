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
        public Dictionary<string, Type> AliasCollection { get; private set; }
        public ISQLSelect Selector { get; private set; }
        public List<ISQLJoin> Joins { get; set; }
        public ISQLWhere Where { get; set; }
        public List<ISQLOrderBy> Orders { get; set; }
        public SQL()
        {
            EntityMapper.RegisterEntityMappers();
            AliasCollection = new Dictionary<string, Type>();
        }

        #region -- Select 语句处理 --
        public ISQLSelect Select<TEntity>(Expression<Func<TEntity, object>> selector = null)
        {
            Selector = new SQLSelect(this, typeof(TEntity));
            return Selector.Select(selector);
        }
        #endregion

        public string Compile()
        {
            var compiler = new SQLCompiler(this);
            return compiler.Compile();
        }
    }


}
