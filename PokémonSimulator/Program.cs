using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokémonAPI;
using PokémonSimulator;

namespace PokémonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Pokémon p0 = RentalPokémon.RentalPor23;
            Pokémon p1 = RentalPokémon.RentalPorygon;
            Pokémon p2 = RentalPokémon.RentalVenusaur;
            Console.Write(Move.Thunderbolt.DamageCalculation(p1, p2));
            
        }
    }
}
