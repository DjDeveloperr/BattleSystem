using System;

namespace BattleSystem
{
    /// <summary>
    ///     Pokemon Battle field properties object
    /// </summary>
    public class Field {
        public Battle battle;
        public string weather = null;
        public string terrain = null;
        public Field(Battle _battle) {
            battle = _battle;
        }

        public void Reset() {
            weather = null;
            terrain = null;
        }

        new public string ToString() {
            return "(Field) Weather: " + (weather == null ? "None" : weather) + ", Terrain: " + (terrain == null ? "None" : terrain);
        }
    }
}
