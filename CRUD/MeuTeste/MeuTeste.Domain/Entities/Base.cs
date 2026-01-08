namespace MeuTeste.Domain.Entities
{
    public abstract class Base
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
