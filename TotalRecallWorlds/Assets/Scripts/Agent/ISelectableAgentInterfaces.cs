using UnityEngine;

namespace ZukiniFun.TotalAgentCore
{
    /// <summary>
    /// 
    /// </summary>
    public enum SelectionState
    {
        Selected = 0,
        Unselected = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LocomotionState
    {
        Idle = 0,
        GoingTo = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISelectableAgentLocomotion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDestinationInput"></param>
        public void GoToDestination(Vector3 userDestinationInput);
    }
}
