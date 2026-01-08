namespace Listas
{
    public class ConjuntoProdutos
    {
        private HashSet<Produto> produtos;

        public ConjuntoProdutos()
        {
            produtos = new HashSet<Produto>(new ComparadorProduto());
            InicializarDados();
        }

        private void InicializarDados()
        {
            string[] nomes = { "Notebook", "Mouse", "Teclado", "Monitor", "Webcam", "Headset", "SSD", "Processador", "Memória RAM", "Placa Mãe",
                               "Fonte", "Cooler", "Hub USB", "Cabo HDMI", "Adaptador", "Carregador", "Bateria Externa", "Fone Bluetooth", "Smartphone", "Tablet",
                               "Smart TV", "Router", "Modem", "Roteador Wifi", "Câmera Digital", "Impressora", "Scanner", "Projetor", "Tela Tátil", "Sensor",
                               "LED RGB", "Ventilador", "Almofada Térmica", "Suporte Notebook", "Base para Mouse", "Mousepad", "Organizador Cabos", "Protetor Tela", "Capa", "Película",
                               "Carregador Rápido", "Cabo USB-C", "Cabo Lightning", "Adaptador USB", "Hub Ethernet", "Switch Ethernet", "Fibra Óptica", "Modem ADSL", "Antena Wifi", "Repetidor Wifi" };
            string[] categorias = { "Periféricos", "Componentes", "Acessórios", "Eletrônicos" };
            Random random = new Random();

            for (int i = 1; i <= 50; i++)
            {
                string nome = nomes[(i - 1) % nomes.Length];
                string categoria = categorias[random.Next(categorias.Length)];
                decimal preco = (decimal)(random.Next(50, 2000) + random.NextDouble());
                string descricao = $"Descrição do produto {i}";

                produtos.Add(new Produto(i, nome, descricao, categoria, preco));
            }
        }

        public List<Produto> BuscarTodos()
        {
            return produtos.ToList();
        }

        public Produto BuscarPorId(int id)
        {
            return produtos.FirstOrDefault(p => p.ID == id);
        }

        public List<Produto> BuscarPorNome(string nome)
        {
            return produtos.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void Acrescentar(Produto produto)
        {
            if (BuscarPorId(produto.ID) == null)
            {
                if (produtos.Add(produto))
                    Console.WriteLine($"Produto '{produto.Nome}' adicionado com sucesso!");
                else
                    Console.WriteLine("Erro ao adicionar produto!");
            }
            else
            {
                Console.WriteLine("Erro: Já existe um produto com este ID!");
            }
        }

        public void Alterar(int id, Produto produtoAtualizado)
        {
            var produto = BuscarPorId(id);
            if (produto != null)
            {
                produto.Nome = produtoAtualizado.Nome;
                produto.Descricao = produtoAtualizado.Descricao;
                produto.Categoria = produtoAtualizado.Categoria;
                produto.Preco = produtoAtualizado.Preco;
                Console.WriteLine($"Produto com ID {id} alterado com sucesso!");
            }
            else
            {
                Console.WriteLine("Erro: Produto não encontrado!");
            }
        }

        public void Deletar(int id)
        {
            var produto = BuscarPorId(id);
            if (produto != null)
            {
                if (produtos.Remove(produto))
                    Console.WriteLine($"Produto com ID {id} deletado com sucesso!");
                else
                    Console.WriteLine("Erro ao deletar produto!");
            }
            else
            {
                Console.WriteLine("Erro: Produto não encontrado!");
            }
        }
    }

    public class ComparadorProduto : IEqualityComparer<Produto>
    {
        public bool Equals(Produto x, Produto y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Produto obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}
