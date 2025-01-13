using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public interface IDapperExecutor
{
    Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
    Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null);
    Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null); // Adicionado
}
