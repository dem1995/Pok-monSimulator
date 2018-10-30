using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PokémonAPI;
using PokémonSimulator;
using MantineLearning;

namespace PokémonSimulator
{
    class Program
    {
        private static int NumberOfBattles { get; } = 1000;
        private static double ExplorationRate { get; set; } = 0.5;
        private static double LearningRate { get; set; }= 0.5;

        static void Main(string[] args)
        {

            /**Region for setting up SARSA function (and possibly parameters)**/
            #region SARSA Setup

            //Set up SARSA object
            var explorationPolicy = new EpsilonGreedyExploration(ExplorationRate);
            int numberOfStates = 15 * 15;
            int numberOfActions = 4;
            SARSA sarsa = new SARSA(numberOfStates, numberOfActions, explorationPolicy);

            //Prepare the state mapping
            Func<Pokémon, Pokémon, int> getState = (Pokémon pokémon1, Pokémon pokémon2) =>
            {

                return 15 * (int) pokémon2.Types[0] +
                    1 * (int) (pokémon2.Types.Count > 1 ? pokémon2.Types[1] : pokémon2.Types[0]);
            };


            #endregion SARSA Setup

            using (StreamWriter sw = new StreamWriter("PineappleExpress.txt"))
            {
                sw.Write("");
            }

            /**Region for setting up the battle itself**/
            #region Battle Execution

            //For the specified number of battles, perform battles and update the policy
            for (int battleNumber = 0; battleNumber < NumberOfBattles; battleNumber++)
            {
                // set exploration rate for this iteration
                explorationPolicy.Epsilon = ExplorationRate - ((double)battleNumber / NumberOfBattles) * ExplorationRate;

                // set learning rate for this iteration
                sarsa.LearningRate = LearningRate - ((double)battleNumber/ NumberOfBattles) * LearningRate;

                //Prepare the Pokémon
                Pokémon pokemon1 = RentalPokémon.RentalGengar; //A pre-made Porygon
                Pokémon pokemon2 = RentalPokémon.RandomRental();    //A pre-made opponent

                int previousState=-1;
                int previousAction=-1;
                int currentState=-1;
                int nextAction=-1;
                double reward = 0.0;
                bool firstTurn = true;

                double percentFinished = 0;

                //Battle loop
                while (!(pokemon1.IsFainted || pokemon2.IsFainted))
                {

                    //Shift states
                    currentState = getState(pokemon1, pokemon2);
                    nextAction = sarsa.GetAction(currentState);

                    //update SARSA
                    if (!firstTurn)
                    {
                        sarsa.UpdateState(previousState, previousAction, reward, currentState, nextAction);
                    }
                    else
                        firstTurn = false;

                    //Determine who moves first
                    Pokémon firstMover = pokemon1.Stats[Stat.Speed] > pokemon2.Stats[Stat.Speed] ? pokemon1: pokemon2;

                    //Perform actions
                    if (pokemon1 == firstMover)
                    {
                        reward = pokemon1.Use(nextAction, pokemon2);
                        Console.WriteLine("{0} (Pokémon 1) used {1}", pokemon1.Species.Name, pokemon1.Moves[nextAction].Name);
                        Console.WriteLine("Did {0} damage. {1} (Pokémon 2) now has {2} health remaining)",
                            reward, pokemon2.Species.Name, pokemon2.RemainingHealth);
                        if (!pokemon2.IsFainted)
                        {
                            pokemon2.Use(0, pokemon1);
                            Console.WriteLine("{0} (Pokémon 2) used {1}", pokemon2.Species.Name, pokemon2.Moves[0].Name);
                            Console.WriteLine("Did {0} damage. {1} (Pokémon 1) now has {2} health remaining)",
                                reward, pokemon1.Species.Name, pokemon1.RemainingHealth);
                        }
                        else
                            reward += 1000;
                    }
                    else
                    {
                        pokemon2.Use(0, pokemon1);

                        Console.WriteLine("{0} (Pokémon 2) used {1}", pokemon2.Species.Name, pokemon2.Moves[0].Name);
                        Console.WriteLine("Did {0} damage. {1} (Pokémon 1) now has {2} health remaining)",
                            reward, pokemon1.Species.Name, pokemon1.RemainingHealth);

                        if (!pokemon1.IsFainted)
                        {
                            reward = pokemon1.Use(nextAction, pokemon2);
                            Console.WriteLine("{0} (Pokémon 1) used {1}", pokemon1.Species.Name, pokemon1.Moves[nextAction].Name);
                            Console.WriteLine("Did {0} damage. {1} (Pokémon 2) now has {2} health remaining)",
                                reward, pokemon2.Species.Name, pokemon2.RemainingHealth);
                        }
                    }

                    previousState = currentState;
                    previousAction = nextAction;
                    percentFinished = ((double)pokemon2.Stats[Stat.HP]-pokemon2.RemainingHealth)/((double)pokemon2.Stats[Stat.HP]);
                }

                sarsa.UpdateState(previousState,previousAction,reward,currentState,nextAction);

                if (pokemon1.IsFainted)
                    Console.WriteLine("{0} (Pokémon 1) Fainted", pokemon1.Species.Name);
                else
                    Console.WriteLine("{0} (Pokémon 2) Fainted", pokemon2.Species.Name);

                //Print score for graphing
                using (StreamWriter sw = new StreamWriter("PineappleExpress.txt", true))
                {

                     sw.WriteLine("{0}, {1}", battleNumber, percentFinished);

                }
            }

            #endregion Battle Execution
        }
    }
}
