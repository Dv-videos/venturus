using MediatR;
using Dapper;
using System.Data;

public class DeleteProdutoHandler : IRequestHandler<DeleteProdutoCommand, bool>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DeleteProdutoHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = "DELETE FROM Produtos WHERE id = @Id;";
        var rowsAffected = await connection.ExecuteAsync(query, new { request.Id });
        return rowsAffected > 0;
    }
}
