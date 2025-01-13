using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using EstoqueAPI.Domain.Enums;
using EstoqueAPI.Domain.DinamicEntities;

public class ConsumirEstoqueHandlerTests
{
    [Fact]
    public async Task Should_Return_Sucesso_When_Estoque_Is_Consumed_Successfully()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock para verificar o estoque disponível
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<ProdutoEstoque>(
                mockDbConnection.Object,
                "SELECT estoque_atual AS EstoqueAtual FROM Produtos WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync(new ProdutoEstoque { EstoqueAtual = 10 }); // Estoque suficiente

        // Configura o mock para atualizar o estoque
        mockDapperExecutor
            .Setup(exec => exec.ExecuteAsync(
                mockDbConnection.Object,
                "UPDATE Produtos SET estoque_atual = estoque_atual - @QuantidadeConsumida WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync(1); // Sucesso na atualização

        // Configura o mock para registrar o consumo diário
        mockDapperExecutor
            .Setup(exec => exec.ExecuteAsync(
                mockDbConnection.Object,
                "INSERT INTO ConsumoDiario (produto_id, quantidade_consumida, data) VALUES (@ProdutoId, @QuantidadeConsumida, CURDATE());",
                It.IsAny<object>()))
            .ReturnsAsync(1); // Sucesso no registro

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new ConsumirEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var command = new ConsumirEstoqueCommand(1, 5); // Produto ID = 1, Consumir 5 unidades

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(AcaoConsumirTipoResultado.Sucesso);
    }

    [Fact]
    public async Task Should_Return_QuantidadeInsuficiente_When_Estoque_Is_Insufficient()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock para verificar o estoque disponível
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<ProdutoEstoque>(
                mockDbConnection.Object,
                "SELECT estoque_atual AS EstoqueAtual FROM Produtos WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync(new ProdutoEstoque { EstoqueAtual = 3 }); // Estoque insuficiente

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new ConsumirEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var command = new ConsumirEstoqueCommand(1, 5); // Produto ID = 1, Consumir 5 unidades

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(AcaoConsumirTipoResultado.QuantidadeInsuficiente);
    }

    [Fact]
    public async Task Should_Return_QuantidadeInsuficiente_When_Produto_Does_Not_Exist()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock para retornar null, simulando que o produto não existe
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<ProdutoEstoque>(
                mockDbConnection.Object,
                "SELECT estoque_atual AS EstoqueAtual FROM Produtos WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync((ProdutoEstoque)null);

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new ConsumirEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var command = new ConsumirEstoqueCommand(999, 5); // Produto inexistente

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(AcaoConsumirTipoResultado.QuantidadeInsuficiente);
    }

    [Fact]
    public async Task Should_Return_Falha_When_Update_Fails()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock para verificar o estoque disponível
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<ProdutoEstoque>(
                mockDbConnection.Object,
                "SELECT estoque_atual AS EstoqueAtual FROM Produtos WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync(new ProdutoEstoque { EstoqueAtual = 10 }); // Estoque suficiente

        // Configura o mock para falhar na atualização do estoque
        mockDapperExecutor
            .Setup(exec => exec.ExecuteAsync(
                mockDbConnection.Object,
                "UPDATE Produtos SET estoque_atual = estoque_atual - @QuantidadeConsumida WHERE id = @Id;",
                It.IsAny<object>()))
            .ReturnsAsync(0); // Falha na atualização

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new ConsumirEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var command = new ConsumirEstoqueCommand(1, 5); // Produto ID = 1, Consumir 5 unidades

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(AcaoConsumirTipoResultado.Falha);
    }
}
