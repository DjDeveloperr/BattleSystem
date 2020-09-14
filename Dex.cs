using System;

namespace BattleSystem
{
    public class HPData {
        public string type;
        public int power = 0;
    }

    public class Dex {
        public static string[] hpTypes = { "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Ghost", "Steel",
			"Fire", "Water", "Grass", "Electric", "Psychic", "Ice", "Dragon", "Dark", };
        
        public static HPData getHP(StatsTable ivs) {
            var res = new HPData();
            int hpTypeX = 0;
            int i = 1;
            foreach (var e in ivs.Array()) {
                hpTypeX += i * (e % 2);
                i *= 2;
            }
            // Gen6 + : always 60 hp power
            res.power = 60;
            res.type = hpTypes[hpTypeX * 15 / 63];
            return res;
        }
    }
}
