using MediatR;
using Dapper;
using Domain.Entities;
using System.Data;

public class GetProdutoByIdHandler : IRequestHandler<GetProdutoByIdQuery, Produto>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    private readonly IDapperExecutor _dapperExecutor;

    public GetProdutoByIdHandler(IDbConnectionFactory dbConnectionFactory, IDapperExecutor dapperExecutor)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dapperExecutor = dapperExecutor;
    }

    public async Task<Produto> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = "SELECT * FROM Produtos WHERE id = @Id;";
        var produto = await _dapperExecutor.QuerySingleOrDefaultAsync<Produto>(connection, query, new { request.Id });

        return produto;
    }
}