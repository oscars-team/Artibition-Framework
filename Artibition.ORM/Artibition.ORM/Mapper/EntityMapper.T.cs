using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public class EntityMapper<TEntity> : IEntityMapper
    {
        private string _tableName;
        private List<string> _primaryKeys;
        private List<string> _identityKeys;
        private Dictionary<string, string> _columnNameMapper;
        public string[] Fields { get; private set; }
        public string[] Columns { get; private set; }
        public EntityMapper()
        {
            _primaryKeys = new List<string>();
            _identityKeys = new List<string>();
            _columnNameMapper = new Dictionary<string, string>();
        }
        public EntityMapper<TEntity> PrimaryKeys(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper<TEntity> IdentityKeys(Expression<Func<TEntity, object>> selector)
        {
            return this;
        }

        public EntityMapper<TEntity> Column(Expression<Func<TEntity, object>> column, string columnName)
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
                        _columnNameMapper.Update(name, columnName);
                        break;
                    case ExpressionType.Convert:
                        var operand = (columnLambda.Body as UnaryExpression).Operand as MemberExpression;
                        name = operand.Member.Name;
                        // 映射到列
                        _columnNameMapper.Update(name, columnName);
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
            if (!_columnNameMapper.TryGetValue(column, out columnName)) {
                return column;
            }
            return columnName;
        }

        public string GetColumnName(Expression<Func<TEntity, object>> column)
        {
            // 只有 p=>p.property 这种形式才为有效值
            if (column.NodeType == ExpressionType.Lambda && (column as LambdaExpression).Body.NodeType == ExpressionType.MemberAccess) {
                // 成员名称
                var name = ((column as LambdaExpression).Body as MemberExpression).Member.Name;
                return GetColumnName(name);
            }

            throw new Exception("EntityMapper.GetColumnName<TEntity>() Error: Unsupported LambdaExpression.");
        }

        private string[] GetColumns()
        {
            try {
                Type t = typeof(TEntity);
                return t.GetProperties().Select(p => p.Name).ToArray();
            }
            catch {
                return new string[0];
            }
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

        public void Register(Type type)
        {
            EntityMapper.Map(type, this);
        }

        public void Register()
        {
            Fields = GetColumns();
            Columns = Fields.Select(p => GetColumnName(p)).ToArray();
            // Submit this mapper
            Register(typeof(TEntity));
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
