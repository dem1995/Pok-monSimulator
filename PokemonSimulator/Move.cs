using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Reflection.Metadata;

namespace PokémonSimulator
{
    public class Move
    {
        public int AttackPower;

        public Type AttackType;

        public MoveCategory Category;


        public Action<Pokémon, Pokémon> PrimaryAction = (Pokémon user, Pokémon defender) => { };

        public int DamageCalculation (Pokémon user, Pokémon defender)
        {
            //Attacker's Level
            int A = user.Level;
            //Attacker's Attack or Special stat
            int B;
            //Attack's power
            int C = AttackPower;
            //Defender's Defense or Special stat
            int D;
            //STAB modifier (1.5 if this attack's type is one of the user's types; 0, otherwise).
            double X = user.Types.Contains(AttackType)?1.5:1;
            //Type effectiveness modifier. Product of the type effectiveness modifiers of this move's type against each of the defender's types.
            double Y = defender.Types.Aggregate
            (
                seed: 1.0,
                func: (result, item) => result * AttackType.AttackMultipliers()[item],
                resultSelector: result=>result
            );
            //Random number between 217 and 255
            int Z = new Random().Next(217, 256);

            switch (Category)
            {
                case MoveCategory.Physical:
                    B = user.Stats[Stat.Attack];
                    D = defender.Stats[Stat.Defense];
                    break;
                case MoveCategory.Special:
                    B = user.Stats[Stat.Special];
                    D = user.Stats[Stat.Special];
                    break;
                default:
                    B = 0;
                    D = int.MaxValue;
                    break;
            }

            double damage = 2 * A / 5.0 + 2;
            damage *= B;
            damage *= C;
            damage /= D;
            damage = damage / 50 + 2;
            damage *= X;
            damage *= Y;
            damage *= Z;
            damage /= 255.0;

            return (int) damage;
        }

        public Func<Pokémon, Pokémon> AdditionalEffects;
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

}
