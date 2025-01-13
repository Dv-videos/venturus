using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Controllers;
using EstoqueAPI.Application.Queries;
using EstoqueAPI.Domain.Enums;
using Domain.Entities;
using EstoqueAPI.Domain.DinamicEntities;

public class ProdutosControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly ProdutosController _controller;

    public ProdutosControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new ProdutosController(_mockMediator.Object);
    }

    [Fact]
    public async Task CreateProduto_Should_Return_Ok_With_Id()
    {
        // Arrange
        var expectedId = 1;
        var command = new CreateProdutoCommand(
            "Produto A",  // Nome
            "P001",       // PartNumber
            10.5m,         // PrecoMedio
            100            // EstoqueAtual
        );

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CreateProdutoCommand>(), default))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateProduto(command);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expectedId);
    }

    [Fact]
    public async Task GetProdutoById_Should_Return_Ok_When_Produto_Exists()
    {
        // Arrange
        var expectedProduto = new Produto
        {
            Id = 1,
            Nome = "Produto A",
            PartNumber = "P001",
            PrecoMedio = 10.5m,
            EstoqueAtual = 100
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProdutoByIdQuery>(), default))
            .ReturnsAsync(expectedProduto);

        // Act
        var result = await _controller.GetProdutoById(1);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedProduto);
    }

    [Fact]
    public async Task GetProdutoById_Should_Return_NotFound_When_Produto_Does_Not_Exist()
    {
        // Arrange
        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetProdutoByIdQuery>(), default))
            .ReturnsAsync((Produto)null);

        // Act
        var result = await _controller.GetProdutoById(999);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAllProdutos_Should_Return_Ok_With_Produtos()
    {
        // Arrange
        var expectedProdutos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Produto A", PartNumber = "P001", PrecoMedio = 10.5m, EstoqueAtual = 100 },
            new Produto { Id = 2, Nome = "Produto B", PartNumber = "P002", PrecoMedio = 20.0m, EstoqueAtual = 50 }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetAllProdutosQuery>(), default))
            .ReturnsAsync(expectedProdutos);

        // Act
        var result = await _controller.GetAllProdutos();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedProdutos);
    }

    [Fact]
    public async Task GetAllProdutos_Should_Return_NotFound_When_No_Produtos()
    {
        // Arrange
        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetAllProdutosQuery>(), default))
            .ReturnsAsync(new List<Produto>());

        // Act
        var result = await _controller.GetAllProdutos();

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be("Nenhum produto encontrado.");
    }

    [Fact]
    public async Task DeleteProduto_Should_Return_Ok_When_Successful()
    {
        // Arrange
        _mockMediator
            .Setup(m => m.Send(It.IsAny<DeleteProdutoCommand>(), default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProduto(1);

        // Assert
        var okResult = result as OkResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ConsumirEstoque_Should_Return_BadRequest_When_Quantidade_Insufficient()
    {
        // Arrange
        _mockMediator
            .Setup(m => m.Send(It.IsAny<ConsumirEstoqueCommand>(), default))
            .ReturnsAsync(AcaoConsumirTipoResultado.QuantidadeInsuficiente);

        var command = new ConsumirEstoqueCommand(
            1,   // ProdutoId
            50   // QuantidadeConsumida
        );

        // Act
        var result = await _controller.ConsumirEstoque(1, command);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Falha ao consumir devido a quantidade insuficiente.");
    }

    [Fact]
    public async Task RelatorioConsumo_Should_Return_Ok_With_Relatorio()
    {
        // Arrange
        var expectedRelatorio = new List<RelatorioConsumo>
        {
            new RelatorioConsumo { ProdutoId = 1, Nome = "Produto A", QuantidadeConsumida = 10, CustoTotal = 100.0m }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RelatorioConsumoQuery>(), default))
            .ReturnsAsync(expectedRelatorio);

        // Act
        var result = await _controller.RelatorioConsumo(DateTime.Today);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedRelatorio);
    }
}
