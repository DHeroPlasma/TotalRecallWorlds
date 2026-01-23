using UnityEngine;

namespace ZukiniFun.TotalAgentCore
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectableMainCharacter : SelectableAgent
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void AwakeOverride()
        {
            CanMove = true;

            base.AwakeOverride();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDestinationInput"></param>
        public override void GoToDestination(Vector3 userDestinationInput)
        {
            base.GoToDestination(userDestinationInput);
        }
    }
}
