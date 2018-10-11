using System;
using System.Collections.Generic;
using System.Text;

namespace PokémonSimulator
{

    public class Species
    {
        public static Species Bulbasaur { get; } = new Species("Bulbasaur", Type.Grass, Type.Poison, StatsGen(262, 165, 163, 197, 157));
        public static Species Ivysaur { get; } = new Species("Ivysaur", Type.Grass, Type.Poison, StatsGen(262, 165, 163, 197, 157));
        public static Species Venusaur { get; } = new Species("Venusaur", Type.Grass, Type.Poison, StatsGen(262, 165, 163, 197, 157));
        public static Species Charmander { get; } = new Species("Charmander", Type.Fire, StatsGen(250, 171, 151, 167, 197));
        public static Species Charmeleon { get; } = new Species("Charmeleon", Type.Fire, StatsGen(280, 185, 175, 189, 219));
        public static Species Charizard { get; } = new Species("Charizard", Type.Fire, Type.Flying, StatsGen(306, 213, 201, 215, 245));
        public static Species Squirtle { get; } = new Species("Squirtle", Type.Water, StatsGen(260, 163, 195, 167, 153));
        public static Species Wartortle { get; } = new Species("Wartortle", Type.Water, StatsGen(282, 183, 219, 189, 175));
        public static Species Blastoise { get; } = new Species("Blastoise", Type.Water, StatsGen(308, 211, 245, 215, 201));

        private Species(string name, Type type1, Dictionary<Stat, int> baseStats) : this(name, new[] {type1}, baseStats)
        {
        }

        private Species(string name, Type type1, Type type2, Dictionary<Stat, int> baseStats) : this(name, new[] {type1, type2}, baseStats)
        {
        }

        private Species(string name, IEnumerable<Type> types, Dictionary<Stat, int> baseStats)
        {
            Name = name;
            Types = new List<Type>();
            BaseStats = baseStats;

            AllSpecies.Add(this);
        }


        private static Dictionary<Stat, int> StatsGen(int hp, int attack, int defense, int special, int speed) =>
            new Dictionary<Stat, int>
            {
                { Stat.HP, hp },
                { Stat.Attack, attack },
                { Stat.Defense, defense },
                { Stat.Special, special },
                { Stat.Speed, speed }
            };
        

        public string Name { get; }

        public List<Type> Types { get; }     

        public Dictionary<Stat, int> BaseStats { get; }

        public List<Species> AllSpecies { get; set; }
    }
}
