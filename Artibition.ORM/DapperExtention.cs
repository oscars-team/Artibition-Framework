using Artibition.ORM.SQLBuilder;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Artibition.ORM
{
    public static class DapperExtention
    {
        public static int Excute(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);
            return SqlMapper.Execute(cnn, strSql, param, transaction, commandTimeout, commandType);
        }
        public static async Task<int> ExcuteAsync(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.ExecuteAsync(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static IDataReader ExcuteReader(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.ExecuteReader(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<IDataReader> ExcuteReaderAsync(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.ExecuteReaderAsync(cnn, strSql, param, transaction, commandTimeout, commandType);
        }


        public static object ExcuteScalar(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.ExecuteScalar(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<object> ExcuteScalarAsync(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.ExecuteScalarAsync(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static T ExcuteScalar<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.ExecuteScalar<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<T> ExcuteScalarAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.ExecuteScalarAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<dynamic> Query(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.Query(cnn, strSql, param, transaction, buffered, commandTimeout, commandType);
        }

        public static async Task<IEnumerable<dynamic>> QueryAsync(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryAsync(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.Query<T>(cnn, strSql, param, transaction, buffered, commandTimeout, commandType);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }


        public static dynamic QueryFirst(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QueryFirst(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<dynamic> QueryFirstAsync(this IDbConnection cnn, SQL sql, Type type, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryFirstAsync(cnn, type, strSql, param, transaction, commandTimeout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QueryFirst(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<T> QueryFirstAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryFirstAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }


        public static dynamic QueryFirstOrDefault(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QueryFirstOrDefault(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<dynamic> QueryFirstOrDefaultAsync(this IDbConnection cnn, SQL sql, Type type, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryFirstOrDefaultAsync(cnn, type, strSql, param, transaction, commandTimeout, commandType);
        }


        public static T QueryFirstOrDefault<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QueryFirstOrDefault<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QueryFirstOrDefaultAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static dynamic QuerySingle(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QuerySingle(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<dynamic> QuerySingleAsync(this IDbConnection cnn, SQL sql, Type type, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QuerySingleAsync(cnn, type, strSql, param, transaction, commandTimeout, commandType);
        }

        public static T QuerySingle<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QuerySingle<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<T> QuerySingleAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QuerySingleAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }


        public static dynamic QuerySingleOrDefault(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QuerySingleOrDefault(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<dynamic> QuerySingleOrDefaultAsync(this IDbConnection cnn, SQL sql, Type type, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QuerySingleOrDefaultAsync(cnn, type, strSql, param, transaction, commandTimeout, commandType);
        }


        public static T QuerySingleOrDefault<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return SqlMapper.QuerySingleOrDefault<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

        public static async Task<T> QuerySingleOrDefaultAsync<T>(this IDbConnection cnn, SQL sql, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var strSql = sql.Compile();
            var param = new DapperParameter(sql.Parameters);

            return await SqlMapper.QuerySingleOrDefaultAsync<T>(cnn, strSql, param, transaction, commandTimeout, commandType);
        }

    }
}
