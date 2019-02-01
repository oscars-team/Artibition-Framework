using Artibition.ORM.SQLBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Test
{
    class Program
    {
        static void Main(string[] args)

        {
            //var sql = new SQL().Select<User>().Where<User>(p => p.name == "leo").And<User>(p => p.age == 10);
            var user = new User() { age = 10, name = "leo" };
            var room = new Room() { users = new List<User>() { user } };
            Expression<Func<User, bool>> someExpr = tb1 => tb1.name == room.users[0].name;
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
            if (node.Expression.NodeType == ExpressionType.Parameter) {
                var param = node.Expression as ParameterExpression;
                Console.Write($"{param.Name}.{node.Member.Name}");
            }
            else {
                value = getMemberAccessValue(node, node.Expression);
                Console.Write(value.ToString());
            }
            return node;
        }

        private object getMemberAccessValue(Expression rootExpression, Expression nodeExpression)
        {
            FieldInfo field;
            PropertyInfo property;


            if (rootExpression.NodeType == ExpressionType.MemberAccess) {
                var root = rootExpression as MemberExpression;
                if (nodeExpression.NodeType == ExpressionType.Constant) {
                    var node = nodeExpression as ConstantExpression;
                    field = node.Type.GetField(root.Member.Name);
                    return field?.GetValue(node.Value);
                }

                if (nodeExpression.NodeType == ExpressionType.MemberAccess) {
                    var node = nodeExpression as MemberExpression;
                    object value = getMemberAccessValue(node, node.Expression);
                    property = node.Type.GetProperty(root.Member.Name);
                    return property?.GetValue(value);
                }

                if (nodeExpression.NodeType == ExpressionType.ArrayIndex) {
                    var node = nodeExpression as BinaryExpression;
                    int index;
                    object[] array;
                    if (node.Right.NodeType == ExpressionType.Constant)
                        index = (int)getMemberAccessValue(node.Right, node.Right);
                    else
                        index = (int)getMemberAccessValue(node.Right, (node.Right as MemberExpression).Expression);

                    array = getMemberAccessValue(node.Left, (node.Left as MemberExpression).Expression) as object[];
                    property = node.Type.GetProperty(root.Member.Name);
                    return property.GetValue(array[index]);

                }

            }

            if (rootExpression.NodeType == ExpressionType.Constant)
                return (rootExpression as ConstantExpression).Value;


            return null;
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
