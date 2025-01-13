using MediatR;
using EstoqueAPI.Domain.Enums;
using EstoqueAPI.Domain.DinamicEntities;

public class ConsumirEstoqueHandler : IRequestHandler<ConsumirEstoqueCommand, AcaoConsumirTipoResultado>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDapperExecutor _dapperExecutor;

    public ConsumirEstoqueHandler(IDbConnectionFactory dbConnectionFactory, IDapperExecutor dapperExecutor)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dapperExecutor = dapperExecutor;
    }

    public async Task<AcaoConsumirTipoResultado> Handle(ConsumirEstoqueCommand request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        // Verifica se o produto existe e tem estoque suficiente
        var produto = await _dapperExecutor.QuerySingleOrDefaultAsync<ProdutoEstoque>(
            connection,
            "SELECT estoque_atual AS EstoqueAtual FROM Produtos WHERE id = @Id;",
            new { Id = request.ProdutoId });

        if ((produto == null) || (produto.EstoqueAtual < request.QuantidadeConsumida))
        {
            return AcaoConsumirTipoResultado.QuantidadeInsuficiente;
        }

        // Atualiza o estoque do produto
        var rowsAffected = await _dapperExecutor.ExecuteAsync(
            connection,
            @"UPDATE Produtos SET estoque_atual = estoque_atual - @QuantidadeConsumida WHERE id = @Id;",
            new { QuantidadeConsumida = request.QuantidadeConsumida, Id = request.ProdutoId });

        if (rowsAffected > 0)
        {
            // Registra o consumo diário
            await _dapperExecutor.ExecuteAsync(
                connection,
                @"INSERT INTO ConsumoDiario (produto_id, quantidade_consumida, data)
                  VALUES (@ProdutoId, @QuantidadeConsumida, CURDATE());",
                new { ProdutoId = request.ProdutoId, QuantidadeConsumida = request.QuantidadeConsumida });

            return AcaoConsumirTipoResultado.Sucesso;
        }

        return AcaoConsumirTipoResultado.Falha;
    }
}
