using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BattleSystem
{

    public class StatsTableBase {
        public int atk = 0;
        public int def = 0;
        public int spe = 0;
        public int spa = 0;
        public int spd = 0;

        public StatsTableBase() {  }

        public StatsTableBase(int[] stats) {
            atk = stats[0];
            def = stats[1];
            spe = stats[2];
            spa = stats[3];
            spd = stats[4];
        }
        public StatsTableBase All(int val) {
            atk = def = spe = spa = spd = val;
            return this;
        }

        public int[] Array() {
            int[] stats = { atk, def, spe, spa, spd };
            return stats;
        }

        new public string ToString() {
            return String.Join(", ", Array());
        }
    }
    public class StatsTable : StatsTableBase {
        public int hp = 0;

        public StatsTable() {  }

        public StatsTable(int[] stats) {
            hp = stats[0];
            atk = stats[1];
            def = stats[2];
            spe = stats[3];
            spa = stats[4];
            if(stats.Length >= 6) spd = stats[5];
            else spd = 0;
        }

        new public int[] Array() {
            int[] stats = { hp, atk, def, spe, spa, spd };
            return stats;
        }

        new public StatsTable All(int val) {
            hp = atk = def = spe = spa = spd = val;
            return this;
        }
    }

    public class BoostsTable : StatsTableBase {
        public int acc = 0;
        public int eva = 0;
    } 

    public class JSON {
        public static T Parse<T>(string data) {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string Stringify(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }

    public class Util {
        public static string ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static Random RND = new Random();
        public static string ToID(string txt) {
            string res = "";
            txt = txt.ToLower();
            for(var  i = 0; i < txt.Length; i++) {
                if(ALLOWED_CHARS.IndexOf(txt[i]) > -1) res += txt[i];
            }
            return res;
        }
        public static int Random(int min, int max) {
            if(min == max) return min;
            if(min > max) {
                int temp = max;
                max = min;
                min = temp;
            } 
            return RND.Next(min, max);
        }

        public static string FixName(string name) {
            while (name.IndexOf("-") > -1) name = name.Replace('-', ' ');
            if(name.IndexOf(" Mega") > -1) name = "M. " + name.Split(" Mega")[0].Trim() + (name.EndsWith(" X") || name.EndsWith(" Y") ? (name.EndsWith(" X") ? " X" : " Y") : "");
            if(name.IndexOf(" Ultra") > -1) name = "U. " + name.Split(" Ultra")[0].Trim();
            if(name.IndexOf(" Ash") > -1) name = "Ash Greninja";
            if(name.IndexOf(" Dawn Wings") > -1) name = "DW. Necrozma";
            if(name.IndexOf(" Dusk Mane") > -1) name = "DM. Necrozma";
            if(name.IndexOf(" Primal") > -1) name = "Primal " + name.Split(" Primal")[0].Trim();
            return name.Replace("G Max", "G-Max");
        }

        public static string ReqState(RequestState state) {
            if(state == RequestState.Switch) return "switch";
            else if(state == RequestState.Move) return "teampreview";
            else if(state == RequestState.Move) return "move";
            else return "none";
        }

        public static string FixStatus(Status s) {
            string res = null;
            if(s == Status.Burn) res = "Burn";
            else if(s == Status.Confusion) res = "Confusion";
            else if(s == Status.Frozen) res = "Frozen";
            else if(s == Status.Paralyze) res = "Paralyze";
            else if(s == Status.Poison) res = "Poison";
            else if(s == Status.Toxic) res = "Toxic";
            return res;
        }

        public static string MovesToText(MoveSetSlot[] moves = null) {
            string ms = "";
            if(moves == null) return "";
            foreach (var mv in moves) {
                ms += (ms == "" ? "" : "\n") + "- " + mv.name + " @ " + mv.pp + "/" + mv.maxpp;
            }
            return ms;
        }

        public static string AfterColon(string txt) {
            return txt.IndexOf(":") > -1 ? (txt.Split(":")[1].Trim()) : txt;
        }

        public static int[] ParseStats(string txt) {
            string[] spl = txt.Split(",");
            List<int> res = new List<int>();
            
            foreach(var s in spl) {
                int a;
                if(Int32.TryParse(s.Trim(), out a)) {
                    res.Add(a);
                } else res.Add(0);
            }

            return res.ToArray();
        }
    }
}
