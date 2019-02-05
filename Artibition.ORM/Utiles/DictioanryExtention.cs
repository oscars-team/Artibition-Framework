using Artibition.ORM.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM
{
    public static class DictioanryExtention
    {
        public static void Update<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            T2 exist;
            if (dictionary.TryGetValue(key, out exist)) {
                if (!exist.Equals(value))
                    dictionary[key] = value;
            }
            else
                dictionary.Add(key, value);
        }
    }


}
