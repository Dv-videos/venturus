using Microsoft.AspNetCore.Mvc;
using MediatR;
using Domain.Entities;
using EstoqueAPI.Domain.Enums;
using System.Diagnostics.Eventing.Reader;
using EstoqueAPI.Application.Queries;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProdutosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoById(int id)
        {
            var produto = await _mediator.Send(new GetProdutoByIdQuery(id));
            return (produto == null || produto.Id == 0) ? NotFound() : Ok(produto);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProdutos()
        {
            var produtos = await _mediator.Send(new GetAllProdutosQuery());
            if (produtos == null || !produtos.Any())
            {
                return NotFound("Nenhum produto encontrado.");
            }
            return Ok(produtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID do produto não corresponde ao ID da rota.");
            }

            var success = await _mediator.Send(command);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var success = await _mediator.Send(new DeleteProdutoCommand(id));
            return success ? Ok() : NotFound();
        }

        [HttpPost("{id}/consumir")]
        public async Task<IActionResult> ConsumirEstoque(int id, [FromBody] ConsumirEstoqueCommand command)
        {
            if (id != command.ProdutoId)
            {
                return BadRequest("ID do produto não corresponde ao ID da rota.");
            }

            var success = await _mediator.Send(command);

            switch (success)
            {
                case AcaoConsumirTipoResultado.Sucesso:
                    return Ok();
                case AcaoConsumirTipoResultado.QuantidadeInsuficiente:
                    return BadRequest("Falha ao consumir devido a quantidade insuficiente.");
                case AcaoConsumirTipoResultado.Falha:
                    return BadRequest("Falha ao consumir o estoque.");
                default:
                    return BadRequest("Falha ao consumir o estoque.");
            }
        }

        [HttpGet("relatorios/consumo")]
        public async Task<IActionResult> RelatorioConsumo([FromQuery] DateTime? data)
        {
            var relatorio = await _mediator.Send(new RelatorioConsumoQuery(data ?? DateTime.Today));
            return Ok(relatorio);
        }

        [HttpGet("{id}/estoque")]
        public async Task<IActionResult> VerificarEstoque(int id)
        {
            var estoque = await _mediator.Send(new VerificarEstoqueQuery(id));
            return estoque != null ? Ok(estoque) : NotFound("Produto não encontrado ou estoque indisponível.");
        }

    }
}