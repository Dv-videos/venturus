using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

public class GetProdutoByIdHandlerTests
{
    [Fact]
    public async Task Should_Return_Produto_When_Exists()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock do DapperExecutor
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<Produto>(
                mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(new Produto
            {
                Id = 1,
                Nome = "Produto Teste",
                EstoqueAtual = 10,
                PrecoMedio = 100.0M
            });

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new GetProdutoByIdHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new GetProdutoByIdQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Nome.Should().Be("Produto Teste");
        result.EstoqueAtual.Should().Be(10);
        result.PrecoMedio.Should().Be(100.0M);
    }

    [Fact]
    public async Task Should_Return_Null_When_Produto_Not_Exists()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock do DapperExecutor
        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<Produto>(
                mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync((Produto)null);

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new GetProdutoByIdHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new GetProdutoByIdQuery(999); // ID inexistente

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
