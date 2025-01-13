using MediatR;

public record DeleteProdutoCommand(int Id) : IRequest<bool>;