using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public partial class SQL
    {
        private List<string> processSelector(Expression e = null)
        {
            var selectors = new List<string>();
            if (e != null) {
                switch (e.NodeType) {
                    // 表达式为Lambda: p=>p.xxx
                    case ExpressionType.Lambda:
                        var lambdaExp = e as LambdaExpression;
                        processSelector(lambdaExp.Body);
                        break;
                    // 表达式为Convert: Convert(p.xxx)
                    case ExpressionType.Convert:
                        var unaryExp = e as UnaryExpression;
                        processSelector(unaryExp.Operand);
                        break;
                    // 表达式为new: p => new{ } , 后面的new { }
                    case ExpressionType.New:
                        var newExp = e as NewExpression;
                        foreach (var arg in newExp.Arguments)
                            selectors.Add(compileSelector(arg));
                        break;
                    // 表达式为MemberAccess: p.xxx
                    case ExpressionType.MemberAccess:
                        selectors.Add(compileSelector(e));
                        break;
                }
            }
            if (selectors.Count == 0)
                selectors.Add("*");

            return selectors;
        }

        private string compileSelector(Expression e)
        {
            switch (e.NodeType) {
                case ExpressionType.MemberAccess:
                    return (e as MemberExpression).Member.Name;

                default:
                    throw new Exception("Unrecognized node type (" + e.NodeType.ToString() + ")");
            }
        }


    }
}
