using MediatR;

public record UpdateProdutoCommand(int Id, string Nome, string PartNumber, decimal PrecoMedio, int EstoqueAtual) : IRequest<bool>;