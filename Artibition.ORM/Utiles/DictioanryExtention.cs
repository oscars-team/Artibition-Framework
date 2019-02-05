using Artibition.ORM.Mapper;
using Artibition.ORM.SQLDialect;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static SQLParameter AddParameter(this List<SQLParameter> parameters, object value, ParameterDirection direction)
        {
            int c = parameters.Count + 1;
            SQLParameter param = new SQLParameter($"{DialectProvider.PARAM_PREFIX}{c}", value, direction);
            parameters.Add(param);
            return param;
        }

        public static SQLParameter AddParameter(this List<SQLParameter> parameters, object value)
        {
            int c = parameters.Count + 1;
            SQLParameter param = new SQLParameter($"{DialectProvider.PARAM_PREFIX}{c}", value);
            parameters.Add(param);
            return param;
        }
    }


}
