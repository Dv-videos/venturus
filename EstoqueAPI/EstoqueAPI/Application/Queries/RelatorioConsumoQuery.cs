namespace EstoqueAPI.Application.Queries;

using EstoqueAPI.Domain.DinamicEntities;
using MediatR;
using System.Collections.Generic;

public record RelatorioConsumoQuery(DateTime Data) : IRequest<IEnumerable<RelatorioConsumo>>;
