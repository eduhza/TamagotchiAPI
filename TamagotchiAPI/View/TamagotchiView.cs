using System.Collections.Specialized;

public class TamagotchiView {
    public (string Name, int? Age) User { get; set; }
    public int? MascotNumber { get; set; }
    public Pokemon? Mascot { get; set; }
    
    public TamagotchiView() {
        TamagotchiView view = this;
        User = ("", null);
        Welcome();
    }

    public void Welcome() {
        Console.WriteLine(@" 
     #######                                                                    
        #       ##    #    #    ##     ####    ####   #####   ####   #    #  # 
        #      #  #   ##  ##   #  #   #    #  #    #    #    #    #  #    #  # 
        #     #    #  # ## #  #    #  #       #    #    #    #       ######  # 
        #     ######  #    #  ######  #  ###  #    #    #    #       #    #  # 
        #     #    #  #    #  #    #  #    #  #    #    #    #    #  #    #  # 
        #     #    #  #    #  #    #   ####    ####     #     ####   #    #  #");

        Console.Write("\n\nQual o seu nome? ");
        string nome = Console.ReadLine().ToUpper();

        Console.Write("\nQual a sua idade? ");
        int? idade = int.TryParse(Console.ReadLine().ToUpper(), out int age) 
            ? age 
            : null;

        User = (nome, idade);
    }

    public string MainMenu() {
        Console.WriteLine($"\n -------------------- MENU --------------------");
        Console.WriteLine(
            $"{User.Name} VOCÊ DESEJA:\n " +
            $"    1 - Adotar um mascote\n " +
            $"    2 - Ver coleção\n " +
            $"    3 - Sair");
        return Console.ReadLine();
    }

    public string BeginAdoption(PokeList pokelist) {
        Console.WriteLine($"\n -------------------- ADOTAR UM MASCOTE --------------------");
        PrintPokeName(pokelist);
        Console.Write("ESCOLHA UMA ESPÉCIE ENTRE 1 e 1009: ".ToUpper());
        return Console.ReadLine();
    }

    public string AdoptAdoption(Pokemon mascot) {
        Console.WriteLine(
            $"\n DESEJA ADOTAR {mascot.name}?\n" +
            $"    1 - SIM\n" +
            $"    2 - NÃO\n" +
            $"    0 - Voltar ao menu inicial");

        return Console.ReadLine();
    }

    public void Adopted(Pokemon mascot) {
        Console.WriteLine($"{mascot.name} ADICIONADO A COLEÇÃO!");
    }

    public void PrintPokeName(PokeList pokelist)
    {
        Console.WriteLine("LISTA DE POKEMONS:");
        for (int i = 0; i < pokelist.results.Count; i++)
        {
            Console.WriteLine($"    {i + 1}: {pokelist.results[i].name}");
        }
    }

    public void PrintPokemonDetails(Pokemon mascote)
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

    public void PrintColecao(List<Mascot> colecao) {
        int i = 1;
        foreach (Mascot mascote in colecao)
        {
            Console.WriteLine($"    {i}: {mascote.Name}");
            i++;
        }
    }
   
    public string ColectionMenu(List<Mascot> colecao) {
        Console.WriteLine($"\nQUAL MASCOTE DESEJA INTERAGIR?");
        PrintColecao(colecao);
        Console.WriteLine($"    0: Voltar ao menu inicial");

        return Console.ReadLine();
    }

    public string MenuSelectedMascot(Mascot mascot) {

        Console.WriteLine($"\n{User.Name} VOCÊ DESEJA:\n" +
            $"    1 - Ver status do {mascot.Name}\n" +
            $"    2 - Brincar com {mascot.Name}\n" +
            $"    3 - Alimentar {mascot.Name}\n" +
            $"    0 - Interagir com outro mascote");

        Console.WriteLine($"\n -------------------- {mascot.Name} --------------------");
        Console.WriteLine($"    Altura: {mascot.Height}\n    Peso: {mascot.Weight}\n    Idade: {DateTime.Now.Subtract(mascot.BirthDate)}");
        return Console.ReadLine();
    }

    public void PrintMascotStatus(Mascot mascot) {

        Console.WriteLine($"\nSTATUS DE {mascot.Name}:");

        if (mascot.Food >= 7) {
            Console.WriteLine($"    {mascot.Name} está alimentado.");
        }
        else if (mascot.Food >= 5) {
            Console.WriteLine($"    {mascot.Name} está com um pouco de fome.");
        }
        else if (mascot.Food > 2) {
            Console.WriteLine($"    {mascot.Name} está faminto!");
        }
        else {
            Console.WriteLine($"    {mascot.Name} vai morrer de fome!");
        }

        if (mascot.Humor >= 7) {
            Console.WriteLine($"    {mascot.Name} está feliz.");
        }
        else if (mascot.Humor >= 5) {
            Console.WriteLine($"    {mascot.Name} está se sentindo um pouco só.");
        }
        else if (mascot.Humor > 2) {
            Console.WriteLine($"    {mascot.Name} está triste!");
        }
        else {
            Console.WriteLine($"    {mascot.Name} vai entrar em depressão!");
        }
    }

    public Mascot PlayMascot(Mascot mascot) {

        Console.WriteLine($"{mascot.Name} está brincando");
        return mascot;
    }
    public Mascot FeedMascot(Mascot mascot) {

        Console.WriteLine($"{mascot.Name} está comendo");
        return mascot;
    }

    public void Exit() {
        Console.WriteLine("BYE!");
    }

    public void ErrorOption() {
        Console.WriteLine("OPÇÃO INVÁLIDA, TENTE NOVAMENTE.");
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

