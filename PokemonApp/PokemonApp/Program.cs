using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using PokemonApp.Models;
using PokemonApp.Util;
using System.Xml.Schema;
using System.Net;
using System.Net.PeerToPeer.Collaboration;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.CodeDom;
using System.Globalization;

namespace PokemonApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Pokemon> list = new List<Pokemon>();
            int lower = 1;
            int upper = 20;
            string condition;

            //Navigate through the different pages
            do
            {
                //Clear pokemon stored in the list and add new pokemon
                list.Clear();
                await Initialize(list, lower, upper);
                DisplayList(list);

                condition = Console.ReadLine().ToLower();

                //User decides to navigate to next pages
                if (condition == "n")
                {
                    lower += 20;
                    upper += 20;
                }

                //User decides to navigate to previous pages
                else if (condition == "p")
                {
                    //If not on the first page then decrease upper bound and lower bound counts
                    if(lower != 1)
                    {
                        lower -= 20;
                        upper -= 20;
                    }
                }

            }while (condition == "p" || condition == "n"); //Exit loop if user decides to stop navigating pages

            //Filtering by name
            if(condition == "g")
            {
                //Clear pokemon stored in the list and add new pokemon
                list.Clear();

                //Name to search
                Console.Write("Pokeman Name: ");
                String name = Console.ReadLine();

                //Formatting of name
                name.ToLower();
 

                await NameFilteredList(list, name);
                if (list.Count > 0)
                {
                    DisplayList(list);
                }

                else
                {
                    Console.WriteLine("No Pokemon fitting criteria found");
                }

                condition = Console.ReadLine().ToLower();
            }

            //Filtering via weight
            else if (condition == "f")
            {
                //Clear pokemon stored in the list and add new pokemon
                list.Clear();

                //Enter weight to search
                int weight;
                bool validInput = false;

                do
                {
                    Console.Write("Enter weight: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out weight))
                    {
                        // Conversion succeeded
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }
                } while (!validInput);

                Console.Clear();
                Console.WriteLine("Entered weight: " + weight);
                await WeightFilteredList(list, weight);

                if (list.Count > 0)
                {
                    DisplayList(list);
                }

                else
                {
                    Console.WriteLine("No Pokemon fitting criteria found");
                }

                condition = Console.ReadLine().ToLower();
            }

            //Selected a poki on the list
            if(condition == "x")
            {
                //Enter weight to search
                int num;
                bool validInput = false;
                int inList = -1;

                do
                {
                    Console.Write("Enter Pokimon Number: ");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out num))
                    {
                        //Check if card is in list
                        if(num >= lower && num <= upper)
                        {
                            validInput = true;
                            
                            for(int i = 0; i < 20; i++)
                            {
                                if(num == list[i].id)
                                {
                                    //get the array value
                                    inList = i;
                                }
                            }
                        }

                        //If it is just a valid card
                        else if (num >= 1)
                        {
                            validInput = true;
                        }

                        //Invalid card number
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a value between "+lower+" and "+upper);
                        }
                    }

                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                    }

                } while (!validInput);

                Console.Clear();
                Console.WriteLine("Entered Number: " + num);
                await PrintCard(num, inList, list); 

                condition = Console.ReadLine().ToLower();
            }
        }

        //List of pokemons from specified lower and upper bound
        public static async Task Initialize(List<Pokemon> list, int lower, int upper)
        {
            HttpClient client = new HttpClient();
            PokeClient poke = new PokeClient(client);

            for (int i = lower; i <= upper; i++)
            {
                list.Add(await poke.GetPokemon(i.ToString()));
            }
        }

        //List of pokemons from specified name
        public static async Task NameFilteredList(List<Pokemon> list, string name)
        {
            HttpClient client = new HttpClient();
            PokeClient poke = new PokeClient(client);
            Pokemon pokemon = new Pokemon();
            int count = 0;
            int pokiNum = 1;

            while (count < 1)
            {
                pokemon = await poke.GetPokemon(pokiNum.ToString());

                //If value contains word searched then add to list
                if ((pokemon.name).Contains(name))
                {
                    list.Add(pokemon);
                    count++;
                }
            }
        }

        //List of pokemons from specified weight
        public static async Task WeightFilteredList(List<Pokemon> list, int weight)
        {
            HttpClient client = new HttpClient();
            PokeClient poke = new PokeClient(client);
            Pokemon pokemon = new Pokemon();
            int count = 0;
            int pokiNum = 1;

            while (count <= 10)
            {
                pokemon = await poke.GetPokemon(pokiNum.ToString());

                //If value contains word searched then add to list
                if (pokemon.weight == weight)
                {
                    list.Add(pokemon);
                    count++;
                }
            }
        }

        //Diplay list of pokemon
        public static void DisplayList(List<Pokemon>list)
        {
            Console.Clear();

            string str = String.Format("{0, -10} {1, -15} {2, -15} {3, -15}", "Number", "Name", "Weight", "Height");
            Console.WriteLine(str+"\n");

            for (int i = 0; i < list.Count; i++)
            {
                str = String.Format("{0, -10} {1, -15} {2, -15} {3, -15}", list[i].id, list[i].name, list[i].weight, list[i].height);
                Console.WriteLine(str);
            }

            //Footer with commands
            Console.WriteLine("\n" + string.Concat(Enumerable.Repeat("*", 80)));
            Console.WriteLine(string.Format("{0,-13} {1, 25} {2, 25} {3, 25}", "Next Page - N", "Previous Page - P", "Filter by Weight - F", "Filter by Name - G" ));
            Console.WriteLine("Select Poki - X");
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 80)));
        }

        //Print detailed card for selected Pokemon
        public static async Task PrintCard(int number, int inList, List<Pokemon> list)
        {
            HttpClient client = new HttpClient();
            PokeClient poke = new PokeClient(client);

            Console.Clear();
            Pokemon pokemon = new Pokemon();

            //The pokemon is already in the list
            if (inList > -1)
            {
                pokemon = list[inList];
            }

            else
            {
                pokemon = await poke.GetPokemon(number.ToString());
            }

            //Header
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 80)));
            Console.WriteLine(string.Format("{0," + ((80 + 7) / 2) + "}", pokemon.name));
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 80)));

            //Body
            Console.WriteLine("\nSTATS: ");
            for(int i = 0; i < pokemon.stats.Count; i++)
            {
                Console.WriteLine(pokemon.stats[i].stat.name+": "+ pokemon.stats[i].base_stat);

                //Create a powerbar
                int bar = (int)Math.Round(((double)pokemon.stats[i].base_stat / 2.00),0);
                Console.WriteLine(string.Concat(Enumerable.Repeat("█", bar)));

                //Create a space
                Console.WriteLine(" ");
            }

            //Footer
            //Header
            Console.WriteLine(string.Concat(Enumerable.Repeat("*", 80)));
        }

    }
}
