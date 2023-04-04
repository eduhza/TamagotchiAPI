using RestSharp;
using System.Text.Json;

public class TamagotchiController {

    private Player Jogador { get; set; }
    private List<Mascot> Colecao { get; set; }

    private TamagotchiView View { get; set; }

    //private Menu OpcaoUsuario { get; set; }

    private string pokeApiUrl = "https://pokeapi.co";
    private string pokeListUrl = "/api/v2/pokemon/";

    public TamagotchiController() {
        this.Jogador = new Player();
        this.Colecao = new List<Mascot>();
        this.View = new TamagotchiView();
        //this.OpcaoUsuario = new Menu();
    }

    public void Jogar() {

        Menu opcaoUsuario = Menu.MAIN;
        string resposta = "";
        View.Welcome();
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
                            Mascot? mascot = GetMascoteDetails(n);
                            View.PrintMascote(mascot);

                            string adotar = View.AdoptAdoption(mascot);

                            if (int.TryParse(adotar, out n)) {
                                if (n == 1) {
                                    Colecao.Add(mascot);
                                    View.Adopted(mascot);
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
                                        View.PlayMascot(Colecao[n - 1]);
                                        Colecao[n - 1].Humor += 2;
                                        Colecao[n - 1].Food -= 1;
                                        View.PrintMascotStatus(Colecao[n - 1]);
                                    }
                                    if (l == 3) {
                                        View.FeedMascot(Colecao[n - 1]);
                                        Colecao[n - 1].Humor += 1;
                                        Colecao[n - 1].Food += 4;
                                        View.PrintMascotStatus(Colecao[n - 1]);
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

    public void PlayWithMascot() {

    }



    Mascot? GetMascoteDetails(int? n) {
        var pokemonhao = GetJson(pokeApiUrl, $"{pokeListUrl}{n}");
        Mascot? mascote = JsonSerializer.Deserialize<Mascot>(pokemonhao.Content);
        return mascote;
    }

    Mascot? GetPokeList() {
        RestResponse response = GetJson(pokeApiUrl, pokeListUrl);
        var pokelist = JsonSerializer.Deserialize<Mascot>(response.Content);

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
