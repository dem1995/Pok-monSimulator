using System;
using System.IO;
using System.Linq;
using MantineLearning;
using PokémonAPI;
using Type = PokémonAPI.Type;

namespace PokémonSimulator
{
    internal class Program
    {
        private static int NumberOfBattles { get; } = 1000;
        private static double ExplorationRate { get; } = 0.5;
        private static double LearningRate { get; } = 0.5;

        private static void Main(string[] args)
        {
            /**Region for setting up SARSA function (and possibly parameters)**/

            #region SARSA Setup

            //Set up SARSA object
            var explorationPolicy = new EpsilonGreedyExploration(ExplorationRate);
            var numberOfStates = 15 * 15 * 15 * 15;
            var numberOfActions = Enum.GetValues(typeof(Type)).Length;
            var sarsa = new SARSA(numberOfStates, numberOfActions, explorationPolicy);

            //Prepare the state mapping
            Func<Pokémon, Pokémon, long> getState = (pokémon1, pokémon2) =>
            {
                var moveTypes = pokémon1.Moves.Select(t => t.AttackType).Distinct().ToList();

                return
                    15 * 15 * 15 * (long) pokémon1.Types[0] +
                    15 * 15 * (long) (pokémon1.Types.Count > 1 ? pokémon1.Types[1] : pokémon1.Types[0]) +
                    15 * (long) pokémon2.Types[0] +
                    1 * (long) (pokémon2.Types.Count > 1 ? pokémon2.Types[1] : pokémon2.Types[0]);
            };

            #endregion SARSA Setup

            using (var sw = new StreamWriter("PineappleExpress.txt"))
            {
                sw.Write("");
            }

            /**Region for setting up the battle itself**/

            #region Battle Execution

            //For the specified number of battles, perform battles and update the policy
            for (var battleNumber = 0; battleNumber < NumberOfBattles; battleNumber++)
            {
                // set exploration rate for this iteration
                explorationPolicy.ExplorationRate =
                    ExplorationRate - (double) battleNumber / NumberOfBattles * ExplorationRate;

                // set learning rate for this iteration
                sarsa.LearningRate = LearningRate - (double) battleNumber / NumberOfBattles * LearningRate;

                //Prepare the Pokémon
                Pokémon pokemon1 = RentalPokémon.RentalPorygon; //A pre-made Porygon
                Pokémon pokemon2 = RentalPokémon.RentalVenusaur; //A pre-made opponent

                long previousState = -1;
                var previousAction = -1;
                long currentState = -1;
                var nextAction = -1;

                var reward = 0.0;
                var firstTurn = true;

                double percentFinished = 0;

                //Battle loop
                while (!(pokemon1.IsFainted || pokemon2.IsFainted))
                {
                    //Shift states
                    currentState = getState(pokemon1, pokemon2);
                    var validTypes = pokemon1.Moves.Select(m => (int) m.AttackType).Distinct().ToList();
                    nextAction = sarsa.GetAction(currentState, validTypes);

                    //update SARSA
                    if (!firstTurn)
                        sarsa.UpdateState(previousState, previousAction, reward, currentState, nextAction);
                    else
                        firstTurn = false;

                    //Determine who moves first
                    var firstMover = pokemon1.Stats[Stat.Speed] > pokemon2.Stats[Stat.Speed] ? pokemon1 : pokemon2;

                    //Perform actions
                    if (pokemon1 == firstMover)
                    {
                        reward = pokemon1.UseMoveOfType((Type) nextAction, pokemon2);
                        Console.WriteLine("{0} (Pokémon 1) used a move of type {1}", pokemon1.Species.Name,
                            Enum.GetName(typeof(Type), (Type) nextAction));
                        Console.WriteLine("Did {0} damage. {1} (Pokémon 2) now has {2} health remaining)",
                            reward, pokemon2.Species.Name, pokemon2.RemainingHealth);
                        Console.WriteLine(((Type) nextAction).MultiplierOn(pokemon2.Types.ToArray()));
                        if (!pokemon2.IsFainted)
                            pokemon2.Use(new Random().Next(4), pokemon1);
                        else
                            reward += 20;
                    }
                    else
                    {
                        pokemon2.Use(new Random().Next(4), pokemon1);

                        //Console.WriteLine("{0} (Pokémon 2) used {1}", pokemon2.Species.Name, pokemon2.Moves[0].Name);
                        //Console.WriteLine("Did {0} damage. {1} (Pokémon 1) now has {2} health remaining)",
                        //    reward, pokemon1.Species.Name, pokemon1.RemainingHealth);

                        if (!pokemon1.IsFainted)
                        {
                            reward = pokemon1.UseMoveOfType((Type) nextAction, pokemon2);
                            Console.WriteLine("{0} (Pokémon 1) used a move of type {1}", pokemon1.Species.Name,
                                Enum.GetName(typeof(Type), (Type) nextAction));
                            Console.WriteLine("Did {0} damage. {1} (Pokémon 2) now has {2} health remaining)",
                                reward, pokemon2.Species.Name, pokemon2.RemainingHealth);
                            Console.WriteLine(((Type) nextAction).MultiplierOn(pokemon2.Types.ToArray()));
                        }
                    }

                    previousState = currentState;
                    previousAction = nextAction;
                    percentFinished = ((double) pokemon2.Stats[Stat.HP] - pokemon2.RemainingHealth) /
                                      pokemon2.Stats[Stat.HP];
                    Console.WriteLine($"{reward}");
                }

                sarsa.UpdateState(previousState, previousAction, reward, currentState, nextAction);

                if (pokemon1.IsFainted)
                    Console.WriteLine("{0} (Pokémon 1) Fainted", pokemon1.Species.Name);
                else
                    Console.WriteLine("{0} (Pokémon 2) Fainted", pokemon2.Species.Name);

                //Print score for graphing
                using (var sw = new StreamWriter($"PineappleExpress({ExplorationRate}_{LearningRate}).txt", true))
                {
                    sw.WriteLine("{0}, {1}", battleNumber, percentFinished);
                }
            }

            #endregion Battle Execution
        }
    }
}