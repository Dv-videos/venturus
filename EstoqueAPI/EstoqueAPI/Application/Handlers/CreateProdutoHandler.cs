using MediatR;
using Dapper;
using System.Data;

public class CreateProdutoHandler : IRequestHandler<CreateProdutoCommand, int>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CreateProdutoHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = @"INSERT INTO Produtos (nome, partnumber, preco_medio, estoque_atual)
                      VALUES (@Nome, @PartNumber, @PrecoMedio, @EstoqueAtual);
                      SELECT LAST_INSERT_ID();";
        var id = await connection.ExecuteScalarAsync<int>(query, request);
        return id;
    }
}