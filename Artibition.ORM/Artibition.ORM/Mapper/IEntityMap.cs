using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public interface IEntityMapper
    {
        void Register<TEntity>();
        string GetTableName();
        string[] GetPrimaryKeys();
        string[] GetIdentityKeys();
    }
}
