using EstoqueAPI.Domain.Enums;
using MediatR;

public record ConsumirEstoqueCommand(int ProdutoId, int QuantidadeConsumida) : IRequest<AcaoConsumirTipoResultado>;