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

        private StringBuilder selectCompilerStr;
        private StringBuilder whereCompilerStr;

        public SQLCompiler()
        {
        }
        private string compileSelect()
        {
            throw new NotImplementedException();
        }
        private string compileWhere(Expression where)
        {
            whereCompilerStr = new StringBuilder();
            if (where != null) {
                whereCompilerStr.Append(" WHERE ");
                this.Visit(where);
            }
            return whereCompilerStr.ToString();
        }

        public string Compile(SQL sql)
        {
            return compileWhere(sql.WhereExpression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            whereCompilerStr.Append("(");
            this.Visit(node.Left);

            switch (node.NodeType) {
                case ExpressionType.Add:
                    whereCompilerStr.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    whereCompilerStr.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    whereCompilerStr.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    whereCompilerStr.Append(" / ");
                    break;
                case ExpressionType.Equal:
                    whereCompilerStr.Append(" = ");
                    break;

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    whereCompilerStr.Append(" AND ");
                    break;

                case ExpressionType.Not:
                    whereCompilerStr.Append(" NOT ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    whereCompilerStr.Append(" OR ");
                    break;

                case ExpressionType.Modulo:
                    whereCompilerStr.Append(" % ");
                    break;
            }

            this.Visit(node.Right);
            whereCompilerStr.Append(")");
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            object value = null;
            Expression expression = searchMemberExpressionTree(node, node.Expression, out value);
            if (expression.NodeType == ExpressionType.Parameter) {
                var param = node.Expression as ParameterExpression;
                whereCompilerStr.Append($"{param.Name}.{node.Member.Name}");
            }
            else if (expression.NodeType == ExpressionType.Constant) {
                whereCompilerStr.Append(value.ToString());
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
            whereCompilerStr.Append(node.Value);
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            whereCompilerStr.Append(node.Name);
            return node;
        }


    }
}
