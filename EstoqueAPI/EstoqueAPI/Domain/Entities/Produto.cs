namespace Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? PartNumber { get; set; }
        public decimal PrecoMedio { get; set; }
        public int EstoqueAtual { get; set; }
    }
}