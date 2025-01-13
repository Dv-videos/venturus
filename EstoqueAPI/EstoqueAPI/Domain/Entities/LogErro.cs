namespace Domain.Entities
{
    public class LogErro
    {
        public int Id { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DataHora { get; set; }
    }
}