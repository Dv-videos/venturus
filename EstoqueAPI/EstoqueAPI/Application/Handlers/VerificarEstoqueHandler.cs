using MediatR;
using EstoqueAPI.Application.Queries;
using EstoqueAPI.Domain.DinamicEntities;

public class VerificarEstoqueHandler : IRequestHandler<VerificarEstoqueQuery, EstoqueDisponivel>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDapperExecutor _dapperExecutor;

    public VerificarEstoqueHandler(IDbConnectionFactory dbConnectionFactory, IDapperExecutor dapperExecutor)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dapperExecutor = dapperExecutor;
    }

    public async Task<EstoqueDisponivel> Handle(VerificarEstoqueQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var query = @"SELECT 
                        p.id AS ProdutoId,
                        p.nome AS Nome,
                        p.estoque_atual AS EstoqueAtual
                      FROM produtos p
                      WHERE p.id = @Id;";

        return await _dapperExecutor.QuerySingleOrDefaultAsync<EstoqueDisponivel>(
            connection, query, new { Id = request.ProdutoId });
    }
}
