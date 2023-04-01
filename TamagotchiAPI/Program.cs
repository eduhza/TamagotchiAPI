using RestSharp;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

string pokeApiUrl = "https://pokeapi.co";
string pokeListUrl = "/api/v2/pokemon/";

RestResponse response = GetJson(pokeApiUrl, pokeListUrl);

var pokelist = JsonSerializer.Deserialize<Mascote>(response.Content);

Console.WriteLine("Lista de Pokemons:");
for (int i = 0; i < pokelist.results.Count; i++)
{
    Console.WriteLine($"{i}: {pokelist.results[i].name}");
}

Console.Write($"Selecione Pokémon desejado: ");
string selectedString = Console.ReadLine();

if (int.TryParse(selectedString, out int n))
{
    var pokemonhao = GetJson(pokeApiUrl, $"{pokeListUrl}{pokelist.results[n].name}");
    Mascote mascote = JsonSerializer.Deserialize<Mascote>(pokemonhao.Content);
    Console.Write($"\n" +
        $"#################### POKEMON SELECIONADO ####################\n" +
        $"Nome Pokemon: {mascote.name} \n" +
        $"Altura: {mascote.height} \n" +
        $"Peso: {mascote.weight} \n" +
        $"Habilidades:\n");

    foreach (var ability in mascote.abilities)
    {
        Console.WriteLine($"\t{ability.ability.name}" );
    }
}
else
{
    Console.WriteLine("Valor errado, você deve entrar com um número inteiro.");
}





RestResponse GetJson(string baseUrl, string resource) {
    var options = new RestClientOptions(baseUrl)
    {
        MaxTimeout = -1,
    };
    var client = new RestClient(options);
    var request = new RestRequest(resource, Method.Get);
    var body = @"";
    request.AddParameter("text/plain", body, ParameterType.RequestBody);
    RestResponse response = client.Execute(request);

    return response;
}



/*
 * https://pokeapi.co/api/v2
 * https://pokeapi.co/api/v2/pokemon/{id or name}/
 * Hoje eu te desafio a desenvolver uma funcionalidade onde o jogador acessa uma lista de opções de espécies de mascotes e visualizar suas características para facilitar sua escolha antes da adoção.
 *
 * 
 * https://viacep.com.br/ws/88063254/json/
 */



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