using System;
using System.Collections.Generic;

namespace BattleSystem
{

    /// <summary>
    ///     Basically a Move object
    /// </summary>
    public class MoveAction {
        public int index = 0;
        public string name = "";
        public bool zMove = false;
        public bool maxMove = false;
    }

    public class UseItem {
        public Pokemon pokemon = null;
        public string item = null;
    }

    /// <summary>
    ///     Choice object of a side
    /// </summary>
    public class Choice {
        public Side side;
        public string error = null;
        public MoveAction move = null;
        public int switchIn = -1;
        public bool zMove = false;
        public bool mega = false;
        public bool ultra = false;
        public bool dynamax = false;
        public int forcedSwitchesLeft = 0;
        public int forcedPassesLeft = 0;
        public bool isPass = false;
        public UseItem useItem = null;
        public bool isDone {
            get {
                return forcedPassesLeft == 0 && forcedPassesLeft == 0 && (move != null || switchIn != -1 || (side.battle.settings.items && useItem != null)) && error == null;
            }
        }

        public void Clear() {
            error = null;
            move = null;
            switchIn = -1;
            zMove = false;
            mega = false;
            ultra = false;
            dynamax = false;
            isPass = false;
            useItem = null;
            forcedSwitchesLeft = 0;
            forcedPassesLeft = 0;
        }
    }

    /// <summary>
    ///     Active request object of a Side
    /// </summary>
    public class ActiveRequest {
        public Side side;
        public MoveAction[] moves = {};
        public MoveAction[] maxMoves = null;
        public MoveAction[] zMoves = null;
    }

    /// <summary>
    ///     A Side in Battle, represents Player
    /// </summary>
    public class Side {
        public Battle battle;
        public int num = 0;
        public string name;
        public List<Pokemon> team = new List<Pokemon>();
        public Side foe;

        public bool faintedLastTurn = false;
        public bool faintedThisTurn = false;

        public bool zMoveUsed = false;
        public bool megaUsed = false;
        public bool dmaxUsed = false;

        public Choice choice;
        public ActiveRequest activeRequest;

        public Side(Battle _battle) {
            battle = _battle;
            choice = new Choice() {
                side = this
            };
            activeRequest = null;
        }

        public Pokemon active {
            get {
                return GetPokemon(0);
            }
        }

        public int pokesLeft {
            get {
                int left = 0;
                foreach (var poke in team) if(!poke.fainted) left++;
                return left;
            }
        }

        public bool noPokesLeft {
            get {
                return pokesLeft == 0;
            }
        }

        public Pokemon GetPokemon(int pos) {
            Pokemon pokemon = null;
            foreach (var p in team) {
                if(p.position == pos) {
                    pokemon = p;
                    break;
                }
            }
            return pokemon;
        }

        public bool ChooseMega(Pokemon poke) {
            if(choice.isDone) return false;
            if(!poke.canMegaEvo) return false;
            choice.mega = !choice.mega;
            return true;
        }

        public bool ChooseZmove(Pokemon poke) {
            if(choice.isDone) return false;
            if(!battle.settings.zmove) return false;
            if(poke.item == null) return false;
            choice.zMove = !choice.zMove;
            return true;
        }

        public bool ChooseDynamax(Pokemon poke) {
            if(choice.isDone) return false;
            if(!poke.canDynamax) return false;
            choice.dynamax = !choice.dynamax;
            return true;
        }

        public bool ChooseUltraBurst(Pokemon poke) {
            if(choice.isDone) return false;
            if(!poke.canUltraBurst) return false;
            choice.ultra = !choice.ultra;
            return true;
        }

        public bool ChooseMove(Pokemon poke, int slot) {
            if(choice.isDone) return false;
            MoveSlot move = poke.GetMove(slot);
            if(move == null) return false;
            choice.move = new MoveAction() {
                index = slot,
                name = move.move,
                zMove = choice.zMove || false,
                maxMove = poke.dynamax || false
            };
            return true;
        }

        public bool ChooseSwitch(Pokemon poke, bool isPass = false) {
            if(choice.isDone) return false;
            if(choice.forcedSwitchesLeft != 0 && !isPass) choice.forcedSwitchesLeft--;
            else if(choice.forcedPassesLeft != 0 && isPass) choice.forcedPassesLeft--;
            else return false;
            choice.switchIn = poke.position;
            choice.isPass = isPass;
            return true;
        }

        public bool ChooseItem(Pokemon poke, string item) {
            if(choice.isDone) return false;

            choice.useItem = new UseItem() {
                pokemon = poke,
                item = item
            };
            return true;
        }

        public bool ChooseLead(Pokemon poke) {
            if(choice.isDone) return false;
            choice.switchIn = poke.position;
            return true;
        }

        public bool UseMove(Pokemon poke, MoveAction move, Pokemon target) {
            if(poke.fainted) return false;
            if(poke.side.name != name) return false; 
            if(!poke.isActive) return false;
            if(poke.GetMove(move.index).pp <= 0) {
                battle.EmitCant(poke, "No PP");
                return true;
            }
            PokeDex.MoveEntry moveData = poke.GetMove(move.index).GetEntry();
            if(moveData == null) {
                battle.EmitCant(poke, "No PP");
                return true;
            } else {
                // TODO: Run Move
            }
            poke.activeMoveActions++;
            return true;
        }

        public bool SwitchTo(Pokemon poke, bool isDrag = false) {
            if(poke.fainted) return false;
            if(poke.side.name != name) return false;
            if(poke.position == 0) return false;
            if(active.trapped || active.maybeTrapped) return false;
            Pokemon old = active;
            old.position = poke.position;
            poke.position = 0;
            old.activeTurns = 0;
            old.activeMoveActions = 0;
            if(isDrag) poke.draggedIn = old.position;
            poke.switchFlag = old.name;
            poke.newlySwitched = true;
            if(choice.isPass) poke.switchCopyFlag = true;
            return true;
        }

        public bool UseItem(Pokemon poke, string item) {
            if(poke.side.name != name) return false;
            poke.UseExternalItem(item);
            return true;
        }

        new public string ToString() {
            return (num + 1) + ": " + name + " [Left: " + pokesLeft + "]";
        }
    }
}
