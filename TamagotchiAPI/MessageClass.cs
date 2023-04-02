public class MessageClass
{
    public string NomeJogador { get; set; }
    public Jogador jogador { get; set; }


    public MessageClass()
    {
        MessageClass mc = this;
        jogador = new Jogador();
        BoasVindas();
    }

    public void BoasVindas()
    {
        Console.WriteLine(@" 
     #######                                                                    
        #       ##    #    #    ##     ####    ####   #####   ####   #    #  # 
        #      #  #   ##  ##   #  #   #    #  #    #    #    #    #  #    #  # 
        #     #    #  # ## #  #    #  #       #    #    #    #       ######  # 
        #     ######  #    #  ######  #  ###  #    #    #    #       #    #  # 
        #     #    #  #    #  #    #  #    #  #    #    #    #    #  #    #  # 
        #     #    #  #    #  #    #   ####    ####     #     ####   #    #  #");

        Console.Write("\n\nQual é seu nome? ");
        jogador.Name = Console.ReadLine().ToUpper();
    }

    public Status MenuInicial()
    {
        Console.WriteLine($"\n\n -------------------- MENU --------------------");
        Console.WriteLine(
            $"{jogador.Name} Escolha uma opção:\n " +
            $"1 - Adotar um mascote\n " +
            $"2 - Ver colecao\n " +
            $"3 - Sair\n");

        string resposta = Console.ReadLine();

        if(int.TryParse(resposta, out int n))
        {
            switch(n)
            {
                case 1:
                    return Status.ADOTAR;
                    break;
                case 2:
                    return Status.COLECAO;
                    break;
                case 3:
                    return Status.SAIR;
                    break;

            }
        }
        Console.WriteLine("Opção inválida.");
        return Status.MENU;
    }


    public int? MenuAdotar()
    {
        Console.Write("Escolha uma espécie entre 1 e 1010: ");
        string resposta = Console.ReadLine();

        if (int.TryParse(resposta, out int n))
        {
            if (n > 0 && n < 1010)
            {
                return n;
            }
        }

        Console.WriteLine("Opção inválida.");

        return null;
    }

    public void PrintColecao(List<Mascote> colecao)
    {
        foreach (Mascote mascote in colecao)
        {
            Console.WriteLine(mascote.name);
        }
    }
    public void PrintMascote(Mascote mascote)
    {
        Console.Write($"\n" +
            $"POKEMON SELECIONADO:\n" +
            $"Nome: {mascote.name} \n" +
            $"Altura: {mascote.height} \n" +
            $"Peso: {mascote.weight} \n" +
            $"Habilidades:\n");

        foreach (var ability in mascote.abilities)
        {
            Console.WriteLine($"\t{ability.ability.name}");
        }
    }

    public bool AdotarMascote(Mascote mascote)
    {
        Console.WriteLine($"Deseja adotar {mascote.name}?\n1 - SIM\n2 - NÃO");
        string resposta = Console.ReadLine();

        if (int.TryParse(resposta, out int n))
        {
            if (n == 1)
            {
                Console.WriteLine($"{mascote.name} adicionado a coleção.");
                return true;
            }
        }
        return false;
    }

    public void PrintPokeList(Mascote pokelist)
    {
        Console.WriteLine($"\n\n -------------------- ADOTAR UM MASCOTE --------------------");
        Console.WriteLine("Lista de Pokemons:");
        for (int i = 0; i < pokelist.results.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {pokelist.results[i].name}");
        }
    }





    public Status MenuColecao()
    {
        Console.WriteLine($"\n\n -------------------- MEUS MASCOTES --------------------");

        Console.WriteLine("Opção inválida.");
        return Status.MENU;
    }
    public Status MenuSair()
    {
        Console.WriteLine($"\n\n -------------------- FIM DO JOGO --------------------");

        Console.WriteLine("Opção inválida.");
        return Status.MENU;
    }

}


/* Você irá construir uma classe que será responsável pela exibição de todas as mensagens e leitura da resposta do usuário.
 * Esta classe deve:
 *  Dar boas vindas ao usuário, ler o nome da pessoa e dados que você achar relevante
 *  Exibir um menu que possibilite: 
 *      "Adoção de mascotes", 
 *      "Ver mascotes adotados" e 
 *      "Sair do Jogo"
 */

