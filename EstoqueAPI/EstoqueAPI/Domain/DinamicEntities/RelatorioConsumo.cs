namespace EstoqueAPI.Domain.DinamicEntities
{
    public class RelatorioConsumo
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public int QuantidadeConsumida { get; set; }
        public decimal CustoTotal { get; set; }
    }
}

