using System.Collections.Generic;

namespace MantineLearning
{
    using System;

    /// <summary>
    /// Modified SARSA learning algorithm for machine learning under differing action restrictions.
    /// Inspiration taken from César and Kirillov's Accord.NET implementation.
    /// </summary>
    public class SARSA
    {
        /// <summary>
        /// Gets or sets the discount factor.
        /// </summary>
        /// <value>
        /// The discount factor; that is, the discount to apply to the expected reward.
        /// </value>
        public double DiscountFactor { get; set; } = 0.95;

        /// <summary>
        /// Gets or sets the learning rate.
        /// </summary>
        /// <value>
        /// The learning rate
        /// </value>
        public double LearningRate { get; set; } = 0.25;

        /// <summary>
        /// Maximum number of possible states.
        /// </summary>
        public long NumStates { get; private set; }

        /// <summary>
        /// Maximum number of possible actions
        /// </summary>
        public int NumActions { get; private set; }

        /// <summary>
        /// Gets or sets the exploration policy.
        /// </summary>
        /// <value>
        /// The exploration policy, i.e. epsilon-greedy
        /// </value>
        public IExplorationPolicy ExplorationPolicy { get; set; }
        
        /// <summary>
        /// Gets or sets the quality values.
        /// </summary>
        /// <value>
        /// The quality values.
        /// </value>
        public double[][] QualityValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SARSA"/> class.
        /// </summary>
        /// <param name="numStates">The max number of states.</param>
        /// <param name="numActions">The max number of actions.</param>
        /// <param name="explorationPolicy">The exploration policy.</param>
        /// <param name="randomize">if set to <c>true</c> [randomize] the created quality array.</param>
        public SARSA(long numStates, int numActions, IExplorationPolicy explorationPolicy, bool randomize=true)
        {
            NumStates = numStates;
            NumActions = numActions;
            ExplorationPolicy = explorationPolicy;

            //quality array
            QualityValues = new double[NumStates][];
            for (long i = 0; i < NumStates; i++)
            {
                QualityValues[i] = new double[NumActions];
            }

            // do randomization on the initial values
            if (randomize)
            {
                Random rand = new Random();

                for (long i = 0; i < NumStates; i++)
                {
                    for (long j = 0; j < NumActions; j++)
                    {
                        QualityValues[i][j] = rand.NextDouble() / 10;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the next recommended available action under the current exploration policy.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="availableActions">The available actions.</param>
        /// <returns>The chosen action</returns>
        public int GetAction(long state, List<int> availableActions)
        {
            return ExplorationPolicy.ChooseAction(QualityValues[state], availableActions);
        }

        /// <summary>
        /// Updates the quality values associated with the last state and action.
        /// </summary>
        /// <param name="previousState">The immediately prior state.</param>
        /// <param name="previousAction">The immediately prior action.</param>
        /// <param name="reward">The reward from choosing the previous action transitioning from the previous state to the next state.</param>
        /// <param name="nextState">The current state.</param>
        /// <param name="nextAction">The next action.</param>
        public void UpdateState(long previousState, long previousAction, double reward, long currentState, long nextAction)
        {
            QualityValues[previousState][previousAction] *= (1.0 - LearningRate);
            QualityValues[previousState][previousAction] += (LearningRate * (reward + DiscountFactor *
                                                           QualityValues[currentState][nextAction]));
        }

        /// <summary>
        /// Updates the quality values associated with the last state and action.
        /// </summary>
        /// <param name="previousState">The immediately prior state.</param>
        /// <param name="previousAction">The immediately prior action choice.</param>
        /// <param name="reward">The reward from choosing the previous action out of the previous state.</param>
        public void UpdateState(long previousState, long previousAction, double reward)
        {
            QualityValues[previousState][previousAction] *= (1 - LearningRate);
            QualityValues[previousState][previousAction] += (LearningRate * reward);
        }
    }
}