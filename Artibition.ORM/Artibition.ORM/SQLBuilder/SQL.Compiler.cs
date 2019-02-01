using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public partial class SQLCompiler : ExpressionVisitor, ISQLCompile
    {
        public SQL Sql { get; }
        private StringBuilder _selectorCompilerStr;
        private StringBuilder _whereCompilerStr;
        public SQLCompiler(SQL sql)
        {
            Sql = sql;
        }
        private string compileSelector()
        {
            Expression selector = Sql.SelectorExpression;
            IEntityMapper selectorMapper = Sql.SelectorMapper;
            _selectorCompilerStr = new StringBuilder();
            if (selector == null) {
                _selectorCompilerStr?.Append("SELECT ");
                _selectorCompilerStr?.Append(
                    selectorMapper.Columns.Length == 0
                    ? "*"
                    : string.Join(", ", selectorMapper.Columns)
                );
            }
            else {
                _selectorCompilerStr?.Append("SELECT ");
                this.Visit(selector);
                _selectorCompilerStr?.TrimComma();
            }
            _selectorCompilerStr?.Append(" FROM ");
            _selectorCompilerStr?.Append(selectorMapper?.GetTableName());
            return _selectorCompilerStr?.ToString();
        }
        private string compileWhere()
        {
            Expression where = Sql.WhereExpression;

            _whereCompilerStr = new StringBuilder();
            if (where != null) {
                _whereCompilerStr?.Append(" WHERE ");
                this.Visit(where);
            }
            return _whereCompilerStr?.ToString();
        }

        public string Compile()
        {
            string select = compileSelector();
            string where = compileWhere();
            return $"{select} {where}";
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _whereCompilerStr.Append("(");
            this.Visit(node.Left);

            switch (node.NodeType) {
                case ExpressionType.Add:
                    _whereCompilerStr?.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    _whereCompilerStr?.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    _whereCompilerStr?.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    _whereCompilerStr?.Append(" / ");
                    break;
                case ExpressionType.Equal:
                    _whereCompilerStr?.Append(" = ");
                    break;

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _whereCompilerStr?.Append(" AND ");
                    break;

                case ExpressionType.Not:
                    _whereCompilerStr?.Append(" NOT ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _whereCompilerStr?.Append(" OR ");
                    break;

                case ExpressionType.Modulo:
                    _whereCompilerStr?.Append(" % ");
                    break;
            }

            this.Visit(node.Right);
            _whereCompilerStr?.Append(")");
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            object value = null;
            Expression expression = searchMemberExpressionTree(node, node.Expression, out value);
            if (expression.NodeType == ExpressionType.Parameter) {
                var param = node.Expression as ParameterExpression;
                var mapper = EntityMapper.GetMapper(expression.Type);
                string alias = param.Name;
                if (Sql.Alias.Count == 0) {
                    // 如果别名中没有数据，此时做隐式转变，去掉所有别名的显示
                    _whereCompilerStr?.Append($"{mapper.GetColumnName(node.Member.Name)}");
                }
                else {
                    if (Sql.Alias.ContainsKey(alias))
                        // 如果此时别名中有数据，而且调用了别名，视为刻意使用别名
                        // 此时需要对别名做验证，如果验证失败，抛出异常
                        // 
                        _whereCompilerStr?.Append($"{alias}.{mapper.GetColumnName(node.Member.Name)}");
                    else
                        throw new Exception($"SQLCompiler.VisitMember Error: 你正在有意使用别名(As)，但在别名集合中没找到\"{alias}\"。尝试检查在Expression中使用的参数名是否在方法As()中定义过。'");
                }
                _selectorCompilerStr?.Append($"{param.Name}.{node.Member.Name},");
            }
            else if (expression.NodeType == ExpressionType.Constant) {
                _whereCompilerStr?.Append(value.ToString());
            }
            return node;
        }
        /// <summary>
        /// 搜索成员的表达式树, 计算表达式最终结果表达式以及结果值
        /// </summary>
        /// <param name="rootExpression"></param>
        /// <param name="nodeExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Expression searchMemberExpressionTree(Expression rootExpression, Expression nodeExpression, out object value)
        {
            FieldInfo field;
            PropertyInfo property;
            value = null;
            if (rootExpression.NodeType == ExpressionType.MemberAccess) {
                var root = rootExpression as MemberExpression;
                if (nodeExpression.NodeType == ExpressionType.Constant) {
                    var node = nodeExpression as ConstantExpression;
                    field = node.Type.GetField(root.Member.Name);
                    value = field?.GetValue(node.Value);
                }

                if (nodeExpression.NodeType == ExpressionType.MemberAccess) {
                    var node = nodeExpression as MemberExpression;
                    searchMemberExpressionTree(node, node.Expression, out value);
                    property = node.Type.GetProperty(root.Member.Name);
                    value = property?.GetValue(value);
                }

                if (nodeExpression.NodeType == ExpressionType.ArrayIndex) {
                    var node = nodeExpression as BinaryExpression;
                    int index;
                    object[] array;
                    if (node.Right.NodeType == ExpressionType.Constant) {
                        searchMemberExpressionTree(node.Right, node.Right, out value);
                        index = (int)value;
                    }
                    else {
                        searchMemberExpressionTree(node.Right, (node.Right as MemberExpression).Expression, out value);
                        index = (int)value;
                    }

                    searchMemberExpressionTree(node.Left, (node.Left as MemberExpression).Expression, out value);
                    array = value as object[];
                    property = node.Type.GetProperty(root.Member.Name);
                    value = property.GetValue(array[index]);
                }

            }

            if (rootExpression.NodeType == ExpressionType.Constant)
                value = (rootExpression as ConstantExpression).Value;

            return nodeExpression;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            // 表达式类似于 p=>p.proprty == 1, p=>p.property == "123"
            _whereCompilerStr?.Append(node.Value);
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _whereCompilerStr?.Append(node.Name);
            return node;
        }

    }
}
