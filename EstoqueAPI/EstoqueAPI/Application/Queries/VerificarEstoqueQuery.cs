namespace EstoqueAPI.Application.Queries
{
    using EstoqueAPI.Domain.DinamicEntities;
    using MediatR;

    public record VerificarEstoqueQuery(int ProdutoId) : IRequest<EstoqueDisponivel>;


}
