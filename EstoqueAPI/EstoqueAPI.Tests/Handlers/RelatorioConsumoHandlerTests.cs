using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using EstoqueAPI.Application.Handlers;
using EstoqueAPI.Application.Queries;
using EstoqueAPI.Domain.DinamicEntities;

public class RelatorioConsumoHandlerTests
{
    [Fact]
    public async Task Should_Return_Relatorio_When_Data_Exists()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock do DapperExecutor para retornar dados do relatório
        mockDapperExecutor
            .Setup(exec => exec.QueryAsync<RelatorioConsumo>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(new List<RelatorioConsumo>
            {
                new RelatorioConsumo { ProdutoId = 1, Nome = "Produto A", QuantidadeConsumida = 10, CustoTotal = 100.0M },
                new RelatorioConsumo { ProdutoId = 2, Nome = "Produto B", QuantidadeConsumida = 5, CustoTotal = 50.0M }
            });

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new RelatorioConsumoHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new RelatorioConsumoQuery(DateTime.Now.Date);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(r => r.ProdutoId == 1 && r.Nome == "Produto A" && r.QuantidadeConsumida == 10 && r.CustoTotal == 100.0M);
        result.Should().ContainSingle(r => r.ProdutoId == 2 && r.Nome == "Produto B" && r.QuantidadeConsumida == 5 && r.CustoTotal == 50.0M);
    }

    [Fact]
    public async Task Should_Return_Empty_When_No_Data_Exists()
    {
        // Arrange
        var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
        var mockDapperExecutor = new Mock<IDapperExecutor>();
        var mockDbConnection = new Mock<IDbConnection>();

        // Configura o mock do DapperExecutor para retornar nenhum dado
        mockDapperExecutor
            .Setup(exec => exec.QueryAsync<RelatorioConsumo>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(new List<RelatorioConsumo>());

        // Configura o mock da fábrica de conexão
        mockDbConnectionFactory
            .Setup(factory => factory.CreateConnection())
            .Returns(mockDbConnection.Object);

        var handler = new RelatorioConsumoHandler(mockDbConnectionFactory.Object, mockDapperExecutor.Object);
        var query = new RelatorioConsumoQuery(DateTime.Now.Date);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
