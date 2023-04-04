public class TamagotchiView {
    public Player User { get; set; }
    public int? MascotNumber { get; set; }
    public Mascot? Mascot { get; set; }
    
    public TamagotchiView() {
        TamagotchiView view = this;
        User = new Player();
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
        User.Name = Console.ReadLine().ToUpper();

        Console.Write("\nQual a sua idade? ");
        User.Age = int.TryParse(Console.ReadLine().ToUpper(), out int age) 
            ? age 
            : null;
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

    public string BeginAdoption(Mascot pokelist) {
        Console.WriteLine($"\n -------------------- ADOTAR UM MASCOTE --------------------");
        PrintPokeName(pokelist);
        Console.Write("ESCOLHA UMA ESPÉCIE ENTRE 1 e 1009: ".ToUpper());
        return Console.ReadLine();
    }

    public string AdoptAdoption(Mascot mascot) {
        Console.WriteLine(
            $"\n DESEJA ADOTAR {mascot.name}?\n" +
            $"    1 - SIM\n" +
            $"    2 - NÃO\n" +
            $"    0 - Voltar ao menu inicial");

        return Console.ReadLine();
    }

    public void Adopted(Mascot mascot) {
        Console.WriteLine($"{mascot.name} ADICIONADO A COLEÇÃO!");
    }

    public void PrintPokeName(Mascot pokelist)
    {
        Console.WriteLine("LISTA DE POKEMONS:");
        for (int i = 0; i < pokelist.results.Count; i++)
        {
            Console.WriteLine($"    {i + 1}: {pokelist.results[i].name}");
        }
    }

    public void PrintMascote(Mascot mascote)
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
            Console.WriteLine($"    {i}: {mascote.name}");
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
        Console.WriteLine($"\n -------------------- {mascot.name} --------------------");
        Console.WriteLine( $"    Altura: {mascot.height}\n    Peso: {mascot.weight}");

        Console.WriteLine($"\n{User.Name} VOCÊ DESEJA:\n" +
            $"    1 - Ver status do {mascot.name}\n" +
            $"    2 - Brincar com {mascot.name}\n" +
            $"    3 - Alimentar {mascot.name}\n" +
            $"    0 - Interagir com outro mascote");
        return Console.ReadLine();
    }

    public void PrintMascotStatus(Mascot mascot) {

        Console.WriteLine($"\nSTATUS DE {mascot.name}:");

        if (mascot.Food >= 7) {
            Console.WriteLine($"    {mascot.name} está alimentado.");
        }
        else if (mascot.Food >= 5) {
            Console.WriteLine($"    {mascot.name} está com um pouco de fome.");
        }
        else if (mascot.Food > 2 ) {
            Console.WriteLine($"    {mascot.name} está faminto!");
        }
        else {
            Console.WriteLine($"    {mascot.name} vai morrer de fome!");
        }

        if (mascot.Humor >= 7) {
            Console.WriteLine($"    {mascot.name} está feliz.");
        }
        else if (mascot.Humor >= 5) {
            Console.WriteLine($"    {mascot.name} está se sentindo um pouco só.");
        }
        else if (mascot.Humor > 2) {
            Console.WriteLine($"    {mascot.name} está triste!");
        }
        else {
            Console.WriteLine($"    {mascot.name} vai entrar em depressão!");
        }
    }

    public Mascot PlayMascot(Mascot mascot) {

        Console.WriteLine($"{mascot.name} está brincando");
        return mascot;
    }
    public Mascot FeedMascot(Mascot mascot) {

        Console.WriteLine($"{mascot.name} está comendo");
        return mascot;
    }

    public void Exit() {
        Console.WriteLine("BYE!");
    }



    public void InvalidOption() {
        Console.WriteLine("OPÇÃO INVÁLIDA, RETORNANDO AO MENU INICIAL");
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

