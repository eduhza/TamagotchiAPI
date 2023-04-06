using AutoMapper;
using RestSharp;
using System.Text.Json;
using TamagotchiAPI.Service;

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
                        View.InvalidOption();
                        opcaoUsuario = Menu.MAIN;
                    }
                    break;

                case Menu.ADOPT:

                    resposta = View.BeginAdoption(GetPokeList());

                    bool Adopted = false;
                    while(!Adopted) {
                        if (int.TryParse(resposta, out selectedMascot)) {
                            if (selectedMascot > 0 && selectedMascot < 1010) {
                                Pokemon? pokemon = GetPokemonDetails(selectedMascot);
                                View.PrintPokemonDetails(pokemon);

                                string adotar = View.AdoptAdoption(pokemon);

                                if (int.TryParse(adotar, out selectedMascot)) {
                                    if (selectedMascot == 1) {

                                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Pokemon, Mascot>());
                                        var mapper = new Mapper(config);

                                        var mascot = mapper.Map<Mascot>(pokemon);

                                        Colecao.Add(mascot);
                                        View.Adopted(pokemon);
                                        Adopted = true;
                                        opcaoUsuario = Menu.MAIN;
                                    }
                                    else if (selectedMascot == 0) {
                                        opcaoUsuario = Menu.MAIN;
                                    }
                                    else {
                                        View.ErrorOption();
                                    }
                                }
                                else {
                                    View.ErrorOption();
                                    //opcaoUsuario = Menu.MAIN;
                                }
                                //opcaoUsuario = Menu.MAIN;
                            }
                        }
                        else {
                            View.ErrorOption();
                            //opcaoUsuario = Menu.ADOPT;
                        }
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
        Task<Pokemon?> pokemon = GetJsonService.GetJsonAsync<Pokemon?>(pokeApiUrl, $"{pokeListUrl}{n}");
        return pokemon.Result;
    }

    PokeList GetPokeList() {
        Task<PokeList?> pokeList = GetJsonService.GetJsonAsync<PokeList?>(pokeApiUrl, pokeListUrl);
        return pokeList.Result;
    }





}
