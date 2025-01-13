using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using EstoqueAPI.Application.Handlers;
using EstoqueAPI.Application.Queries;
using EstoqueAPI.Domain.DinamicEntities;

public class VerificarEstoqueHandlerTests
{
    [Fact]
    public async Task Should_Return_EstoqueDisponivel_When_Produto_Exists()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        var expectedEstoque = new EstoqueDisponivel
        {
            ProdutoId = 1,
            Nome = "Produto A",
            EstoqueAtual = 15
        };

        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<EstoqueDisponivel>(
                mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(expectedEstoque);

        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new VerificarEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new VerificarEstoqueQuery(1); // Produto com ID 1

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ProdutoId.Should().Be(1);
        result.Nome.Should().Be("Produto A");
        result.EstoqueAtual.Should().Be(15);
    }

    [Fact]
    public async Task Should_Return_Null_When_Produto_Does_Not_Exist()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        mockDapperExecutor
            .Setup(exec => exec.QuerySingleOrDefaultAsync<EstoqueDisponivel>(
                mockDbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync((EstoqueDisponivel)null);

        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new VerificarEstoqueHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new VerificarEstoqueQuery(999); // Produto inexistente

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
