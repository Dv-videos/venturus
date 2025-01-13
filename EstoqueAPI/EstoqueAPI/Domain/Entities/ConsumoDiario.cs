namespace Domain.Entities
{
    public class ConsumoDiario
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int QuantidadeConsumida { get; set; }
        public DateTime Data { get; set; }
    }
}