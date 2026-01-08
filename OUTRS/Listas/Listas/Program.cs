// See https://aka.ms/new-console-template for more information
using Listas;

bool executando = true;

while (executando)
{
    int opcaoTipo = 0;
    bool selecionado = false;

    while (!selecionado)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║    SISTEMA DE GERENCIAMENTO DE PRODUTOS    ║");
        Console.WriteLine("║         Escolha o tipo de lista:           ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.WriteLine("║  1 - Lista Comum (Vetor)                   ║");
        Console.WriteLine("║  2 - Lista Duplamente Ligada               ║");
        Console.WriteLine("║  3 - Conjunto (HashSet)                    ║");
        Console.WriteLine("║  4 - Dicionário                            ║");
        Console.WriteLine("║  0 - Sair do programa                      ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.Write("\nDigite sua opção: ");

        if (int.TryParse(Console.ReadLine(), out opcaoTipo) && opcaoTipo >= 0 && opcaoTipo <= 4)
        {
            selecionado = true;
        }
        else
        {
            Console.WriteLine("\nOpção inválida! Pressione qualquer tecla para tentar novamente...");
            Console.ReadKey();
        }
    }

    if (opcaoTipo == 0)
    {
        executando = false;
    }
    else
    {
        switch (opcaoTipo)
        {
            case 1:
                ExecutarListaComum();
                break;
            case 2:
                ExecutarListaDuplamenteLigada();
                break;
            case 3:
                ExecutarConjunto();
                break;
            case 4:
                ExecutarDicionario();
                break;
        }
    }
}

Console.Clear();
Console.WriteLine("\n╔════════════════════════════════════════════╗");
Console.WriteLine("║       Obrigado por usar o sistema!         ║");
Console.WriteLine("╚════════════════════════════════════════════╝\n");

void ExecutarListaComum()
{
    ListaComum lista = new ListaComum();
    bool continuar = true;

    while (continuar)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║    LISTA COMUM - GERENCIAMENTO DE PRODUTOS ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.WriteLine("║  1 - Buscar todos os produtos              ║");
        Console.WriteLine("║  2 - Buscar por ID                         ║");
        Console.WriteLine("║  3 - Buscar por Nome                       ║");
        Console.WriteLine("║  4 - Acrescentar produto                   ║");
        Console.WriteLine("║  5 - Alterar produto                       ║");
        Console.WriteLine("║  6 - Deletar produto                       ║");
        Console.WriteLine("║  0 - Voltar ao menu principal              ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.Write("\nDigite sua opção: ");

        string opcao = Console.ReadLine();
        Console.WriteLine();

        switch (opcao)
        {
            case "1":
                ExibirTodosProdutos(lista.BuscarTodos());
                break;
            case "2":
                BuscarPorIdComum(lista);
                break;
            case "3":
                BuscarPorNomeComum(lista);
                break;
            case "4":
                AcrescentarProdutoComum(lista);
                break;
            case "5":
                AlterarProdutoComum(lista);
                break;
            case "6":
                DeletarProdutoComum(lista);
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida!");
                break;
        }

        if (continuar && opcao != "0")
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}

void ExecutarListaDuplamenteLigada()
{
    ListaDuplamenteLigada lista = new ListaDuplamenteLigada();
    bool continuar = true;

    while (continuar)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║ LISTA DUPLAMENTE LIGADA - GERENCIAMENTO    ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.WriteLine("║  1 - Buscar todos os produtos              ║");
        Console.WriteLine("║  2 - Buscar por ID                         ║");
        Console.WriteLine("║  3 - Buscar por Nome                       ║");
        Console.WriteLine("║  4 - Acrescentar produto                   ║");
        Console.WriteLine("║  5 - Alterar produto                       ║");
        Console.WriteLine("║  6 - Deletar produto                       ║");
        Console.WriteLine("║  0 - Voltar ao menu principal              ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.Write("\nDigite sua opção: ");

        string opcao = Console.ReadLine();
        Console.WriteLine();

        switch (opcao)
        {
            case "1":
                ExibirTodosProdutos(lista.BuscarTodos());
                break;
            case "2":
                BuscarPorIdDuplamenteLigada(lista);
                break;
            case "3":
                BuscarPorNomeDuplamenteLigada(lista);
                break;
            case "4":
                AcrescentarProdutoDuplamenteLigada(lista);
                break;
            case "5":
                AlterarProdutoDuplamenteLigada(lista);
                break;
            case "6":
                DeletarProdutoDuplamenteLigada(lista);
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida!");
                break;
        }

        if (continuar && opcao != "0")
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}

void ExecutarConjunto()
{
    ConjuntoProdutos conjunto = new ConjuntoProdutos();
    bool continuar = true;

    while (continuar)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║    CONJUNTO - GERENCIAMENTO DE PRODUTOS    ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.WriteLine("║  1 - Buscar todos os produtos              ║");
        Console.WriteLine("║  2 - Buscar por ID                         ║");
        Console.WriteLine("║  3 - Buscar por Nome                       ║");
        Console.WriteLine("║  4 - Acrescentar produto                   ║");
        Console.WriteLine("║  5 - Alterar produto                       ║");
        Console.WriteLine("║  6 - Deletar produto                       ║");
        Console.WriteLine("║  0 - Voltar ao menu principal              ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.Write("\nDigite sua opção: ");

        string opcao = Console.ReadLine();
        Console.WriteLine();

        switch (opcao)
        {
            case "1":
                ExibirTodosProdutos(conjunto.BuscarTodos());
                break;
            case "2":
                BuscarPorIdConjunto(conjunto);
                break;
            case "3":
                BuscarPorNomeConjunto(conjunto);
                break;
            case "4":
                AcrescentarProdutoConjunto(conjunto);
                break;
            case "5":
                AlterarProdutoConjunto(conjunto);
                break;
            case "6":
                DeletarProdutoConjunto(conjunto);
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida!");
                break;
        }

        if (continuar && opcao != "0")
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}

void ExecutarDicionario()
{
    DicionarioProdutos dicionario = new DicionarioProdutos();
    bool continuar = true;

    while (continuar)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║  DICIONÁRIO - GERENCIAMENTO DE PRODUTOS    ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.WriteLine("║  1 - Buscar todos os produtos              ║");
        Console.WriteLine("║  2 - Buscar por ID                         ║");
        Console.WriteLine("║  3 - Buscar por Nome                       ║");
        Console.WriteLine("║  4 - Acrescentar produto                   ║");
        Console.WriteLine("║  5 - Alterar produto                       ║");
        Console.WriteLine("║  6 - Deletar produto                       ║");
        Console.WriteLine("║  0 - Voltar ao menu principal              ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.Write("\nDigite sua opção: ");

        string opcao = Console.ReadLine();
        Console.WriteLine();

        switch (opcao)
        {
            case "1":
                ExibirTodosProdutos(dicionario.BuscarTodos());
                break;
            case "2":
                BuscarPorIdDicionario(dicionario);
                break;
            case "3":
                BuscarPorNomeDicionario(dicionario);
                break;
            case "4":
                AcrescentarProdutoDicionario(dicionario);
                break;
            case "5":
                AlterarProdutoDicionario(dicionario);
                break;
            case "6":
                DeletarProdutoDicionario(dicionario);
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida!");
                break;
        }

        if (continuar && opcao != "0")
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}

// ============= Método auxiliar para alterar com prévia =============
string LerCampoComPadrão(string label, string valorPadrão)
{
    Console.Write($"{label} [{valorPadrão}]: ");
    string entrada = Console.ReadLine();
    return string.IsNullOrEmpty(entrada) ? valorPadrão : entrada;
}

decimal LerDecimalComPadrão(string label, decimal valorPadrão)
{
    while (true)
    {
        Console.Write($"{label} [R$ {valorPadrão:F2}]: ");
        string entrada = Console.ReadLine();
        
        if (string.IsNullOrEmpty(entrada))
            return valorPadrão;
        
        if (decimal.TryParse(entrada, out decimal resultado))
            return resultado;
        
        Console.WriteLine("Valor inválido! Tente novamente.");
    }
}

// ============= Métodos auxiliares para ListaComum =============
void BuscarPorIdComum(ListaComum lista)
{
    Console.Write("Digite o ID do produto: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var produto = lista.BuscarPorId(id);
        if (produto != null)
            Console.WriteLine($"\nProduto encontrado: {produto}");
        else
            Console.WriteLine("\nProduto não encontrado!");
    }
    else
        Console.WriteLine("ID inválido!");
}

void BuscarPorNomeComum(ListaComum lista)
{
    Console.Write("Digite o nome do produto: ");
    string nome = Console.ReadLine();
    var produtos = lista.BuscarPorNome(nome);
    if (produtos.Count > 0)
    {
        Console.WriteLine($"\n{produtos.Count} produto(s) encontrado(s):");
        foreach (var p in produtos)
            Console.WriteLine(p);
    }
    else
        Console.WriteLine("Nenhum produto encontrado!");
}

void AcrescentarProdutoComum(ListaComum lista)
{
    Console.Write("Digite o ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    if (lista.BuscarPorId(id) != null)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com ID {id}!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite o nome: ");
    string nome = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("\n⚠ ERRO: O nome não pode estar vazio!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    if (lista.BuscarPorNome(nome).Count > 0)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com nome '{nome}'!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite a descrição: ");
    string descricao = Console.ReadLine();

    Console.Write("Digite a categoria: ");
    string categoria = Console.ReadLine();

    Console.Write("Digite o preço: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
    {
        Console.WriteLine("Preço inválido!");
        return;
    }

    lista.Acrescentar(new Produto(id, nome, descricao, categoria, preco));
}

void AlterarProdutoComum(ListaComum lista)
{
    Console.Write("Digite o ID do produto a alterar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produtoExistente = lista.BuscarPorId(id);
    if (produtoExistente == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO ATUAL:                             ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produtoExistente}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    Console.WriteLine("(Deixe em branco para manter o valor atual)\n");

    string nome = LerCampoComPadrão("Novo nome", produtoExistente.Nome);
    string descricao = LerCampoComPadrão("Nova descrição", produtoExistente.Descricao);
    string categoria = LerCampoComPadrão("Nova categoria", produtoExistente.Categoria);
    decimal preco = LerDecimalComPadrão("Novo preço", produtoExistente.Preco);

    lista.Alterar(id, new Produto(id, nome, descricao, categoria, preco));
}

void DeletarProdutoComum(ListaComum lista)
{
    Console.Write("Digite o ID do produto a deletar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produto = lista.BuscarPorId(id);
    if (produto == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO A DELETAR:                         ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produto}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    
    Console.Write("Deseja realmente deletar este produto? (S/N): ");
    string resposta = Console.ReadLine();

    if (resposta?.ToUpper() == "S")
        lista.Deletar(id);
    else
        Console.WriteLine("Operação cancelada!");
}

// ============= Métodos auxiliares para ListaDuplamenteLigada =============
void BuscarPorIdDuplamenteLigada(ListaDuplamenteLigada lista)
{
    Console.Write("Digite o ID do produto: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var produto = lista.BuscarPorId(id);
        if (produto != null)
            Console.WriteLine($"\nProduto encontrado: {produto}");
        else
            Console.WriteLine("\nProduto não encontrado!");
    }
    else
        Console.WriteLine("ID inválido!");
}

void BuscarPorNomeDuplamenteLigada(ListaDuplamenteLigada lista)
{
    Console.Write("Digite o nome do produto: ");
    string nome = Console.ReadLine();
    var produtos = lista.BuscarPorNome(nome);
    if (produtos.Count > 0)
    {
        Console.WriteLine($"\n{produtos.Count} produto(s) encontrado(s):");
        foreach (var p in produtos)
            Console.WriteLine(p);
    }
    else
        Console.WriteLine("Nenhum produto encontrado!");
}

void AcrescentarProdutoDuplamenteLigada(ListaDuplamenteLigada lista)
{
    Console.Write("Digite o ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    if (lista.BuscarPorId(id) != null)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com ID {id}!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite o nome: ");
    string nome = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("\n⚠ ERRO: O nome não pode estar vazio!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    if (lista.BuscarPorNome(nome).Count > 0)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com nome '{nome}'!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite a descrição: ");
    string descricao = Console.ReadLine();

    Console.Write("Digite a categoria: ");
    string categoria = Console.ReadLine();

    Console.Write("Digite o preço: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
    {
        Console.WriteLine("Preço inválido!");
        return;
    }

    lista.Acrescentar(new Produto(id, nome, descricao, categoria, preco));
}

void AlterarProdutoDuplamenteLigada(ListaDuplamenteLigada lista)
{
    Console.Write("Digite o ID do produto a alterar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produtoExistente = lista.BuscarPorId(id);
    if (produtoExistente == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO ATUAL:                             ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produtoExistente}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    Console.WriteLine("(Deixe em branco para manter o valor atual)\n");

    string nome = LerCampoComPadrão("Novo nome", produtoExistente.Nome);
    string descricao = LerCampoComPadrão("Nova descrição", produtoExistente.Descricao);
    string categoria = LerCampoComPadrão("Nova categoria", produtoExistente.Categoria);
    decimal preco = LerDecimalComPadrão("Novo preço", produtoExistente.Preco);

    lista.Alterar(id, new Produto(id, nome, descricao, categoria, preco));
}

void DeletarProdutoDuplamenteLigada(ListaDuplamenteLigada lista)
{
    Console.Write("Digite o ID do produto a deletar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produto = lista.BuscarPorId(id);
    if (produto == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO A DELETAR:                         ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produto}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    
    Console.Write("Deseja realmente deletar este produto? (S/N): ");
    string resposta = Console.ReadLine();

    if (resposta?.ToUpper() == "S")
        lista.Deletar(id);
    else
        Console.WriteLine("Operação cancelada!");
}

// ============= Métodos auxiliares para Conjunto =============
void BuscarPorIdConjunto(ConjuntoProdutos conjunto)
{
    Console.Write("Digite o ID do produto: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var produto = conjunto.BuscarPorId(id);
        if (produto != null)
            Console.WriteLine($"\nProduto encontrado: {produto}");
        else
            Console.WriteLine("\nProduto não encontrado!");
    }
    else
        Console.WriteLine("ID inválido!");
}

void BuscarPorNomeConjunto(ConjuntoProdutos conjunto)
{
    Console.Write("Digite o nome do produto: ");
    string nome = Console.ReadLine();
    var produtos = conjunto.BuscarPorNome(nome);
    if (produtos.Count > 0)
    {
        Console.WriteLine($"\n{produtos.Count} produto(s) encontrado(s):");
        foreach (var p in produtos)
            Console.WriteLine(p);
    }
    else
        Console.WriteLine("Nenhum produto encontrado!");
}

void AcrescentarProdutoConjunto(ConjuntoProdutos conjunto)
{
    Console.Write("Digite o ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    if (conjunto.BuscarPorId(id) != null)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com ID {id}!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite o nome: ");
    string nome = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("\n⚠ ERRO: O nome não pode estar vazio!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    if (conjunto.BuscarPorNome(nome).Count > 0)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com nome '{nome}'!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite a descrição: ");
    string descricao = Console.ReadLine();

    Console.Write("Digite a categoria: ");
    string categoria = Console.ReadLine();

    Console.Write("Digite o preço: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
    {
        Console.WriteLine("Preço inválido!");
        return;
    }

    conjunto.Acrescentar(new Produto(id, nome, descricao, categoria, preco));
}

void AlterarProdutoConjunto(ConjuntoProdutos conjunto)
{
    Console.Write("Digite o ID do produto a alterar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produtoExistente = conjunto.BuscarPorId(id);
    if (produtoExistente == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO ATUAL:                             ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produtoExistente}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    Console.WriteLine("(Deixe em branco para manter o valor atual)\n");

    string nome = LerCampoComPadrão("Novo nome", produtoExistente.Nome);
    string descricao = LerCampoComPadrão("Nova descrição", produtoExistente.Descricao);
    string categoria = LerCampoComPadrão("Nova categoria", produtoExistente.Categoria);
    decimal preco = LerDecimalComPadrão("Novo preço", produtoExistente.Preco);

    conjunto.Alterar(id, new Produto(id, nome, descricao, categoria, preco));
}

void DeletarProdutoConjunto(ConjuntoProdutos conjunto)
{
    Console.Write("Digite o ID do produto a deletar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produto = conjunto.BuscarPorId(id);
    if (produto == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO A DELETAR:                         ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produto}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    
    Console.Write("Deseja realmente deletar este produto? (S/N): ");
    string resposta = Console.ReadLine();

    if (resposta?.ToUpper() == "S")
        conjunto.Deletar(id);
    else
        Console.WriteLine("Operação cancelada!");
}

// ============= Métodos auxiliares para Dicionário =============
void BuscarPorIdDicionario(DicionarioProdutos dicionario)
{
    Console.Write("Digite o ID do produto: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var produto = dicionario.BuscarPorId(id);
        if (produto != null)
            Console.WriteLine($"\nProduto encontrado: {produto}");
        else
            Console.WriteLine("\nProduto não encontrado!");
    }
    else
        Console.WriteLine("ID inválido!");
}

void BuscarPorNomeDicionario(DicionarioProdutos dicionario)
{
    Console.Write("Digite o nome do produto: ");
    string nome = Console.ReadLine();
    var produtos = dicionario.BuscarPorNome(nome);
    if (produtos.Count > 0)
    {
        Console.WriteLine($"\n{produtos.Count} produto(s) encontrado(s):");
        foreach (var p in produtos)
            Console.WriteLine(p);
    }
    else
        Console.WriteLine("Nenhum produto encontrado!");
}

void AcrescentarProdutoDicionario(DicionarioProdutos dicionario)
{
    Console.Write("Digite o ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    if (dicionario.BuscarPorId(id) != null)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com ID {id}!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite o nome: ");
    string nome = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("\n⚠ ERRO: O nome não pode estar vazio!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    if (dicionario.BuscarPorNome(nome).Count > 0)
    {
        Console.WriteLine($"\n⚠ ERRO: Já existe um produto com nome '{nome}'!");
        Console.WriteLine("Pressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.Write("Digite a descrição: ");
    string descricao = Console.ReadLine();

    Console.Write("Digite a categoria: ");
    string categoria = Console.ReadLine();

    Console.Write("Digite o preço: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
    {
        Console.WriteLine("Preço inválido!");
        return;
    }

    dicionario.Acrescentar(new Produto(id, nome, descricao, categoria, preco));
}

void AlterarProdutoDicionario(DicionarioProdutos dicionario)
{
    Console.Write("Digite o ID do produto a alterar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produtoExistente = dicionario.BuscarPorId(id);
    if (produtoExistente == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO ATUAL:                             ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produtoExistente}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    Console.WriteLine("(Deixe em branco para manter o valor atual)\n");

    string nome = LerCampoComPadrão("Novo nome", produtoExistente.Nome);
    string descricao = LerCampoComPadrão("Nova descrição", produtoExistente.Descricao);
    string categoria = LerCampoComPadrão("Nova categoria", produtoExistente.Categoria);
    decimal preco = LerDecimalComPadrão("Novo preço", produtoExistente.Preco);

    dicionario.Alterar(id, new Produto(id, nome, descricao, categoria, preco));
}

void DeletarProdutoDicionario(DicionarioProdutos dicionario)
{
    Console.Write("Digite o ID do produto a deletar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido!");
        return;
    }

    var produto = dicionario.BuscarPorId(id);
    if (produto == null)
    {
        Console.WriteLine("Erro: Produto não encontrado!");
        return;
    }

    Console.WriteLine($"\n╔════════════════════════════════════════════╗");
    Console.WriteLine($"║ PRODUTO A DELETAR:                         ║");
    Console.WriteLine($"╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ {produto}");
    Console.WriteLine($"╚════════════════════════════════════════════╝\n");
    
    Console.Write("Deseja realmente deletar este produto? (S/N): ");
    string resposta = Console.ReadLine();

    if (resposta?.ToUpper() == "S")
        dicionario.Deletar(id);
    else
        Console.WriteLine("Operação cancelada!");
}

// ============= Método auxiliar para exibir todos os produtos =============
void ExibirTodosProdutos(List<Produto> produtos)
{
    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto registrado!");
        return;
    }

    Console.WriteLine($"Total de produtos: {produtos.Count}\n");
    for (int i = 0; i < produtos.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {produtos[i]}");
    }
}
