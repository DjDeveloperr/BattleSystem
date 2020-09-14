using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BattleSystem
{
    public class Test
    {
        static void Main(string[] args) {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            PokemonSet pset = new PokemonSet() {
                name = "Charmander",
                nature = "Timid",
                ability = "Solar Power",
                ivs = new StatsTable(new int[6]{ 31, 31, 31, 31, 31, 31 }),
                evs = new StatsTable(),
                shiny = true,
                hp = 100,
                maxhp = 100,
                gigantamax = false,
                pokeball = "Great Ball",
                moves = new MoveSetSlot[]{
                    new MoveSetSlot("Ember", 0, 10, 10),
                    new MoveSetSlot("Scratch", 1, 10, 10),
                },
            };

            PokemonSet pset2 = new PokemonSet() {
                name = "Squirtle",
                nature = "Bold",
                ability = "Rain Dish",
                ivs = new StatsTable(new int[6]{ 31, 31, 31, 31, 31, 31 }),
                evs = new StatsTable(),
                shiny = false,
                hp = 100,
                maxhp = 100,
                gigantamax = false,
                pokeball = "Master Ball",
                moves = new MoveSetSlot[]{
                    new MoveSetSlot("Tail Whip", 0, 10, 10),
                    new MoveSetSlot("Water Gun", 1, 10, 10),
                },
            };

            ETest(pset, pset2);

            // Console.WriteLine(JSON.Stringify(PokeDex.Data.GetPokemon("Pikachu")));
            // Console.WriteLine(JSON.Stringify(PokeDex.Data.GetAbility("Contrary")));
            // Console.WriteLine(JSON.Stringify(PokeDex.Data.GetMove("Leaf Storm")));
            // Console.WriteLine(JSON.Stringify(PokeDex.Data.GetItem("Blazikenite")));
            await Task.Delay(-1);
        }

        public static void ETest(PokemonSet pset, PokemonSet pset2) {
            PlayerOptions p1 = new PlayerOptions() {
                name = "Player 1",
                team = new PokemonSet[]{
                    pset
                },
            };

            PlayerOptions p2 = new PlayerOptions() {
                name = "Player 2",
                team = new PokemonSet[]{
                    pset2
                },
            };
            Battle battle = new Battle(p1, p2, new BattleSettings() {
                mega = true,
                zmove = true,
                dynamax = true,
                items = true,
            });

            battle.OnText += (string text, Side side) => {
                Console.WriteLine((side != null ? ("["+(side.num+1)+"] ") : "") + text);
            };

            battle.OnChoice += (Side side, string type, bool forced) => {
                Console.Write($"[{side.num+1}] >>> ");
                string[] choice = Console.ReadLine().Split(" ");
                if(choice[0].ToLower() == "m") {
                    int slot = Int32.Parse(choice[1]);
                    Console.WriteLine("Move " + slot);
                    bool c = battle.ChooseMove(side.active, slot);
                    Console.WriteLine("Chosen: "+c);
                } else if(choice[0].ToLower() == "s") {
                    int slot = Int32.Parse(choice[1]);
                    Console.WriteLine("Switch " + slot);
                    bool c = battle.ChooseSwitch(side.team[slot-1]);
                    Console.WriteLine("Chosen: "+c);
                }
            };

            battle.Start();
        }
    }
}
