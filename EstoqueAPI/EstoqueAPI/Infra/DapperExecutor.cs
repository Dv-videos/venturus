using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

public class DapperExecutor : IDapperExecutor
{
    public async Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
    {
        return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null)
    {
        return await connection.ExecuteAsync(sql, param);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null)
    {
        return await connection.QueryAsync<T>(sql, param);
    }
}
