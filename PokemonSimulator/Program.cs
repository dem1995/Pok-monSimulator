using System;

namespace PokémonAPI
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Type.Ghost.AttackMultipliers()[Type.Ghost]);
            Console.WriteLine(Type.Ghost.AttackMultipliers()[Type.Normal]);
            Console.WriteLine(Type.Ghost.AttackMultipliers()[Type.Ground]);
            Console.WriteLine(Type.Ghost.DefenseMultipliers()[Type.Ghost]);
            Console.WriteLine(Type.Ghost.DefenseMultipliers()[Type.Ground]);           
        }
    }
}

