using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public class SQLAlias : Dictionary<string, Type>
    {
        static readonly string alias_prefix = "tb_";
        string lastAlias = string.Empty;
        Type lastType = null;
        public string LastCreatedAlias => lastAlias;
        public Type LastCreatedType => lastType;
        public SQLAlias()
        {

        }
        public new void Add(string alias, Type t)
        {
            if (string.IsNullOrEmpty(alias))
                alias = alias_prefix + (this.Count + 1).ToString();

            Type exist;
            if (this.TryGetValue(alias, out exist))
                throw new Exception($"别名\"{alias}\"重复, 请重新指定");

            base.Add(alias, t);
            lastAlias = alias;
            lastType = t;
        }
    }
}
