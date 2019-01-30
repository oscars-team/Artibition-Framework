﻿using System;
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
        SQL Select<TEntity>(string alias = null, Expression<Func<TEntity, object>> selector = null);
        SQLWhere Where<TEntity>(string alias = null, Expression<Func<TEntity, bool>> where = null);

        string Compile();
    }
}