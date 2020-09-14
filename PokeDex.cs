using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BattleSystem.PokeDex
{
    public class GenderRatio {
        public float M = 0f;
        public float F = 0f;
    }

    public class AbilityEntry {
        public string name = "";
        public int num = -1;
        public float rating = -1;
        public string shortDesc = null;
        public string desc = null;
    }

    public class PokeDexEntry {
        public int id = 0;
        public string name = "";
        public string[] types = {};
        public GenderRatio genderRatio = new GenderRatio();
        public StatsTable baseStats = new StatsTable();
        public Dictionary<string, string> abilities = new Dictionary<string, string>();
        public float height = 0f;
        public float weight = 0f;
        public string color = "";
        public string[] evos = {};
        public string[] eggGroups = {};
        public string prevo = null;
        public int evoLevel = -1;
        public string[] otherFormes = {};
        public string[] formeOrder = {};
        public string canGigantamax = null;
        public string baseSpecies = null;
        public string forme = null;
        public string requiredItem = null;
        public string changesFrom= null;
        public string evoType = null;
        public bool noGender = false;
        public int gen = 0;
        public string evoItem = null;
        public string evoCondition = null;
        public bool canHatch = false;
        public string evoMove = null;
        public string baseForme = null;
        public string[] cosmeticFormes = {};
        public int maxHP = -1;
        public string requiredAbility = null;
        public string battleOnly = null;
        public string requiredMove = null;
        public string[] requiredItems = {};
        public bool cannotDynamax = false;
    }

    public class MoveSecondary {
        public int chance = 0;
        public BoostsTable boosts;
    }

    public class MoveEntry {
        public int num = -1;
        public string name = "";
        public bool accuracy = false;
        public int basePower = 0;
        public string category = "";
        public string desc = null;
        public string shortDesc = null;
        public int pp = 0;
        public int priority = 0;
        public string isZ = null;
        public int critRatio = 0;
        public MoveSecondary secondary = null;
        public string target = null;
        public string type = null;
        public string isMax = null;
    }

    public class ItemFling {
        public int basePower = 0;
    }

    public class ItemNaturalGift {
        public int basePower = 0;
        public string type = "";
    }

    public class ItemEntry {
        public string name = "";
        public string desc = null;
        public int num = 0;
        public string megaEvolves = null;
        public string megaStone = null;
        public int gen = 0;
        public string[] itemUser = {};
        public string zMove = null;
        public string zMoveFrom = null;
        public string zMoveType = null;
        public bool isPokeball = false;
        public ItemFling fling;
        public ItemNaturalGift naturalGift;
        public bool isBerry = false;
        public bool isChoice = false;
        public string isNonstandard = null;
        public string forcedForme = null;
        public BoostsTable boosts;
    }

    public class TypeEntry {
        public Dictionary<string, int> damageTaken = new Dictionary<string, int>();
        public StatsTable HPivs = new StatsTable();
    }

    public class NatureEntry {
        public string name = "";
        public string plus = null;
        public string minus = null;
    }

    public class Data {
        static Dictionary<string, PokeDexEntry> pokes = JsonConvert.DeserializeObject<Dictionary<string, PokeDexEntry>>(File.ReadAllText("./data/pokedex.json"));
        static Dictionary<string, AbilityEntry> abis = JsonConvert.DeserializeObject<Dictionary<string, AbilityEntry>>(File.ReadAllText("./data/abilities.json"));
        static Dictionary<string, MoveEntry> moves = JsonConvert.DeserializeObject<Dictionary<string, MoveEntry>>(File.ReadAllText("./data/moves.json"));
        static Dictionary<string, ItemEntry> items = JsonConvert.DeserializeObject<Dictionary<string, ItemEntry>>(File.ReadAllText("./data/items.json"));
        static Dictionary<string, TypeEntry> types = JsonConvert.DeserializeObject<Dictionary<string, TypeEntry>>(File.ReadAllText("./data/types.json"));
        static Dictionary<string, NatureEntry> natures = JsonConvert.DeserializeObject<Dictionary<string, NatureEntry>>(File.ReadAllText("./data/natures.json"));

        public static T Get<T>(Dictionary<string, T> data, string name) where T: class {
            T entry;
            if(!data.TryGetValue(Util.ToID(name), out entry)) return null;
            else return entry;
        }
        public static PokeDexEntry GetPokemon(string name) {
            return Get<PokeDexEntry>(pokes, name);
        }
        public static AbilityEntry GetAbility(string name) {
            return Get<AbilityEntry>(abis, name);
        }

        public static MoveEntry GetMove(string name) {
            return Get<MoveEntry>(moves, name);
        }

        public static ItemEntry GetItem(string name) {
            return Get<ItemEntry>(items, name);
        }

        public static TypeEntry GetType(string name) {
            return Get<TypeEntry>(types, name);
        }

        public static NatureEntry GetNature(string name) {
            return Get<NatureEntry>(natures, name);
        }
    }
}