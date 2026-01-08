namespace MeuTeste.Domain.DTOs
{
    public class ProductInputDto
    {
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
