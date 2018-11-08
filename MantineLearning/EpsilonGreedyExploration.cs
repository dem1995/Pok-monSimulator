using System;
using System.Collections.Generic;

namespace MantineLearning
{

    /// <summary>
    /// An epsilon-greedy exploration policy. The exploration rate, <see cref="ExplorationRate"/> defines the chance that a
    /// random choice will be taken (for the sake of exploration)
    /// </summary>
    /// <seealso cref="MantineLearning.IExplorationPolicy" />
    public class EpsilonGreedyExploration : IExplorationPolicy
    {
        /// <summary>
        /// A random number generator for this instance of the <see cref="EpsilonGreedyExploration"/> class.
        /// </summary>
        private readonly Random _randy = new Random();

        /// <summary>
        /// Retrieves the exploration rate; that is, the chance of a random action being taken.
        /// </summary>
        /// <value>
        /// The exploration rate; that is, the chance of a random action being taken.
        /// </value>
        public double ExplorationRate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpsilonGreedyExploration"/> class.
        /// </summary>
        /// <param name="explorationRate">The exploration rate.</param>
        public EpsilonGreedyExploration(double explorationRate)
        {
            ExplorationRate=explorationRate;
        }

        /// <summary>
        /// Chooses the next action under an epsilon-greedy policy from amongst the available actions (or any actions if availableActions is null)
        /// </summary>
        /// <param name="actionRewards">The expected reward for picking a given action.</param>
        /// <param name="availableActions">The available action to choose from.</param>
        /// <returns></returns>
        public int ChooseAction(double[] actionRewards, List<int> availableActions = null)
        {

            double maxReward = -1;
            int greedyAction = 0;

            for (int i = 0; i < actionRewards.Length; i++)
            {
                if (availableActions?.Contains(i)??true && actionRewards[i] > maxReward)
                {
                    maxReward = actionRewards[i];
                    greedyAction = i;
                }
            }

            //If we're set to explore, pick a random action from amongst the available ones
            if (_randy.NextDouble() < ExplorationRate)
            {
                return ChooseRandomFrom(availableActions, _randy);
            }

            return greedyAction;
        }

        /// <summary>
        /// Chooses a random integer from amongst the list of choices.
        /// </summary>
        /// <param name="choices">The choices.</param>
        /// <param name="rand">The random number generator to use.</param>
        /// <returns>A random choice from the list of integers.</returns>
        public static int ChooseRandomFrom(List<int> choices, Random rand)
        {
            return choices[rand.Next(choices.Count)];
        }
    }
}

