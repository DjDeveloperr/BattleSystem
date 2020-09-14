using System;

namespace BattleSystem
{
    /// <summary>
    ///     Request State in Battle.
    /// </summary>
    public enum RequestState { None = 0, Teampreview = 1, Move = 2, Switch = 3 };
    /// <summary>
    ///     Type of choice in Choice Action object
    /// </summary>
    public enum Choices { Move = 0, Switch = 1, InstaSwitch = 2, Team = 3, Shift = 4, Pass = 5 };
    /// <summary>
    ///     Pokemon Gender enum
    /// </summary>
    public enum Gender { None = 0, M = 1, F = 2 };
    /// <summary>
    ///     Status Condition of Pokemon
    /// </summary>
    public enum Status { None = 0, Burn = 1, Paralyze = 2, Frozen = 3, Poison = 4, Toxic = 5, Confusion = 6 };
    /// <summary>
    ///     Type of Staleness
    /// </summary>
    public enum Staleness { None = 0, Extenal = 1, Internal = 2 };
}
