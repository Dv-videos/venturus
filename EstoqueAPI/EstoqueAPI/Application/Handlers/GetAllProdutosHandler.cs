using MediatR;
using Dapper;
using Domain.Entities;
using System.Data;

public class GetAllProdutosHandler : IRequestHandler<GetAllProdutosQuery, IEnumerable<Produto>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetAllProdutosHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Produto>> Handle(GetAllProdutosQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = "SELECT * FROM Produtos;";
        return await connection.QueryAsync<Produto>(query);
    }
}