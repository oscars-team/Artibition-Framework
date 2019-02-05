using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public partial class SQLCompiler : ExpressionVisitor, ISQLCompile
    {
        public SQL Sql { get; }
        ISQLOrderBy _compilingOrder = null;
        private StringBuilder _selectorCompilerStr;
        private StringBuilder _whereCompilerStr;
        private StringBuilder _orderCompilerStr;
        private StringBuilder _joinsCompilerStr;
        private StringBuilder _currentJoinCompilerStr;
        private ISQLJoin _currentCompilingJoin;
        //Type _visitingBinaryLeftSideType;
        //Type _visitingBinaryRightSideType;
        //string _visitingBinaryLeftSideAlias;
        //string _visitingBinaryRightSideAlias;

        public SQLCompiler(SQL sql)
        {
            Sql = sql;
        }
        private string compileSelector()
        {
            ISQLSelect sqlSelect = Sql.Selector;
            Expression selectExpression = sqlSelect.Expression;
            IEntityMapper selectorMapper = sqlSelect.Mapper;
            _selectorCompilerStr = new StringBuilder("SELECT ");
            if (selectExpression == null) {
                if (!string.IsNullOrEmpty(sqlSelect.Alias))
                    // 被As强制定义了别名
                    Sql.AliasCollection.Update(sqlSelect.Alias, Sql.Selector.Type);

                _selectorCompilerStr.Append(
                    selectorMapper.Columns.Length == 0
                    ? sqlSelect.ComposeSelectKey("*")
                    : string.Join(", ", selectorMapper.Columns.Select(p => sqlSelect.ComposeSelectKey(p)))
                );
            }
            else {
                this.Visit(selectExpression);
                _selectorCompilerStr.TrimComma();
            }
            _selectorCompilerStr.Append(" FROM ");
            _selectorCompilerStr.Append(sqlSelect.ComposeTableName());
            string compileResult = _selectorCompilerStr.ToString();
            _selectorCompilerStr = null;
            return compileResult;
        }
        private string compileJoins()
        {
            ISQLJoin[] joins = Sql.Joins?.ToArray();
            _joinsCompilerStr = new StringBuilder();
            if (joins?.Count() > 0) {
                ISQLJoin j;
                int c = joins.Count();
                for (var i = 0; i < c; i++) {
                    j = joins[i];
                    if (i == c - 1)
                        _joinsCompilerStr.Append(compileJoin(j));
                    else
                        _joinsCompilerStr.AppendLine(compileJoin(j));
                }
            }
            string compileResult = _joinsCompilerStr.ToString();
            _joinsCompilerStr = null;
            return compileResult;
        }

        private string compileJoin(ISQLJoin join)
        {
            _currentJoinCompilerStr = new StringBuilder();
            switch (join.JoinType) {
                default:
                    _currentJoinCompilerStr.Append("INNER JOIN ");
                    break;
                case JoinType.CrossJoin:
                    _currentJoinCompilerStr.Append("CROSS JOIN ");
                    break;
                case JoinType.FullJoin:
                    _currentJoinCompilerStr.Append("FULL JOIN ");
                    break;
                case JoinType.LeftJoin:
                    _currentJoinCompilerStr.Append("LEFT JOIN ");
                    break;
                case JoinType.RightJoin:
                    _currentJoinCompilerStr.Append("RIGHT JOIN ");
                    break;
            }

            string alias = null;
            // 查找第一个类型匹配的参数名，作为alias
            foreach (var p in join.Expression.Parameters) {
                if (p.Type == join.Type) {
                    alias = p.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(alias)) throw new Exception($"SQLCompiler.compileJoin Error: Join中找不到类型为\"{join.Type.ToString()}\"的参数");
            if (Sql.AliasCollection.ContainsKey(alias)) throw new Exception($"SQLCompiler.compileJoin Error: Join中别名\"{alias}\"重复，请检查是否在Select,As,Join中定义过");
            Sql.AliasCollection.Update(alias, join.Type);
            _currentJoinCompilerStr.Append($"{join.ComposeTableName()} AS {alias}");
            _currentJoinCompilerStr.Append(" ON ");
            _currentCompilingJoin = join;
            this.Visit(join.Expression.Body);
            string compileResult = _currentJoinCompilerStr.ToString();
            _currentCompilingJoin = null;
            _currentJoinCompilerStr = null;
            return compileResult;
        }

        private string compileWhere()
        {
            Expression where = Sql.Where?.Lambda.Body;
            _whereCompilerStr = new StringBuilder();
            if (where != null) {
                _whereCompilerStr.Append("WHERE ");
                this.Visit(where);
            }
            string compileResult = _whereCompilerStr.ToString();
            _whereCompilerStr = null;
            return compileResult;
        }
        private string compileOrder()
        {
            _orderCompilerStr = new StringBuilder();
            List<ISQLOrderBy> orders = Sql.Orders;
            if (orders?.Count > 0) {
                _orderCompilerStr.Append("ORDER BY ");
                orders.ForEach(o => {
                    _compilingOrder = o;
                    this.Visit(o.Expression);
                    _compilingOrder = null;
                });
            }

            string compileResult = _orderCompilerStr.TrimComma().ToString();
            _orderCompilerStr = null;
            return compileResult;
        }
        public string Compile()
        {
            StringBuilder sqlBulder = new StringBuilder();
            // 编译Select
            string sqlSelect = compileSelector();
            if (!string.IsNullOrEmpty(sqlSelect))
                sqlBulder.AppendLine(sqlSelect);
            // 编译Joins
            string sqlJoins = compileJoins();
            if (!string.IsNullOrEmpty(sqlJoins))
                sqlBulder.AppendLine(sqlJoins);
            // 编译Where
            string sqlWhere = compileWhere();
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlBulder.AppendLine(sqlWhere);
            // 编译Order
            string sqlOrders = compileOrder();
            if (!string.IsNullOrEmpty(sqlOrders))
                sqlBulder.AppendLine(sqlOrders);

            return sqlBulder.ToString();
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _whereCompilerStr?.Append("(");
            _currentJoinCompilerStr?.Append("(");
            this.Visit(node.Left);

            switch (node.NodeType) {
                case ExpressionType.Add:
                    _whereCompilerStr?.Append(" + ");
                    _currentJoinCompilerStr?.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    _whereCompilerStr?.Append(" - ");
                    _currentJoinCompilerStr?.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    _whereCompilerStr?.Append(" * ");
                    _currentJoinCompilerStr?.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    _whereCompilerStr?.Append(" / ");
                    _currentJoinCompilerStr?.Append(" / ");
                    break;
                case ExpressionType.Equal:
                    _whereCompilerStr?.Append(" = ");
                    _currentJoinCompilerStr?.Append(" = ");
                    break;

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _whereCompilerStr?.Append(" AND ");
                    _currentJoinCompilerStr?.Append(" AND ");
                    break;

                case ExpressionType.Not:
                    _whereCompilerStr?.Append(" NOT ");
                    _currentJoinCompilerStr?.Append(" NOT ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _whereCompilerStr?.Append(" OR ");
                    _currentJoinCompilerStr?.Append(" OR ");
                    break;

                case ExpressionType.Modulo:
                    _whereCompilerStr?.Append(" % ");
                    _currentJoinCompilerStr?.Append(" % ");
                    break;
            }

            this.Visit(node.Right);
            _whereCompilerStr?.Append(")");
            _currentJoinCompilerStr?.Append(")");
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

                if (_selectorCompilerStr != null) {
                    // 编译select语句
                    if (!string.IsNullOrEmpty(Sql.Selector.Alias)) {
                        // 被As强制定义了别名
                        if (!string.IsNullOrEmpty(alias) && alias != Sql.Selector.Alias)
                            throw new Exception($"SQLSelect.ConfirmAlias Error: 别名\"{ Sql.Selector.Alias}\"不可用，因为当前的Select中已经定义了别名\"{alias}\"，请确保Select表达式\"{alias}=>\"中的名称与As(\"{Sql.Selector.Alias}\")中的名称一致");
                    }
                    else {
                        // 没有被As强制定义别名
                        Sql.Selector.Alias = alias;
                    }
                    Sql.AliasCollection.Update(alias, Sql.Selector.Type);
                    _selectorCompilerStr?.Append($"{Sql.Selector.ComposeSelectKey(mapper.GetColumnName(node.Member.Name))},");
                }

                if (_whereCompilerStr != null) {
                    // 编译where语句
                    if (Sql.AliasCollection.Count == 0) {
                        // 没有Alias，不显示
                        _whereCompilerStr.Append($"{mapper.GetColumnName(node.Member.Name)}");
                    }
                    else {
                        if (Sql.AliasCollection.ContainsKey(alias))
                            _whereCompilerStr.Append($"{alias}.{mapper.GetColumnName(node.Member.Name)}");
                        else
                            throw new Exception($"SQLCompiler.VisitMember Error: 你正在有意使用别名\"{alias}\"，但在别名集合中没找到。尝试检查在Expression中使用的参数名是否在Select中使用过，或者是否在方法As()中定义过。'");
                    }
                }

                if (_orderCompilerStr != null) {
                    // 编译order语句
                    if (Sql.AliasCollection.Count == 0) {
                        // 没有Alias，不显示
                        _orderCompilerStr.Append($"{mapper.GetColumnName(node.Member.Name)}{(_compilingOrder.By == OrderBy.DESC ? " DESC" : "")},");
                    }
                    else {
                        if (Sql.AliasCollection.ContainsKey(alias))
                            _orderCompilerStr.Append($"{alias}.{mapper.GetColumnName(node.Member.Name)}{(_compilingOrder.By == OrderBy.DESC ? " DESC" : "")},");
                        else
                            throw new Exception($"SQLCompiler.VisitMember Error: 你正在有意使用别名\"{alias}\"，但在别名集合中没找到。尝试检查在Expression中使用的参数名是否在Select中使用过，或者是否在方法As()中定义过。'");
                    }
                }

                if (_currentJoinCompilerStr != null) {
                    // 编译Join语句
                    _currentJoinCompilerStr.Append($"{alias}.{mapper.GetColumnName(node.Member.Name)}");
                }
            }

            else if (expression.NodeType == ExpressionType.Constant) {
                _whereCompilerStr?.Append(value.ToString());
                _currentJoinCompilerStr?.Append(value.ToString());
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
            _currentJoinCompilerStr?.Append(node.Value);
            return node;
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            _whereCompilerStr?.Append(node.Name);
            _currentJoinCompilerStr?.Append(node.Name);
            return node;
        }
    }
}
