using MediatR;
using System.Collections.Generic;
using Domain.Entities;

public record GetAllProdutosQuery() : IRequest<IEnumerable<Produto>>;