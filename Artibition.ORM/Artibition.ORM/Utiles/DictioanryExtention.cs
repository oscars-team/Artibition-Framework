using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM
{
    public static class DictioanryExtention
    {
        public static void AddUpdate(this Dictionary<Type, IEntityMapper> dictionary, Type key, IEntityMapper value)
        {
            IEntityMapper obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
