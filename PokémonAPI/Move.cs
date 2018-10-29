using System;
using System.Linq;

namespace PokémonAPI
{
    /// <summary>
    /// The skills used by the Pokémon in battle (https://bulbapedia.bulbagarden.net/wiki/Move).
    /// </summary>
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
        public void Use(Pokémon user, Pokémon defender)
        {
            if (Category!=MoveCategory.Status)
                defender.Damage += CalculateDamage(user, defender);
            AdditionalEffects?.Invoke(user, defender);
        }

        /// <summary>
        /// Gets the additional effects the move may have.
        /// </summary>
        /// <value>
        /// The additional effects the move may have (i.e. pretty much anything that isn't straightforward pure damage move effects)
        /// </value>
        public Action<Pokémon, Pokémon> AdditionalEffects { get; } = null;

        /// <summary>
        /// Calculates the raw damage, assuming this move has no additional effects.
        /// </summary>
        /// <param name="user">The Pokémon using this move.</param>
        /// <param name="defender">The Pokémon defending against this move.</param>
        /// <returns></returns>
        public int CalculateDamage (Pokémon user, Pokémon defender)
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

        /// <summary>
        /// Generates the additional move effect expected from a move that can conditionally cause a non-volatile status
        /// </summary>
        /// <param name="probability">The probability the move will cause the given non-volatile status.</param>
        /// <returns></returns>
        public static Action<Pokémon, Pokémon> StatusCauserAction(StatusCondition condition, decimal probability, Type? immuneType = null)
        {
            return (Pokémon attacker, Pokémon defender) =>
            {
                if (defender.Status == StatusCondition.None)
                    if (immuneType==null || !defender.Types.Contains((Type) immuneType))
                        if (new Random().NextDouble() < (double) probability)
                            defender.Status = condition;
            };
        }
    }

    /// <summary>
    /// The category of the move (https://bulbapedia.bulbagarden.net/wiki/Damage_category).
    /// This determines which stat is used for attack/defense (i.e., Attack/Special or Defense/Special),
    /// as well as whether the move does damage at all (i.e., Status moves do not).
    /// </summary>
    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

}
