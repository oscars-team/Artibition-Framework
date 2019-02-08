using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Artibition.ORM.Mapper
{
    public partial class EntityMapper : IEntityMapper
    {
        private List<string> _primaryKeys;
        private List<string> _identityKeys;

        public string[] Fields { get; private set; }
        public string[] Columns { get; private set; }
        public string TableName { get; private set; }
        public Dictionary<string, string> ColumnMapper { get; private set; }
        public EntityMapper()
        {
            _primaryKeys = new List<string>();
            _identityKeys = new List<string>();
            ColumnMapper = new Dictionary<string, string>();
        }
        public EntityMapper PrimaryKeys<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper IdentityKeys<TEntity>(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper Column<TEntity>(Expression<Func<TEntity, object>> column, string columnName)
        {
            // 只有 p=>p.property 这种形式才为有效值
            if (column.NodeType == ExpressionType.Lambda) {
                // 成员名称
                string name = string.Empty;
                LambdaExpression columnLambda = column as LambdaExpression;
                switch (columnLambda.Body.NodeType) {
                    case ExpressionType.MemberAccess:
                        name = (columnLambda.Body as MemberExpression).Member.Name;
                        // 映射到列
                        ColumnMapper.Update(name, columnName);
                        break;
                    case ExpressionType.Convert:
                        var operand = (columnLambda.Body as UnaryExpression).Operand as MemberExpression;
                        name = operand.Member.Name;
                        // 映射到列
                        ColumnMapper.Update(name, columnName);
                        break;
                }
            }
            return this;
        }
        public string GetColumnName(string column)
        {
            if (string.IsNullOrEmpty(column))
                throw new Exception("EntityMapper.GetColumn() Error: column name is empty");

            string columnName;
            if (!ColumnMapper.TryGetValue(column, out columnName)) {
                return column;
            }
            return columnName;
        }

        public string GetColumnName<TEntity>(Expression<Func<TEntity, object>> column)
        {
            // 只有 p=>p.property 这种形式才为有效值
            if (column.NodeType == ExpressionType.Lambda && (column as LambdaExpression).Body.NodeType == ExpressionType.MemberAccess) {
                // 成员名称
                var name = ((column as LambdaExpression).Body as MemberExpression).Member.Name;
                return GetColumnName(name);
            }

            throw new Exception("EntityMapper.GetColumnName<TEntity>() Error: Unsupported LambdaExpression.");
        }

        private string[] GetColumns<TEntity>()
        {
            try {
                Type t = typeof(TEntity);
                return t.GetProperties().Select(p => p.Name).ToArray();
            }
            catch {
                return new string[0];
            }
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
            TableName = tableName;
            return this;
        }
        public void Register(Type t)
        {
            Map(t, this);
        }
        public void Register<TEntity>()
        {
            Fields = GetColumns<TEntity>();
            Columns = Fields.Select(p => GetColumnName(p)).ToArray();
            // Submit this mapper
            Register(typeof(TEntity));
        }

        public string GetTableName()
        {
            return TableName;
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
