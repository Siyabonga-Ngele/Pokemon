using PokemonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonApp.Util
{
    public class PokeClient
    {
        public HttpClient httpClient;

        public PokeClient(HttpClient client)
        {
            httpClient = client;    
        }

        public async Task<Pokemon> GetPokemon(string id)
        {
            var response = await this.httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
            Pokemon pokemon = await response.Content.ReadAsAsync<Pokemon>();

            return pokemon;
        }
    }
}
