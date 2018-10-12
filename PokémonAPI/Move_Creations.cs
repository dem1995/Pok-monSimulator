using System;
using System.Collections.Generic;

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

        public List<Move> AllMoves { get; } = new List<Move>();
    }
}
