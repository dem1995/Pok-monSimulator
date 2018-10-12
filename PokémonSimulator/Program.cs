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
            //A pre-made Porygon
            Pokémon pokemon1 = RentalPokémon.RentalPorygon;

            //A pre-made Venusaur
            Pokémon pokemon2 = RentalPokémon.RentalVenusaur;

            //An example of Porygon attacking Venusaur
            int damageDealt = pokemon1.Use(1, pokemon2);

            //Checking remaining health
            int p1Remaining = pokemon1.RemainingHealth;
            int p2Remaining = pokemon2.RemainingHealth;

            //Testing to see if Pokémon has fainted
            bool hasFainted = pokemon2.IsFainted;

        }
    }
}
