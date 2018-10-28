using System;
using System.Collections.Generic;
using System.Linq;

namespace PokémonAPI
{
    /// <summary>
    /// Enumeration of valid Pokémon moves.
    /// </summary>
    public partial class Move
    {
        private Move(string name, Type type, MoveCategory category, int attackPower,
            Action<Pokémon, Pokémon> additionalEffects)
        {
            Name = name;
            AttackType = type;
            Category = category;
            AttackPower = attackPower;
            AllMoves.Add(this);
        }

        public static Move IceBeam { get; } = new Move("Ice Beam", Type.Ice, MoveCategory.Special, 95, null);

        public static Move Thunderbolt { get; } =
            new Move("Thunderbolt", Type.Electric, MoveCategory.Special, 95, null);

        public static Move Flamethrower { get; } =
            new Move("Flamethrower", Type.Fire, MoveCategory.Special, 95, null);

        public static Move Tackle { get; } = new Move("Tackle", Type.Normal, MoveCategory.Physical, 35, null);

        public static Move Swift { get; } = new Move("Swift", Type.Normal, MoveCategory.Physical, 60, null);

        public static Move SolarBeam { get; } = new Move("SolarBeam", Type.Grass, MoveCategory.Special, 120, null);

        public static Move HydroPump { get; } = new Move("Hydro Pump", Type.Water, MoveCategory.Special, 120, null);

        /// <summary>
        /// Changes the user's types to match the opponent's
        /// </summary>
        public static Move Conversion { get; } = new Move("Conversion", Type.Normal, MoveCategory.Status, 0,
            (Pokémon attacker, Pokémon defender) => { attacker.Types.Clear(); attacker.Types.AddRange(defender.Types);}
            );


        public List<Move> AllMoves { get; } = new List<Move>();
    }
}
