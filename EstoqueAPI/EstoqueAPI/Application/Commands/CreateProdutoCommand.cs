using MediatR;

public record CreateProdutoCommand(string Nome, string PartNumber, decimal PrecoMedio, int EstoqueAtual) : IRequest<int>;
