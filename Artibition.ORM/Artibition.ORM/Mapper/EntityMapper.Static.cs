using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public partial class EntityMapper
    {
        private static Dictionary<Type, IEntityMapper> _entityMapper = new Dictionary<Type, IEntityMapper>();
        public static void map(Type etype, IEntityMapper mtype)
        {
            _entityMapper.AddUpdate(etype, mtype);
        }

        public static void map<TEntity>(IEntityMapper mtype)
        {
            _entityMapper.AddUpdate(typeof(TEntity), mtype);
        }

        public static IEntityMapper getMapper(Type t)
        {
            return _entityMapper[t];
        }

        private static IEnumerable<Type> EnumerateEntityMappers()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    foreach (var t in type.GetInterfaces()) {
                        if (t == typeof(IEntityMapper)) {
                            yield return type;
                            break;
                        }
                    }
                }
            }
        }

        public static bool IsMapperRegistered { get; private set; } = false;

        public static void RegisterEntityMappers()
        {
            if (!IsMapperRegistered) {
                foreach (var t in EnumerateEntityMappers()) {
                    Activator.CreateInstance(t);
                }
                IsMapperRegistered = true;
            }
        }
    }
}
