using AutoMapper;
using RestSharp;
using System.Text.Json;
using TamagotchiAPI.Service;

public class TamagotchiController {

    private Player Jogador { get; set; }
    private List<Mascot> Colecao { get; set; }

    private TamagotchiView View { get; set; }

    private string pokeApiUrl = "https://pokeapi.co";
    private string pokeListUrl = "/api/v2/pokemon/";

    private bool ApiConnectionSucess = false;

    public TamagotchiController() {
        this.Jogador = new Player();
        this.Colecao = new List<Mascot>();
        this.View = new TamagotchiView();
    }

    public void Jogar() {

        Menu opcaoUsuario = Menu.MAIN;

        while (true) {
            string name = View.GetUserName();
            if(name == "") {
                View.ErrorLog("Insira um nome.");
            }
            else {
                Jogador.Name = name;
                break;
            }
        }

        while (true) {
            string idadeStr = View.GetUserAge();

            if(int.TryParse(idadeStr, out int age)) {
                Jogador.Age = age;
                break;
            }
            else {
                View.ErrorLog("Insira um número inteiro.");
            }
        }
        View.User = (Jogador.Name, Jogador.Age);

        string resposta = "";
        bool jogar = true;
        while (jogar) {

            switch (opcaoUsuario) {
                case Menu.MAIN:
                    resposta = View.MainMenu();

                    if (int.TryParse(resposta, out int selectedMascot)) {
                        switch (selectedMascot) {
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
                        View.ErrorOption();
                        opcaoUsuario = Menu.MAIN;
                    }
                    break;

                case Menu.ADOPT:

                    PokeList? pokeList = GetPokeList();
                    if (ApiConnectionSucess) {
                        View.BeginAdoption(pokeList);
                        bool Adopted = false;
                        while (!Adopted) {
                            resposta = View.SelectAdoption();

                            if (int.TryParse(resposta, out selectedMascot)) {
                                if (selectedMascot > 0 && selectedMascot < 1010) {
                                    
                                    Pokemon? pokemon = GetPokemonDetails(selectedMascot);

                                    if (ApiConnectionSucess) {

                                        View.PrintPokemonDetails(pokemon);

                                        bool isAdopting = true;
                                        while (isAdopting) {
                                            string adotar = View.AdoptAdoption(pokemon);
                                            if (int.TryParse(adotar, out selectedMascot)) {

                                                switch (selectedMascot) {
                                                    case 0:
                                                        Adopted = true;
                                                        isAdopting = false;
                                                        opcaoUsuario = Menu.MAIN;
                                                        break;

                                                    case 1:
                                                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Pokemon, Mascot>());
                                                        var mapper = new Mapper(config);

                                                        var mascot = mapper.Map<Mascot>(pokemon);

                                                        Colecao.Add(mascot);
                                                        View.Adopted(pokemon);
                                                        Adopted = true;
                                                        isAdopting = false;
                                                        opcaoUsuario = Menu.MAIN;
                                                        break;

                                                    case 2:
                                                        Adopted = true;
                                                        isAdopting = false;
                                                        break;

                                                    default:
                                                        View.ErrorOption();
                                                        break;
                                                }

                                            }
                                            else {
                                                View.ErrorOption();
                                            }
                                        }

                                    }
                                }
                                else {
                                    View.ErrorOption();
                                }
                            }
                            else {
                                View.ErrorOption();
                            }
                        }
                    }
                    else {
                        opcaoUsuario = Menu.MAIN;
                    }
                    break;

                case Menu.COLECTION:

                    resposta = View.ColectionMenu(Colecao);

                    if (int.TryParse(resposta, out selectedMascot)) {
                        if (selectedMascot > 0 && selectedMascot <= Colecao.Count) {

                            bool notSelected = true;
                            while (notSelected) {
                                resposta = View.MenuSelectedMascot(Colecao[selectedMascot - 1]);
                                if (int.TryParse(resposta, out int action)) {

                                    switch (action) {
                                        case 0:
                                            opcaoUsuario = Menu.COLECTION;
                                            notSelected = false;
                                            break;
                                        case 1:
                                            View.PrintMascotStatus(Colecao[selectedMascot - 1]);
                                            break;
                                        case 2: 
                                            PlayMascot(selectedMascot);
                                            break;
                                        case 3:
                                            FeedMascot(selectedMascot);
                                            break;
                                        default:
                                            View.ErrorOption();
                                            break;
                                    }
                                }
                                else {
                                    View.ErrorOption();
                                }
                            }
                        }
                        else {
                            if(selectedMascot == 0) {
                                opcaoUsuario = Menu.MAIN;
                            }
                            else {  
                                View.ErrorOption(); 
                            }
                        }
                    }
                    else {
                        View.ErrorOption();
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
        var result = GetPokemonDetailsAPI(n);
        
        if (result.Item2 == null) {
            ApiConnectionSucess = true;
            return result.Item1;
        }
        else {
            View.ErrorLog(result.Item2);
            ApiConnectionSucess = false;
            return null;
        }
    } 

    (Pokemon?, string?) GetPokemonDetailsAPI(int? n) {
        var pokemon = GetJsonService.GetJsonAsync<Pokemon?>(pokeApiUrl, $"{pokeListUrl}{n}");
        return pokemon.Result;
    }

    PokeList? GetPokeList() {
        var result = GetPokeListAPI();
        
        if(result.Item2 == null ) {
            ApiConnectionSucess = true;
            return result.Item1;
        }
        else {
            View.ErrorLog(result.Item2);
            ApiConnectionSucess = false;
            return null;
        }
    }

    (PokeList?, string?) GetPokeListAPI() {
        var pokeList = GetJsonService.GetJsonAsync<PokeList>(pokeApiUrl, pokeListUrl);
        return pokeList.Result;
    }





}
