using System;
using System.Data;

namespace Artibition.Repository
{
    public interface IRepository
    {
        IDbConnection Connection { get; }

        TEntity GetSingle<TEntity>(object keyValue);

    }
}
