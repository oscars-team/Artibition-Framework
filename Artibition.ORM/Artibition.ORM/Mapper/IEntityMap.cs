using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public interface IEntityMapper
    {

        void Register(Type type);
        string GetTableName();
        string[] GetPrimaryKeys();
        string[] GetIdentityKeys();
        string[] Columns { get; }
        string[] Fields { get; }
        string GetColumnName(string column);
    }
}
