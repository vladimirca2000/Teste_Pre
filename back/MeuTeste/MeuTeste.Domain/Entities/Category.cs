namespace MeuTeste.Domain.Entities
{
    public class Category : Base
    {
        public required string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
