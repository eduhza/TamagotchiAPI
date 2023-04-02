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

            mc.MenuAdocao(1);
            GetPokeList(); //Mostra os primeiros 20 pokemons

            bool adotado = false;
            while (!adotado)
            {
                //int? n = mc.MenuAdotar();
                int? n = mc.MenuAdocao(2);

                if (n.HasValue)
                {
                    if (GetMascoteDetails((int)n))
                    {
                        adotado = !adotado;
                    }
                }
                option = mc.status;
                if (option == Status.MENU)
                {
                    break;
                }
            }

            //option = Status.MENU;
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

    mc.PrintListName(pokelist);
}

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