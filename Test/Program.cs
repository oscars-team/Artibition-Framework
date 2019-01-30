using Artibition.ORM.SQLBuilder;
using System;
using System.Linq.Expressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)

        {
            //var sql = new SQL().Select<User>().Where<User>(p => p.name == "leo").And<User>(p => p.age == 10);
            int? age = 1;
            var name = "leo";
            Expression<Func<User, bool>> someExpr = tb1 => tb1.age == age && tb1.name == name;
            var builder = new WhereBuilder();
            builder.Visit(someExpr.Body);
            Console.WriteLine();
            Console.ReadLine();
        }


    }

    public class WhereBuilder : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Console.Write("(");

            this.Visit(node.Left);

            switch (node.NodeType) {
                case ExpressionType.Add:
                    Console.Write(" + ");
                    break;
                case ExpressionType.Divide:
                    Console.Write(" / ");
                    break;
                case ExpressionType.Equal:
                    Console.Write(" = ");
                    break;
                case ExpressionType.AndAlso:
                    Console.Write(" and ");
                    break;
            }

            this.Visit(node.Right);

            Console.Write(")");

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            switch (node.Expression.NodeType) {
                // 固定参数
                case ExpressionType.Parameter:
                    var param = node.Expression as ParameterExpression;
                    Console.Write($"{param.Name}.{node.Member.Name}");
                    break;
                case ExpressionType.Constant:
                    switch (node.Expression.NodeType) {
                        case ExpressionType.Constant: //表示为一个常数
                            var cons = node.Expression as ConstantExpression;
                            var t = cons.Type;
                            var value = getMemberConstValue(node, cons);
                            Console.Write(value.ToString());
                            break;
                    }
                    break;
            }
            return node;
        }

        private object getMemberConstValue(MemberExpression mbExp, ConstantExpression consExp)
        {
            var value = consExp.Value;
            var name = mbExp.Member.Name;
            var type = consExp.Type;
            var field = type.GetField(name) ?? type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                return field.GetValue(value);
            else {
                var prop = type.GetProperty(name) ?? type.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (prop != null) {
                    return prop.GetValue(value, null);
                }
            }
            return null;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {

            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Console.Write(node.Name);
            return node;
        }
    }
}
