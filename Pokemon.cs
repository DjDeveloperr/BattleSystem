using System;
using System.Collections.Generic;

namespace BattleSystem
{
    /// <summary>
    ///     Pokemon Set's Move Slots
    /// </summary>
    public class MoveSetSlot
    {
        public string name;
        public int index;
        public int pp;
        public int maxpp;

        public MoveSetSlot() {  }
        public MoveSetSlot(string _name, int _index, int _pp, int _maxpp) {
            name = _name;
            index = _index;
            pp = _pp;
            maxpp = _maxpp;
        }
    }

    /// <summary>
    ///     Pokemon Set - basically Pokemon outside Battle
    /// </summary>
    public class PokemonSet
    {
        public string name;
        public int level;
        public int maxhp;
        public int hp;
        public string nature;
        public string ability;
        public bool shiny;
        public string form = null;
        public Status status;
        public StatsTable ivs;
        public StatsTable evs;
        public MoveSetSlot[] moves;
        public string pokeball = "Poke Ball";
        public Gender gender;
        public int happiness = 0;
        public bool gigantamax = false;
        public string item = null;

        public PokemonSet FromText(string txt)
        {
            PokemonSet set = new PokemonSet();
            string[] spl = txt.Split("\n");
            foreach(string el in spl) {
                if(el.StartsWith("Level: ")) set.level = Int32.Parse(Util.AfterColon(el));
                else if(el.StartsWith("Nature: ")) set.nature = Util.AfterColon(el);
                else if(el.StartsWith("Item: ") && Util.AfterColon(el) != "None") set.item = Util.AfterColon(el);
                else if(el.StartsWith("Pokeball: ")) set.pokeball = Util.AfterColon(el);
                else if(el.StartsWith("Happiness: ")) set.happiness = Int32.Parse(Util.AfterColon(el));
                else if(el.StartsWith("Form: ") && Util.AfterColon(el) != "None") set.form = Util.AfterColon(el);
                else if(el.StartsWith("Gender: ")) {
                    Gender g = Gender.None;
                    string val = Util.AfterColon(el).ToLower();
                    if(val == "m") g = Gender.M;
                    else if(val == "f") g = Gender.F;
                    set.gender = g;
                } else if(el.StartsWith("IVs: ")) set.ivs = new StatsTable(Util.ParseStats(Util.AfterColon(el)));
                else if(el.StartsWith("EVs: ")) set.evs = new StatsTable(Util.ParseStats(Util.AfterColon(el)));
                else if(el.StartsWith("Info: ")) {

                } else if(el.StartsWith("- ")) {
                    string[] md = el.Remove(0, 1).Trim().Split("@");
                    MoveSetSlot ms = new MoveSetSlot();
                    ms.name = md[0];
                    int pp = 0;
                    int maxpp = 0;
                    if(md.Length >= 2) {
                        string[] ppd = md[1].Split("/");

                        if(ppd.Length >= 1) Int32.TryParse(ppd[0], out pp);
                        if(ppd.Length >= 2) Int32.TryParse(ppd[1], out maxpp);
                    }
                    ms.pp = pp;
                    ms.maxpp = maxpp;
                } else if(el.IndexOf("@") > -1) {
                    string[] d = el.Split("@");
                    string name = d[0];
                    if(name.StartsWith("G-Max")) {
                        set.gigantamax = true;
                        name = name.Remove(0, 5).Trim();
                    }
                    set.name = name;
                    string abi = d.Length >= 2 ? d[1] : "No Ability";
                    bool shiny = abi.IndexOf("[S]") > -1;
                    if(shiny) abi = abi.Split("[S]")[0].Trim();
                    set.shiny = shiny;
                    set.ability = ability;
                }
            }
            return set;
        }

        public string ToText()
        {
            return 
            (gigantamax ? "G-Max " : "") + name + " @ " + ability + (shiny ? " [S]" : "") 
            + "\nLevel: " + level 
            + "\nInfo: " + hp + "/" + maxhp + (status != Status.None ? (" " + Util.FixStatus(status)) : "") 
            + "\nGender: " + (gender == Gender.M ? "M" : (gender == Gender.F ? "F" : "None")) 
            + "\nNature: " + nature 
            + "\nItem: " + (item == null ? "None" : item) 
            + "\nIVs: " + ivs.ToString() 
            + "\nEVs: " + evs.ToString() 
            + "\nHappiness: " + happiness 
            + "\nPokeball: " + pokeball
            + "\nForm: " + (form == null ? "None" : form)
            + "\n" + Util.MovesToText(moves);
        }
    }

    /// <summary>
    ///     Move Slot object of Pokemon in Battle
    /// </summary>
    public class MoveSlot
    {
        public int index;
        public string move;
        public int pp = 0;
        public int maxpp = 0;
        public string target = null;
        public bool disabled = false;
        public string disabledSource = null;
        public bool used = false;
        public bool isVirtual = false;

        public PokeDex.MoveEntry GetEntry() {
            return PokeDex.Data.GetMove(move);
        }
    }

    public class PokemonVolatiles {
        public string protect = null;
        public string detect = null;
        public string maxGaurd = null;
        public string kingsShield = null;
        public string spikyShield = null;
        public string banefulBunker = null;
        public string obstruct = null;
    }

    /// <summary>
    ///     (Battle) Pokemon object
    /// </summary>
    public class Pokemon
    {
        public Battle battle;
        public Side side;
        public PokemonSet set;

        public string name;
        public int level;
        public bool shiny;
        public bool gmax;
        public int happiness;
        public string pokeball;
        public Gender gender;
        public string nature = null;
        public string baseHpType;
        public int baseHpPower;

        public MoveSlot[] baseMoves;
        public List<MoveSlot> moves = new List<MoveSlot>();

        public string hpType;
        public int hpPower;

        public int position;
        public Status status;
        public StatsTable baseStoredStats;
        public StatsTableBase storedStats;
        public BoostsTable boosts;

        public string baseAbility;
        public string ability;

        public string item = null;
        public bool usedItemThisTurn = false;
        public bool ateBerry;

        public bool trapped = false;
        public bool maybeTrapped = false;
        public bool maybeDisabled = false;

        public Pokemon illusion = null;
        public string transformed = null;

        public int maxhp = 0;
        public int baseMaxhp = 0;
        public int hp = 0;
        public string[] types;
        public string addedType = null;
        public string apparentType = null;
        public string switchFlag = null;
        public bool forceSwitchFlag = false;
        public bool switchCopyFlag = false;
        public bool newlySwitched = false;
        public bool beingCalledBack = false;
        public int draggedIn;
        public bool mustRecharge = false;
        public bool confused = false;

        public string lastMove = null;
        public Pokemon lastMoveTarget = null;
        public string moveThisTurn = null;
        public bool statsRaisedThisTurn = false;
        public bool statsLoweredThisTurn = false;
        public bool hurtThisTurn = false;
        public int lastDamage = 0;

        public int activeTurns = 0;
        public int activeMoveActions = 0;
        public int previouslySwitchedIn;
        public bool trauntTurn = false;
        public bool isStarted = false;
        public bool duringMove = false;

        public int weight = 0;
        public int speed = 0;
        public int abilityOrder = 0;

        public bool gigantamax = false;
        public bool dynamax = false;
        public bool ultra = false;
        public string mega = null;

        public Staleness staleness = 0;
        public Staleness pendingStaleness = 0;
        public Staleness volatileStaleness = 0;
        public PokemonVolatiles volatiles = new PokemonVolatiles();

        public string form = null;

        public Pokemon() { }
        public Pokemon(PokemonSet _set, Side _side)
        {
            side = _side;
            set = _set;
            battle = side.battle;

            name = set.name;
            level = set.level;
            shiny = set.shiny || false;
            gender = set.gender;
            happiness = set.happiness;
            pokeball = set.pokeball;
            baseMaxhp = set.maxhp;
            nature = set.nature;
            maxhp = set.maxhp;
            hp = set.hp;
            status = set.status;
            form = set.form;

            baseAbility = set.ability;
            ability = set.ability;

            var hpData = Dex.getHP(set.ivs);
            hpType = hpData.type;
            hpPower = hpData.power;

            baseHpPower = hpPower;
            baseHpType = hpType;

            weight = 1;

            var entry = GetEntry();
            types = entry.types;
            if(entry.maxHP != -1) maxhp = hp = baseMaxhp = entry.maxHP;
            if(entry.noGender) gender = Gender.None;

            foreach (var ms in set.moves) {
                moves.Add(new MoveSlot() {
                    index = ms.index,
                    move = ms.name,
                    pp = ms.pp,
                    maxpp = ms.maxpp,
                });
            }
        }

        public bool canMegaEvo
        {
            get
            {
                return battle.settings.mega && mega == null && item != null;
            }
        }

        public bool fainted
        {
            get
            {
                return hp == 0;
            }
        }

        public bool canDynamax
        {
            get
            {
                return battle.settings.dynamax && !dynamax;
            }
        }

        public bool canUltraBurst
        {
            get
            {
                return battle.settings.ultra && (name == "Necrozma-Dawn-Wings" || name == "Necrozma-Dusk-Mane") && item == "Ultranecrozium Z";
            }
        }

        public string fullname
        {
            get
            {
                string n = name;
                if (transformed != null) name = transformed;
                return Util.FixName(mega != null ? mega : (gigantamax ? ("G-Max " + n) : n));
            }
        }

        public bool isActive
        {
            get
            {
                return position == 0;
            }
        }

        public MoveSlot GetMove(int slot)
        {
            if(slot > 4 || moves.Count < slot || slot < 1) return null;
            return moves[slot-1];
        }

        public int Heal(int amount)
        {
            if (hp == maxhp) return 0;
            if (hp + amount > maxhp) amount = maxhp - hp;
            hp += amount;
            return amount;
        }

        public int Damage(int amount)
        {
            if (fainted) return 0;
            if (hp < amount) amount = hp;
            hp -= amount;
            lastDamage = amount;
            return amount;
        }

        public bool ConsumeItem()
        {
            if (item == null) return false;
            item = null;
            return true;
        }

        public void UseExternalItem(string _item)
        {

        }

        new public string ToString()
        {
            return "#" + (position + 1) + " " + (shiny ? "[S] " : "") + "Lv. " + level + fullname + (gender == Gender.M ? " (M)" : (gender == Gender.F ? " (F)" : "")) + " [HP: " + hp + "/" + maxhp + "]";
        }

        public PokeDex.PokeDexEntry GetEntry() {
            return PokeDex.Data.GetPokemon(name);
        }

        public PokeDex.AbilityEntry GetAbility() {
            return PokeDex.Data.GetAbility(ability);
        }

        public PokeDex.NatureEntry GetNature() {
            return PokeDex.Data.GetNature(nature);
        }

        public PokeDex.ItemEntry GetItem() {
            if(item == null) return null;
            else return PokeDex.Data.GetItem(item);
        }
    }
}
