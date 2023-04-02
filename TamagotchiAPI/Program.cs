using RestSharp;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

string pokeApiUrl = "https://pokeapi.co";
string pokeListUrl = "/api/v2/pokemon/";

MessageClass mc = new MessageClass();
Status option = Status.INICIO;
Jogador jogador = new Jogador();
List<Mascote> Colecao= new List<Mascote>();

bool playing = true;
option = Status.MENU;

while (playing)
{
    switch (option)
    {
        case Status.MENU:
            option = mc.MenuInicial();
            break;

        case Status.ADOTAR:
            
            GetPokeList(); //Mostra os primeiros 20 pokemons

            bool adotado = false;
            while (!adotado)
            {
                int? n = mc.MenuAdotar();

                if (n.HasValue)
                {
                    if (GetMascoteDetails((int)n))
                    {
                        adotado = !adotado;
                    }
                }
            }

            option = Status.MENU;
            break;

        case Status.COLECAO:
            mc.PrintColecao(Colecao);


            option = Status.MENU;
            break;

        case Status.SAIR:

            playing = false;
            break;
    }
}

bool GetMascoteDetails(int n)
{
    var pokemonhao = GetJson(pokeApiUrl, $"{pokeListUrl}{n}");
    Mascote? mascote = JsonSerializer.Deserialize<Mascote>(pokemonhao.Content);

    mc.PrintMascote(mascote);

    if (mc.AdotarMascote(mascote))
    {
        Colecao.Add(mascote);
        return true;
    }
    
    return false;
}

void GetPokeList()
{
    RestResponse response = GetJson(pokeApiUrl, pokeListUrl);
    var pokelist = JsonSerializer.Deserialize<Mascote>(response.Content);

    mc.PrintPokeList(pokelist);
}


//while (true) {
//    Console.Write($"Selecione Pokémon desejado: ");
//    string? selectedString = Console.ReadLine();
//    if (int.TryParse(selectedString, out int n) && n != 0) {
//        GetPokemon(n);
//        break;
//    }
//    else {
//        Console.WriteLine("Número inválido!");
//    }
//}

RestResponse GetJson(string baseUrl, string resource) {
    var options = new RestClientOptions(baseUrl) {
        MaxTimeout = -1,
    };
    var client = new RestClient(options);
    var request = new RestRequest(resource, Method.Get);
    var body = @"";
    request.AddParameter("text/plain", body, ParameterType.RequestBody);
    RestResponse response = client.Execute(request);

    return response;
}





//RestResponse responseCEP = GetJson("https://viacep.com.br/ws/88063254/json/", "");
//var Endereco = JsonSerializer.Deserialize<Endereco>(responseCEP.Content);
//public class Endereco
//{
//    public string cep { get; set; }
//    public string logradouro { get; set; }
//    public string complemento { get; set; }
//    public string bairro { get; set; }
//    public string localidade { get; set; }
//    public string uf { get; set; }
//}