namespace MeuTeste.Domain.Entities
{
    public class Product : Base
    {
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public virtual Category? Category { get; set; }
    }
}
