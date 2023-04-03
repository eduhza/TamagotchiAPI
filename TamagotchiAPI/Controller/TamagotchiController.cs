using RestSharp;
using System.Text.Json;

public class TamagotchiController {

    private Jogador Jogador { get; set; }
    private List<Mascote> Colecao { get; set; }

    private TamagotchiView View { get; set; }

    private Status OpcaoUsuario { get; set; }

    private string pokeApiUrl = "https://pokeapi.co";
    private string pokeListUrl = "/api/v2/pokemon/";

    public TamagotchiController() {
        this.Jogador = new Jogador();
        this.Colecao = new List<Mascote>();
        this.View = new TamagotchiView();
        this.OpcaoUsuario = new Status();
    }

    public void Jogar() {

        OpcaoUsuario = Status.MENU;
        View.BoasVindas();
        Jogador.Nome = View.Jogador.Nome;
        Jogador.Idade = View.Jogador.Idade;

        bool jogar = true;
        while (jogar) {

            switch (OpcaoUsuario) {
                case Status.MENU:
                    View.MenuInicial();
                    OpcaoUsuario = View.Opção;
                    break;

                case Status.ADOTAR:
                    View.MenuAdocao("inicio", null);
                    View.PrintPokeName(GetPokeList());
                    View.MenuAdocao("escolher", null);
                    int? numeroMascote = View.NumeroMascote;
                    if (numeroMascote.HasValue) {
                        Mascote? mascote =  GetMascoteDetails(numeroMascote);
                        View.PrintMascote(mascote);
                        View.MenuAdocao("adotar", mascote);

                        if (View.Adotado) { Colecao.Add(mascote); }
                       
                        OpcaoUsuario = View.Opção;
                    }

                    break;

                case Status.COLECAO:
                    View.PrintColecao(Colecao);
                    OpcaoUsuario = Status.MENU;
                    break;

                case Status.SAIR:
                    jogar = false;
                    break;
            }


        }


    }



    Mascote? GetMascoteDetails(int? n) {
        var pokemonhao = GetJson(pokeApiUrl, $"{pokeListUrl}{n}");
        Mascote? mascote = JsonSerializer.Deserialize<Mascote>(pokemonhao.Content);
        return mascote;
    }

    Mascote? GetPokeList() {
        RestResponse response = GetJson(pokeApiUrl, pokeListUrl);
        var pokelist = JsonSerializer.Deserialize<Mascote>(response.Content);

        return pokelist;
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
}
