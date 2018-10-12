using System.Collections.Generic;
using PokémonAPI;
using static PokémonAPI.HelperMethods;

namespace PokémonSimulator
{
    /// <summary>
    /// A class for holding pre-generated Pokémon.
    /// </summary>
    public class RentalPokémon: Pokémon
    {

        public List<RentalPokémon> AvailablePokémon { get; }= new List<RentalPokémon>();

        public static RentalPokémon RentalPor23 { get; } = new RentalPokémon(null, new List<Move>());

        public static RentalPokémon RentalPorygon { get; } = new RentalPokémon(Species.Porygon, Listify(Move.IceBeam, Move.Thunderbolt, Move.Swift, Move.Tackle));
        public static RentalPokémon RentalCharizard { get; } = new RentalPokémon(Species.Charizard, Listify(Move.Flamethrower));
        public static RentalPokémon RentalBlastoise { get; } = new RentalPokémon(Species.Blastoise, Listify(Move.HydroPump));
        public static RentalPokémon RentalVenusaur { get; } = new RentalPokémon(Species.Venusaur, Listify(Move.SolarBeam));


        private RentalPokémon(Species species, IEnumerable<Move> moves) : base(species, moves)
        {
            AvailablePokémon.Add(this);
        }
    }
}
