using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    /// <summary>
    /// ISQL 接口
    /// 用于定义编译方法
    /// </summary>
    public interface ISQL
    {
        ISQLSelect Select<TEntity>(Expression<Func<TEntity, object>> selector = null);

        string Compile();
    }
}
