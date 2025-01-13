using MediatR;
using Dapper;
using System.Data;

public class UpdateProdutoHandler : IRequestHandler<UpdateProdutoCommand, bool>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UpdateProdutoHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var query = @"UPDATE Produtos 
                      SET nome = @Nome, partnumber = @PartNumber, preco_medio = @PrecoMedio, estoque_atual = @EstoqueAtual
                      WHERE id = @Id;";
        var rowsAffected = await connection.ExecuteAsync(query, request);
        return rowsAffected > 0;
    }
}
