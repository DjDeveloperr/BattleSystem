using System;

/// <summary>
///     A full-fledged Pokemon Battle system.
/// </summary>
namespace BattleSystem
{
    /// <summary>
    ///     Basic Player options for Battle. Used by Battle Side.
    /// </summary>
    public class PlayerOptions {
        public string name;
        public PokemonSet[] team;
        public bool npc = false;
    }

    /// <summary>
    ///     Battle Settings that decide allowations, nature of Battle, etc
    /// </summary>
    public class BattleSettings {
        public bool rated = false;
        public bool wild = false;
        public bool dynamax = false;
        public bool mega = false;
        public bool zmove = false;
        public bool ultra = false;
        public bool teampreview = false;
        public bool autostart = false;
        public bool items = false;
    }

    /// <summary>
    ///     Battle class - to instanciate a Pokemon Battle and simulate
    /// </summary>
    public class Battle {
        public Field field;
        public Side[] sides = new Side[2];
        public int turn = 0;
        public bool started = false;
        public bool ended = false;
        public string winner = null;
        public bool hasRun = false;
        public RequestState state;
        public BattleSettings settings;
        public DateTime startedAt;
        public DateTime endedAt;
        public DateTime lastUpdate = DateTime.Now;

        /** 
          * Events Declaration 
          * Event Delegates are declared in BattleEvents.cs
          */
        /// <summary>
        ///     When Battle starts at "time"
        /// </summary>
        public event EventOnStart OnStart;
        /// <summary>
        ///     When battle ends at "time"
        /// </summary>
        public event EventOnEnd OnEnd;
        /// <summary>
        ///     When "poke" used "move" on "target" (?)
        /// </summary>
        public event EventOnAttack OnAttack;
        /// <summary>
        ///     When Pokemon ("takenBy") took "damage" from "move" used by "from" 
        /// </summary>
        public event EventOnDamage OnDamage;
        /// <summary>
        ///     When "poke" fainted
        /// </summary>
        public event EventOnFaint OnFaint;
        /// <summary>
        ///     When "poke" had used "item"
        /// </summary>
        public event EventOnItemUse OnItemUse;
        /// <summary>
        ///     When player used item on "poke" (Externally)
        /// </summary>
        public event EventOnItemUseExternal OnItemUseExternal;
        /// <summary>
        ///     When "poke"'s "item" has been consumed
        /// </summary>
        public event EventOnItemEnd OnItemEnd;
        /// <summary>
        ///     When "poke"'s ability is announced/affected
        /// </summary>
        public event EventOnAbility OnAbility;
        /// <summary>
        ///     When "poke" had Mega Evolved
        /// </summary>
        public event EventOnMega OnMega;
        /// <summary>
        ///     When "poke" had Dynamaxed
        /// </summary>
        public event EventOnDynamax OnDynamax;
        /// <summary>
        ///     When "poke" had used its Z-Power
        /// </summary>
        public event EventOnZmove OnZmove;
        /// <summary>
        ///     When "poke" had Ultra Burst (DW/DM Necrozma)
        /// </summary>
        public event EventOnUltraBurst OnUltraBust;
        /// <summary>
        ///     When "condition" started on Battle Field
        /// </summary>
        public event EventOnField OnFieldStart;
        /// <summary>
        ///     When "condition" ended on Battle Field
        /// </summary>
        public event EventOnField OnFieldEnd;
        /// <summary>
        ///     When "condition" is started on "side"
        /// </summary>
        public event EventOnSide OnSideStart;
        /// <summary>
        ///     When "side"'s "condition" has ended
        /// </summary>
        public event EventOnSide OnSideEnd;
        /// <summary>
        ///     When "poke" transformed into "to"
        /// </summary>
        public event EventOnTransform OnTransform;
        /// <summary>
        ///     When "poke" changed form to "form"
        /// </summary>
        public event EventOnFormChange OnFormChange;
        /// <summary>
        ///     When "poke" resisted an effect/move/etc
        /// </summary>
        public event EventOnResist OnResist;
        /// <summary>
        ///     When "poke" was immune from an effect/move/etc
        /// </summary>
        public event EventOnImmune OnImmune;
        /// <summary>
        ///     When "poke" was switched out
        /// </summary>
        public event EventOnSwitch OnSwitch;
        /// <summary>
        ///     When "poke" was dragged in to the battle field
        /// </summary>
        public event EventOnDrag OnDrag;
        /// <summary>
        ///     When "poke" is affected by "status"
        /// </summary>
        public event EventOnStatus OnStatus;
        /// <summary>
        ///     When "poke" cures from "status"
        /// </summary>
        public event EventOnStatusCure OnStatusCure;
        /// <summary>
        ///     When illusion of "poke" stats
        /// </summary>
        public event EventOnIllusion OnIllusion;
        /// <summary>
        ///     When weather has changed to "weather"
        /// </summary>
        public event EventOnWeather OnWeatherStart;
        /// <summary>
        ///     When "weather" has changed to none
        /// </summary>
        public event EventOnWeather OnWeatherEnd;
        /// <summary>
        ///     When "effect" fails
        /// </summary>
        public event EventOnFail OnFail;
        /// <summary>
        ///     When "side" wins the battle
        /// </summary>
        public event EventOnWin OnWin;
        /// <summary>
        ///     When tie-breaker happens
        /// </summary>
        public event EventOnTie OnTie;
        /// <summary>
        ///     When turn count changes to "turn"
        /// </summary>
        public event EventOnTurn OnTurn;
        /// <summary>
        ///     When a Pokemon uses move and it must recharge next turn
        /// </summary>
        public event EventOnRecharge OnRecharge;
        /// <summary>
        ///     When a Pokemon (Magikarp) used move but absolutely nothing happened
        /// </summary>
        public event EventOnNothing OnNothing;
        /// <summary>
        ///     When "stat" boost of "poke" was boosted by "amount"
        /// </summary>
        public event EventOnStatBoost OnStatBoost;
        /// <summary>
        ///     When "stat" boost of "poke" was un-boosted by "amount"
        /// </summary>
        public event EventOnStatUnboost OnStatUnboost;
        /// <summary>
        ///     When "stat" boost of "poke" was cleared
        /// </summary>
        public event EventOnStatClear OnStatClear;
        /// <summary>
        ///     When all stat boosts of "poke" were cleared
        /// </summary>
        public event EventOnStatClearAll OnStatClearAll;
        /// <summary>
        ///     When "poke" used "berry"
        /// </summary>
        public event EventOnBerryUse OnBerryUse;
        /// <summary>
        ///     When stat changes of "poke" were inverted
        /// </summary>
        public event EventOnStatInvert OnStatInvert;
        /// <summary>
        ///     When "poke" was healed by "amount" hp
        /// </summary>
        public event EventOnHeal OnHeal;
        /// <summary>
        ///     When "by" copied stats from "from"
        /// </summary>
        public event EventOnStatCopy OnStatCopy;
        /// <summary>
        ///     When stats of "poke1" and "poke2" were swapped
        /// </summary>
        public event EventOnSwapBoost OnSwapBoost;
        /// <summary>
        ///     When "stat" boost of "poke" was directly modified to "amount"
        /// </summary>
        public event EventOnSetBoost OnSetBoost;
        /// <summary>
        ///     When a "poke" (Groudon/Kyogre) changes to Primal
        /// </summary>
        public event EventOnPrimal OnPrimal;
        /// <summary>
        ///     When a double-turn "move" was used for which "poke" have to prepare
        /// </summary>
        public event EventOnPrepare OnPrepare;
        /// <summary>
        ///     When a multi-hit "move" was hit by "poke" on "target" "count" times
        /// </summary>
        public event EventOnHitCount OnHitCount;
        /// <summary>
        ///     When a "move" used by "poke" was a critical hit on "target"
        /// </summary>
        public event EventOnCritical OnCritical;
        /// <summary>
        ///     When a "move" used by "poke" was super effective on "target"
        /// </summary>
        public event EventOnSuperEffective OnSuperEffective;
        /// <summary>
        ///     When a "move" used by "poke" on "target" (?) misses
        /// </summary>
        public event EventOnMiss OnMiss;
        /// <summary>
        ///     When a move was used by "poke" (?) and there was no target
        /// </summary>
        public event EventOnNoTarget OnNoTarget;
        /// <summary>
        ///     When a "poke" couldn't perform action because of "reason"
        /// </summary>
        public event EventOnCant OnCant;
        /// <summary>
        ///     Whenever a choice of type "type" is active for Player "side"
        /// </summary>
        public event EventOnChoice OnChoice;
        /// <summary>
        ///     When user runs from a Wild Pokemon
        /// </summary>
        public event EventOnNothing OnRun;
        /// <summary>
        ///     When Battle Text is sent
        /// </summary>
        public event EventOnText OnText;

        public Battle(PlayerOptions pl1, PlayerOptions pl2, BattleSettings _settings) {
            field = new Field(this);

            sides[0] = new Side(this);

            sides[1] = new Side(this);

            sides[0].name = pl1.name;
            sides[0].num = 0;

            sides[1].name = pl2.name;
            sides[1].num = 1;
            
            sides[1].foe = p1;
            sides[0].foe = p2;

            for (var i = 0; i < pl1.team.Length; i++) {
                PokemonSet pokeset = pl1.team[i];
                Pokemon poke = new Pokemon(pokeset, p1);
                poke.position = i;

                sides[0].team.Add(poke);
            }

            for (var i = 0; i < pl2.team.Length; i++) {
                PokemonSet pokeset = pl2.team[i];
                Pokemon poke = new Pokemon(pokeset, p2);
                poke.position = i;

                sides[1].team.Add(poke);
            }

            settings = _settings;
            OnText += (string text, Side side) => {};
            if(settings.autostart) Start();
        }

        public Side p1 {
            get {
                return sides[0];
            }
        }

        public Side p2 {
            get {
                return sides[1];
            }
        }

        public bool canMove {
            get {
                return started && !ended;
            }
        }

        public bool allChoicesDone {
            get {
                return p1.choice.isDone && p2.choice.isDone;
            }
        }

        new public string ToString() {
            return "[" + (started ? ("Started: " + startedAt.ToLongTimeString()) : (ended ? ("Ended: " + endedAt.ToLongTimeString() + " (" + winner + ")"): "Not Started")) + "] Battle - " + p1.name + " vs " + p2.name + " | " + lastUpdate.ToLongTimeString();
        }

        void Text(params string[] args) {
            OnText(string.Join(" ", args));
        }

        public void Start() {
            started = true;
            startedAt = DateTime.Now;
            Update();
            if(OnStart != null) OnStart(startedAt);
            OnText("Battle has started!");
            state = settings.teampreview ? RequestState.Teampreview : RequestState.Move;
            turn = 0;
            if(state != RequestState.Teampreview) {
                p1.choice.switchIn = 0;
                p2.choice.switchIn = 0;
                CommitDecisions();
            } else {
                SendChoices();
            }
        }

        public void EmitCant(Pokemon poke, string reason) {
            OnCant(poke, reason);
            Text(poke.name, "couldn't move because of", reason+"!");
        }

        public void SendChoices(RequestState _s = RequestState.None, bool forced = false) {
            if(_s == RequestState.None) _s = state;
            foreach(var side in sides) {
                if(OnChoice != null) OnChoice(side, Util.ReqState(_s), forced);
                OnText("What would you do now?", side);
            }
        } 

        public bool NextTurn() {
            if(!canMove) return false;
            // TODO: Damage active Pokemon's if they are Toxic'd/Poisoned

            // Decide winner if one has really won
            if(p1.noPokesLeft || p2.noPokesLeft) {
                Side win = p1.noPokesLeft ? (!p2.noPokesLeft ? p2 : null) : p1;
                if(win == null) {
                    // It's a tie
                    Tie();
                    return true;
                } else {
                    // A side won already
                    Win(win);
                    return true;
                }
            }

            turn++;
            Pokemon active1 = p1.active;
            Pokemon active2 = p2.active;
            if(active1 != null) active1.activeTurns++;
            if(active2 != null) active2.activeTurns++;
            p1.choice.Clear();
            p2.choice.Clear();
            Update();
            if(OnTurn != null) OnTurn(turn);
            Text($"Turn {turn}");
            state = RequestState.Move;
            SendChoices();
            return true;
        }

        public bool Heal(Pokemon target, int amount, string source) {
            int healed = target.Heal(amount);
            if(healed == 0) return false;
            if(OnHeal != null) OnHeal(target, amount, source);
            OnText($"{target.name} gained some HP" + (source != null ? (" from " + source) : "") + "!");
            return true;
        }

        public bool Damage(Pokemon target, int amount, Pokemon source = null, bool silent = false) {
            int damaged = target.Damage(amount);
            if(damaged == 0) return false;
            if(!silent && OnDamage != null) OnDamage(source, amount, target);
            OnText($"{target.name} lost ${amount} HP!");
            return true;
        }

        public bool SwitchTo(Pokemon poke) {
            bool switched = poke.side.SwitchTo(poke);
            if(switched && OnSwitch != null) OnSwitch(poke);
            Text("Go,", poke.name + "!");
            return switched;
        }

        public bool UseMove(Pokemon poke, MoveAction move, Pokemon target = null) {
            if(poke.status == Status.Frozen) {
                if(OnCant != null) OnCant(poke, "Frozen");
                Text(poke.name, "couldn't move because of being Frozen!");
                return true;
            }
            if(poke.status == Status.Paralyze && Util.Random(0, 100) <= 50) {
                if(OnCant != null) OnCant(poke, "Paralyzed");
                Text(poke.name, "couldn't move because of being Paralyzed!");
                return true;
            }
            if(poke.mustRecharge) {
                if(OnCant != null) OnCant(poke, "Recharge");
                Text(poke.name, "couldn't move because of recharge!");
                return true;
            }
            if(poke.confused && Util.Random(0, 100) <= 50) {
                Damage(poke, 10, null, true);
                if(OnCant != null) OnCant(poke, "Confusion");
                Text(poke.name, "couldn't move because of confusion!");
                Text(poke.name, "hurt itself in confusion!");
                return true;
            }
            bool used = poke.side.UseMove(poke, move, target);
            if(used && OnAttack != null) OnAttack(poke, move, target);
            if(used) Text(poke.name, "used", move.name + (target != null ? (" on " + target.name) : "") + "!"); 
            return used;
        }

        public bool UseItem(Pokemon poke, string item) {
            bool used = poke.side.UseItem(poke, item);
            if(used && OnItemUseExternal != null) OnItemUseExternal(poke, item);
            if(used) Text(poke.name, "used", item + "!");
            return used;
        }

        public bool ChooseLead(Pokemon poke) {
            if(!canMove) return false;
            bool chosen = poke.side.ChooseLead(poke);
            TryCommitDecisions(chosen);
            return chosen;
        }

        public bool DragIn(Pokemon poke) {
            bool dragged = poke.side.SwitchTo(poke, true);
            if(dragged && OnDrag != null) OnDrag(poke);
            if(dragged) Text(poke.name, "was dragged in!");
            return dragged;
        }

        public bool ChooseSwitch(Pokemon poke, bool isPass = false) {
            if(!canMove) return false;
            bool done = poke.side.ChooseSwitch(poke, isPass);
            TryCommitDecisions(done);
            return done;
        }

        public bool ChooseMove(Pokemon poke, int slot) {
            if(!canMove) return false;
            bool done = poke.side.ChooseMove(poke, slot);
            TryCommitDecisions(done);
            return done;
        }

        public bool ChooseItem(Pokemon poke, string item) {
            if(!canMove) return false;
            bool done = poke.side.ChooseItem(poke, item);
            TryCommitDecisions(done);
            return done;
        }

        public bool ChooseZmove(Pokemon poke) {
            if(!canMove) return false;
            bool done = poke.side.ChooseZmove(poke);
            TryCommitDecisions(done);
            return done;
        }

        public bool ChooseMega(Pokemon poke) {
            if(!canMove) return false;
            bool done = poke.side.ChooseMega(poke);
            TryCommitDecisions(done);
            return done;
        }

        public bool ChooseDynamax(Pokemon poke) {
            if(!canMove) return false;
            bool done = poke.side.ChooseDynamax(poke);
            TryCommitDecisions(done);
            return done;
        }

        public bool TryCommitDecisions(bool externalTask = true) {
            if(allChoicesDone && externalTask) {
                CommitDecisions();
                return true;
            } else return false;
        }

        public void CommitDecisions(bool forced = false) {
            if(!allChoicesDone && !forced) return;
            Console.WriteLine("Commit...");
            bool p1done = false;
            bool p2done = false;
            if(p1.choice.switchIn != -1) {
                SwitchTo(p1.GetPokemon(p1.choice.switchIn));
                p1done = true;
            }
            if(p2.choice.switchIn != -1) {
                SwitchTo(p2.GetPokemon(p2.choice.switchIn));
                p2done = true;
            }
            if(p1.choice.useItem != null) {
                UseItem(p1.choice.useItem.pokemon, p1.choice.useItem.item);
                p1done = true;
            }
            if(p2.choice.useItem != null) {
                UseItem(p2.choice.useItem.pokemon, p2.choice.useItem.item);
                p2done = true;
            }
            if(!p1done || !p2done) {
                int first = 0;
                if(p1.active.speed > p2.active.speed && !p1done) first = 1;
                else if(p2.active.speed > p1.active.speed && !p2done) first = 2;
                else first = 0;

                if(first == 0) first = Util.Random(0, 2) == 1 ? 1 : 2;

                Side s1 = first == 1 ? p1 : p2;
                Side s2 = first == 1 ? p2 : p1;
                bool s1d = s1.num == 0 ? p1done : p2done;
                bool s2d = s2.num == 0 ? p1done : p2done;

                if(!s1d && s1.choice.move != null) {
                    bool used = UseMove(s1.active, s1.choice.move, s2.active);
                }

                if(!s2d && s2.choice.move != null) {
                    bool used = UseMove(s2.active, s2.choice.move, s1.active);
                }
            }
            NextTurn();
        }

        public bool Win(Side side) {
            if(!canMove) return false;
            winner = side.name;
            if(OnWin != null) OnWin(side);
            Text(side.name, "has won the battle!");
            End();
            return true;
        }

        public void Update() {
            lastUpdate = DateTime.Now;
        }

        public bool Tie() {
            if(!canMove) return false;
            winner = null;
            if(OnTie != null) OnTie();
            Text("Battle ended with a tie!");
            End();
            return true;
        }

        public bool Run() {
            if(!canMove) return false;
            if(!settings.wild) return false;
            winner = null;
            hasRun = true;
            if(OnRun != null) OnRun();
            Text("You got away safely.");
            End();
            return true;
        }

        public bool End() {
            if(ended) return false;
            ended = true;
            endedAt = DateTime.Now;
            Update();
            if(OnEnd != null) OnEnd(endedAt);
            Text("Battle has ended.");
            return true;
        }
    }
}
