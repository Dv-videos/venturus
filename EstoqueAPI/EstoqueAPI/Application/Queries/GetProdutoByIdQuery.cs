using MediatR;
using Domain.Entities;

public record GetProdutoByIdQuery(int Id) : IRequest<Produto>;