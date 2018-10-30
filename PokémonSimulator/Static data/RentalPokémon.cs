using System;
using System.Collections.Generic;
using PokémonAPI;
using static PokémonAPI.HelperMethods;

namespace PokémonSimulator
{
    /// <inheritdoc />
    /// <summary>
    /// A class for holding pre-generated Pokémon.
    /// </summary>
    public class RentalPokémon: Pokémon
    {

        public List<RentalPokémon> AvailablePokémon { get; }= new List<RentalPokémon>();

        public static RentalPokémon RentalPorygon => new RentalPokémon(Species.Porygon, Listify(Move.IceBeam, Move.Thunderbolt, Move.Swift, Move.Tackle));
        public static RentalPokémon RentalCharizard => new RentalPokémon(Species.Charizard, Listify(Move.Flamethrower, Move.Earthquake, Move.Strength, Move.Tackle));
        public static RentalPokémon RentalBlastoise => new RentalPokémon(Species.Blastoise, Listify(Move.HydroPump, Move.Earthquake, Move.Strength, Move.Tackle));
        public static RentalPokémon RentalVenusaur => new RentalPokémon(Species.Venusaur, Listify(Move.SolarBeam, Move.Cut, Move.BodySlam, Move.Tackle));
        public static RentalPokémon RentalGengar => new RentalPokémon(Species.Gengar, Listify(Move.Psychic, Move.Thunderbolt, Move.BodySlam, Move.Strength));

        private RentalPokémon(Species species, IEnumerable<Move> moves) : base(species, moves)
        {
            AvailablePokémon.Add(this);
        }

        public static RentalPokémon RandomRental()
        {
            switch (new Random().Next(5))
            {
                case 0:
                    return RentalPorygon;
                case 1:
                    return RentalCharizard;
                case 2:
                    return RentalBlastoise;
                case 3:
                    return RentalVenusaur;
                case 4:
                    return RentalGengar;
                default:
                    return RentalVenusaur;
            }
        }
    }
}
