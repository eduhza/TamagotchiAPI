public class TamagotchiView {
    public Jogador Jogador { get; set; }
    public Status Opção { get; set; }
    public int? NumeroMascote { get; set; }
    public Mascote? Mascote { get; set; }
    public bool Adotado { get; set; }
    


    public TamagotchiView() {
        TamagotchiView view = this;
        Jogador = new Jogador();
        Adotado = false;
    }

    public void BoasVindas() {
        Console.WriteLine(@" 
     #######                                                                    
        #       ##    #    #    ##     ####    ####   #####   ####   #    #  # 
        #      #  #   ##  ##   #  #   #    #  #    #    #    #    #  #    #  # 
        #     #    #  # ## #  #    #  #       #    #    #    #       ######  # 
        #     ######  #    #  ######  #  ###  #    #    #    #       #    #  # 
        #     #    #  #    #  #    #  #    #  #    #    #    #    #  #    #  # 
        #     #    #  #    #  #    #   ####    ####     #     ####   #    #  #");

        Console.Write("\n\nQual é seu nome? ");
        Jogador.Nome = Console.ReadLine().ToUpper();

        Console.Write("\nQual a sua idade? ");

        if (int.TryParse(Console.ReadLine().ToUpper(), out int idade)) {
            Jogador.Idade = idade;
        }
        else {
            Jogador.Idade = null;
        }
    }

    public void MenuInicial() {
        Opção = Status.MENU;
        Console.WriteLine($"\n\n -------------------- MENU --------------------");
        Console.WriteLine(
            $"{Jogador.Nome} Escolha uma opção:\n " +
            $"1 - Adotar um mascote\n " +
            $"2 - Ver coleção\n " +
            $"3 - Sair");

        string resposta = Console.ReadLine();

        if(int.TryParse(resposta, out int n)) {
            switch(n) {
                case 1:
                    Opção = Status.ADOTAR;
                    break;
                case 2:
                    Opção = Status.COLECAO;
                    break;
                case 3:
                    Opção = Status.SAIR;
                    break;
                default:
                    Opção = Status.MENU;
                    break;
            }
        }
        else {
            Console.WriteLine("Opção inválida.");
            Opção = Status.MENU;
        }
    }

    public void MenuAdocao(string subMenu, Mascote? mascote) {
        Opção = Status.ADOTAR;
        switch (subMenu) {
            case "inicio":
                Console.WriteLine($"\n\n -------------------- ADOTAR UM MASCOTE --------------------");
                //return null;
                break;

            case "escolher":
                Console.Write("Escolha uma espécie entre 1 e 1010: ");
                string resposta = Console.ReadLine();

                if (int.TryParse(resposta, out int n)) {
                    if (n > 0 && n < 1010) {
                        NumeroMascote = n;
                    }
                }
                else {
                    Console.WriteLine("Opção inválida.");
                    NumeroMascote = null;
                }

                break;

            case "adotar":
                Adotado = false;
                Console.WriteLine(
                    $"Deseja adotar {mascote.name}?\n" +
                    $"1 - SIM\n" +
                    $"2 - NÃO\n" +
                    $"3 - Voltar ao menu inicial");

                string adotar = Console.ReadLine();

                if (int.TryParse(adotar, out int k)) {
                    if (k == 1) {
                        Console.WriteLine($"{mascote.name} adicionado a coleção.");
                        Opção = Status.MENU;
                        Adotado = true;
                    }
                    if (k == 3) {
                        Opção = Status.MENU;
                    }
                }
                break;
        }
    }
    public void PrintPokeName(Mascote pokelist)
    {
        Console.WriteLine("Lista de Pokemons:");
        for (int i = 0; i < pokelist.results.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {pokelist.results[i].name}");
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

    public void PrintColecao(List<Mascote> colecao) {
        Console.WriteLine("Sua coleção:");
        int i = 1;
        foreach (Mascote mascote in colecao)
        {
            Console.WriteLine($"Mascote {i}: {mascote.name}");
            i++;
        }
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

