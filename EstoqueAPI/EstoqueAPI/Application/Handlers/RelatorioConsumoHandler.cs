namespace EstoqueAPI.Application.Handlers;

using MediatR;
using EstoqueAPI.Application.Queries;
using EstoqueAPI.Domain.DinamicEntities;

public class RelatorioConsumoHandler : IRequestHandler<RelatorioConsumoQuery, IEnumerable<RelatorioConsumo>>
{
    private readonly IDapperExecutor _dapperExecutor;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RelatorioConsumoHandler(IDbConnectionFactory dbConnectionFactory, IDapperExecutor dapperExecutor)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dapperExecutor = dapperExecutor;
    }

    public async Task<IEnumerable<RelatorioConsumo>> Handle(RelatorioConsumoQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var query = @"SELECT 
                        cd.produto_id AS ProdutoId,
                        p.nome AS Nome,
                        SUM(cd.quantidade_consumida) AS QuantidadeConsumida,
                        SUM(cd.quantidade_consumida * p.preco_medio) AS CustoTotal
                    FROM ConsumoDiario cd
                    INNER JOIN produtos p ON cd.produto_id = p.id
                    WHERE cd.data = @Data
                    GROUP BY cd.produto_id, p.nome;";

        var relatorio = await _dapperExecutor.QueryAsync<RelatorioConsumo>(connection, query, new { Data = request.Data });
        return relatorio;
    }
}
