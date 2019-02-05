using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public interface IEntityMapper
    {
        string TableName { get; }
        string[] GetPrimaryKeys();
        string[] GetIdentityKeys();
        string[] Columns { get; }
        string[] Fields { get; }
        string GetColumnName(string column);
        void Register(Type type);

    }
}
