using System;
using System.Linq;

namespace PokémonAPI
{

    public partial class Move
    {
        /// <summary>
        /// Gets the name of the move.
        /// </summary>
        /// <value>
        /// The name of the move.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the move.
        /// </summary>
        /// <value>
        /// The type of the move.
        /// </value>
        public Type AttackType { get; }

        /// <summary>
        /// Gets the category of the move (Physical, Special, Status)
        /// </summary>
        /// <value>
        /// The category of the move (Physical, Special, Status)
        /// </value>
        public MoveCategory Category { get; }

        /// <summary>
        /// Gets the power of the move.
        /// </summary>
        /// <value>
        /// The power of the move
        /// </value>
        public int AttackPower { get; }

        /// <summary>
        /// The primary effect of the move.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="defender">The defender.</param>
        public void UseAction(Pokémon user, Pokémon defender)
        {
            if (Category!=MoveCategory.Status)
                defender.Damage += DamageCalculation(user, defender);
            AdditionalEffects?.Invoke(user, defender);
        }

        public Action<Pokémon, Pokémon> AdditionalEffects { get; } = null;

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
            //STAB modifier (1.5 if this attack's type is one of the user's types; 1, otherwise).
            double X = user.Types.Contains(AttackType)?1.5:1;

            var temp = AttackType.AttackMultipliers();
            var temp2 = temp[Type.Grass];
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
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

}
