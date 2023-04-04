using AutoMapper;
using RestSharp;
using System.Text.Json;

public class TamagotchiController {

    private Player Jogador { get; set; }
    private List<Mascot> Colecao { get; set; }

    private TamagotchiView View { get; set; }

    //private MascoteMapping MascoteMapping { get; set; }

    private string pokeApiUrl = "https://pokeapi.co";
    private string pokeListUrl = "/api/v2/pokemon/";

    public TamagotchiController() {
        this.Jogador = new Player();
        this.Colecao = new List<Mascot>();
        this.View = new TamagotchiView();
        //this.MascoteMapping = new MascoteMapping();


    }

    public void Jogar() {

        Menu opcaoUsuario = Menu.MAIN;
        string resposta = "";
        Jogador.Name = View.User.Name;
        Jogador.Age = View.User.Age;

        bool jogar = true;
        while (jogar) {

            switch (opcaoUsuario) {
                case Menu.MAIN:
                    resposta = View.MainMenu();

                    if (int.TryParse(resposta, out int n)) {
                        switch (n) {
                            case 1:
                                opcaoUsuario = Menu.ADOPT;
                                break;
                            case 2:
                                opcaoUsuario = Menu.COLECTION;
                                break;
                            case 3:
                                opcaoUsuario = Menu.EXIT;
                                break;
                            default:
                                opcaoUsuario = Menu.MAIN;
                                break;
                        }
                    }
                    else {
                        View.InvalidOption();
                        opcaoUsuario = Menu.MAIN;
                    }
                    break;

                case Menu.ADOPT:

                    resposta = View.BeginAdoption(GetPokeList());

                    if (int.TryParse(resposta, out n)) {
                        if (n > 0 && n < 1010) {
                            Pokemon? pokemon = GetPokemonDetails(n);
                            View.PrintPokemonDetails(pokemon);

                            string adotar = View.AdoptAdoption(pokemon);

                            if (int.TryParse(adotar, out n)) {
                                if (n == 1) {

                                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Pokemon, Mascot>());
                                    var mapper = new Mapper(config);

                                    var mascot = mapper.Map<Mascot>(pokemon);

                                    Colecao.Add(mascot);
                                    View.Adopted(pokemon);
                                    opcaoUsuario = Menu.MAIN;
                                }
                                if (n == 0) {
                                    opcaoUsuario = Menu.MAIN;
                                }
                            }
                            else {
                                View.InvalidOption();
                                opcaoUsuario = Menu.MAIN;
                            }
                            //opcaoUsuario = Menu.MAIN;
                        }
                    }
                    else {
                        View.InvalidOption();
                        opcaoUsuario = Menu.MAIN;
                    }


                    break;

                case Menu.COLECTION:

                    resposta = View.ColectionMenu(Colecao);

                    if (int.TryParse(resposta, out n)) {
                        if (n > 0 && n <= Colecao.Count) {

                            bool selectedMascote = true;
                            while (selectedMascote) {
                                resposta = View.MenuSelectedMascot(Colecao[n - 1]);
                                if (int.TryParse(resposta, out int l)) {
                                    if (l == 1) {
                                        View.PrintMascotStatus(Colecao[n - 1]);
                                    }
                                    if (l == 2) {
                                        PlayMascot(n);
                                    }
                                    if (l == 3) {
                                        FeedMascot(n);
                                    }
                                    if (l == 0) {
                                        opcaoUsuario = Menu.COLECTION;
                                        selectedMascote = false;
                                    }
                                }
                            }
                        }
                        else { opcaoUsuario = Menu.MAIN; }
                    }

                    break;

                case Menu.EXIT:
                    View.Exit();
                    jogar = false;
                    break;
            }
        }
    }

    public void PlayMascot(int n) {
        View.PlayMascot(Colecao[n - 1]);
        Colecao[n - 1].Humor += 2;
        Colecao[n - 1].Food -= 1;
        View.PrintMascotStatus(Colecao[n - 1]);
    }
    public void FeedMascot(int n) {
        View.FeedMascot(Colecao[n - 1]);
        Colecao[n - 1].Humor += 1;
        Colecao[n - 1].Food += 4;
        View.PrintMascotStatus(Colecao[n - 1]);
    }



    Pokemon? GetPokemonDetails(int? n) {
        var pokeJson = GetJson(pokeApiUrl, $"{pokeListUrl}{n}");
        Pokemon? pokemon = JsonSerializer.Deserialize<Pokemon>(pokeJson.Content);
        return pokemon;
    }

    PokeList? GetPokeList() {
        RestResponse response = GetJson(pokeApiUrl, pokeListUrl);
        var pokelist = JsonSerializer.Deserialize<PokeList>(response.Content);

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
