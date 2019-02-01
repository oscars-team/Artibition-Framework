using System;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.Mapper
{
    public partial class EntityMapper
    {
        private static Dictionary<Type, IEntityMapper> _entityMapper = new Dictionary<Type, IEntityMapper>();
        /// <summary>
        /// 将类型映射Mapper实体
        /// </summary>
        /// <param name="etype"></param>
        /// <param name="mtype"></param>
        public static void Map(Type etype, IEntityMapper mtype)
        {
            _entityMapper.Update(etype, mtype);
        }
        /// <summary>
        /// 将类型映射值实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mtype"></param>
        public static void Map<TEntity>(IEntityMapper mtype)
        {
            _entityMapper.Update(typeof(TEntity), mtype);
        }
        /// <summary>
        /// 根据类型获取响应实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEntityMapper GetMapper(Type t)
        {
            return _entityMapper[t];
        }

        public static IEntityMapper GetMapper<TEntity>()
        {
            return _entityMapper[typeof(TEntity)];
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

        /// <summary>
        /// 是否做过类型配置映射
        /// </summary>
        public static bool IsMapperRegistered { get; private set; } = false;
        /// <summary>
        /// 开始做类型配置映射
        /// </summary>
        public static void RegisterEntityMappers()
        {
            if (!IsMapperRegistered) {
                foreach (var t in EnumerateEntityMappers()) {
                    if (!t.Name.Contains("EntityMapper"))
                        Activator.CreateInstance(t);
                }
                IsMapperRegistered = true;
            }
        }
    }
}
