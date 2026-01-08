namespace Listas
{
    public class ListaDuplamenteLigada
    {
        private class No
        {
            public Produto Dado { get; set; }
            public No Proximo { get; set; }
            public No Anterior { get; set; }

            public No(Produto dado)
            {
                Dado = dado;
                Proximo = null;
                Anterior = null;
            }
        }

        private No inicio;
        private No fim;
        private int tamanho;

        public ListaDuplamenteLigada()
        {
            inicio = null;
            fim = null;
            tamanho = 0;
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

                Acrescentar(new Produto(i, nome, descricao, categoria, preco));
            }
        }

        public List<Produto> BuscarTodos()
        {
            List<Produto> resultado = new List<Produto>();
            No atual = inicio;
            while (atual != null)
            {
                resultado.Add(atual.Dado);
                atual = atual.Proximo;
            }
            return resultado;
        }

        public Produto BuscarPorId(int id)
        {
            No atual = inicio;
            while (atual != null)
            {
                if (atual.Dado.ID == id)
                    return atual.Dado;
                atual = atual.Proximo;
            }
            return null;
        }

        public List<Produto> BuscarPorNome(string nome)
        {
            List<Produto> resultado = new List<Produto>();
            No atual = inicio;
            while (atual != null)
            {
                if (atual.Dado.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                    resultado.Add(atual.Dado);
                atual = atual.Proximo;
            }
            return resultado;
        }

        public void Acrescentar(Produto produto)
        {
            if (BuscarPorId(produto.ID) != null)
            {
                Console.WriteLine("Erro: Já existe um produto com este ID!");
                return;
            }

            No novoNo = new No(produto);
            if (inicio == null)
            {
                inicio = novoNo;
                fim = novoNo;
            }
            else
            {
                fim.Proximo = novoNo;
                novoNo.Anterior = fim;
                fim = novoNo;
            }
            tamanho++;
            Console.WriteLine($"Produto '{produto.Nome}' adicionado com sucesso!");
        }

        public void Alterar(int id, Produto produtoAtualizado)
        {
            No atual = inicio;
            while (atual != null)
            {
                if (atual.Dado.ID == id)
                {
                    atual.Dado.Nome = produtoAtualizado.Nome;
                    atual.Dado.Descricao = produtoAtualizado.Descricao;
                    atual.Dado.Categoria = produtoAtualizado.Categoria;
                    atual.Dado.Preco = produtoAtualizado.Preco;
                    Console.WriteLine($"Produto com ID {id} alterado com sucesso!");
                    return;
                }
                atual = atual.Proximo;
            }
            Console.WriteLine("Erro: Produto não encontrado!");
        }

        public void Deletar(int id)
        {
            No atual = inicio;
            while (atual != null)
            {
                if (atual.Dado.ID == id)
                {
                    if (atual.Anterior != null)
                        atual.Anterior.Proximo = atual.Proximo;
                    else
                        inicio = atual.Proximo;

                    if (atual.Proximo != null)
                        atual.Proximo.Anterior = atual.Anterior;
                    else
                        fim = atual.Anterior;

                    tamanho--;
                    Console.WriteLine($"Produto com ID {id} deletado com sucesso!");
                    return;
                }
                atual = atual.Proximo;
            }
            Console.WriteLine("Erro: Produto não encontrado!");
        }
    }
}
