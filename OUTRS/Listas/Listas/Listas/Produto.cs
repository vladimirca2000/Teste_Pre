namespace Listas
{
    public class Produto
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public decimal Preco { get; set; }

        public Produto(int id, string nome, string descricao, string categoria, decimal preco)
        {
            ID = id;
            Nome = nome;
            Descricao = descricao;
            Categoria = categoria;
            Preco = preco;
        }

        public override string ToString()
        {
            return $"[ID: {ID}] {Nome} | Categoria: {Categoria} | Descrição: {Descricao} | Preço: R$ {Preco:F2}";
        }
    }
}
