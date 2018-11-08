using System.Collections.Generic;

namespace MantineLearning
{
    /// <summary>
    /// Interface for policies for machine learning.
    /// </summary>
    public interface IExplorationPolicy
    {
        /// <summary>
        /// Chooses the next action under the policy from amongst the available actions (or any actions if availableActions is null)
        /// </summary>
        /// <param name="actionRewards">The expected reward for picking a given action.</param>
        /// <param name="availableActions">The available action to choose from.</param>
        /// <returns></returns>
        int ChooseAction(double[] actionRewards, List<int> availableActions = null);
    }
}