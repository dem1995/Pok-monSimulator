using System.Collections.Generic;
using System.Linq;

namespace PokémonSimulator
{
    /// <summary>
    /// Data structure that represents individual Pokémon.
    /// </summary>
    public class Pokémon
    {
        /// <summary>
        /// Constructs a Pokémon of a given species with the given moves
        /// </summary>
        /// <param name="species">The maximum number of hit points this Pokémon can have.</param>
        /// <param name="moves">The moves this Pokémon has</param>
        public Pokémon(Species species, IEnumerable<Move> moves)
        {
            Level = 100;
            Species = species;
            Moves = moves.ToList();
        }

        private int fred;

        public int Level { get; }
        public Species Species { get; }
        public List<Type> Types { get; set; }


        /// <summary>
        /// The list of <see cref="Move" />s this Pokémon has, presumably 4 or fewer.
        /// </summary>
        /// <value>
        /// The assortment of moves this Pokémon has
        /// </value>
        public List<Move> Moves { get; set; }


        public Dictionary<Stat, int> Stats => Species.BaseStats;

        public int RemainingHealth
        {
            get
            {
                // If damage exceeds max health, return zero. Otherwise, return the difference.
                if (Damage >= Stats[Stat.HP])
                    return 0;
                return Damage - Stats[Stat.HP];
            }
        }

        public int Damage { get; set; } = 0;
    }
}