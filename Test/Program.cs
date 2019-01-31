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
            var user = new User() { age = 10, name = "leo" };
            var name = "leo";
            Expression<Func<User, bool>> someExpr = tb1 => tb1.name == user.name;
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
            object value = null;
            switch (node.Expression.NodeType) {
                // 固定参数
                case ExpressionType.Parameter:
                    var param = node.Expression as ParameterExpression;
                    Console.Write($"{param.Name}.{node.Member.Name}");
                    break;
                case ExpressionType.Constant:
                    value = getConstantValue(node, node.Expression as ConstantExpression);
                    Console.Write(value.ToString());
                    break;
                case ExpressionType.MemberAccess:
                    value = getMemberAccessValue(node, node.Expression as MemberExpression);
                    Console.WriteLine(value.ToString());
                    break;

            }
            return node;
            //return VisitMember(node, node.Expression);
        }

        /// <summary>
        /// 获取常量表达式中常量的值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private object getConstantValue(MemberExpression root, ConstantExpression node)
        {
            /**
             *  类似于：
             *  var name = 'test'
             *  p=>p.prop == name
             * 
             */
            var rootExp = root as MemberExpression;
            var propName = rootExp.Member.Name;
            var propType = node.Type;
            var prop = propType.GetField(propName);
            if (prop != null)
                return prop.GetValue(node.Value);

            return null;
            //var value = node.Value;
            //var name = root.Member.Name;
            //var type = consExp.Type;
            //var field = type.GetField(name) ?? type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //if (field != null)
            //    return field.GetValue(value);
            //else {
            //    var prop = type.GetProperty(name) ?? type.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            //    if (prop != null) {
            //        return prop.GetValue(value, null);
            //    }
            //}
            //return null;
        }

        /// <summary>
        /// 获取属性访问表达式所访问的属性值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private object getMemberAccessValue(MemberExpression root, MemberExpression node)
        {
            switch (node.Expression.NodeType) {
                /**
                 *  表达类似与：
                 *  var obj = new WhateveryClass(){ prop1 = 1, prop2 = "2" }
                 *  p=>p.prop == obj.prop1
                 */
                case ExpressionType.Constant:

                    break;
            }
            return getMemberAccessValue(root, node.Expression as MemberExpression);
        }
        //private object getMemberAccessValue(Expression root, Expression node)
        //{
        //    var value = consExp.Value;
        //    var name = mbExp.Member.Name;
        //    var type = consExp.Type;
        //    var field = type.GetField(name) ?? type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //    if (field != null)
        //        return field.GetValue(value);
        //    else {
        //        var prop = type.GetProperty(name) ?? type.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //        if (prop != null) {
        //            return prop.GetValue(value, null);
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 表达式为一个常量时执行
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            // 表达式类似于 p=>p.proprty == 1, p=>p.property == "123"
            Console.Write(node.Value);
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Console.Write(node.Name);
            return node;
        }
    }
}
