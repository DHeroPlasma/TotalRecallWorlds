using UnityEngine;
using ZukiniFun.TotalGameCore;
using ZukiniFun.TotalInputCore;
using static UnityEngine.Rendering.DebugUI;

namespace ZukiniFun.TotalAgentCore
{
    /*
     * Overrides agent class to add main character functionalities like specific outline behavior.
     */
    public class SelectableMainCharacter : SelectableAgent
    {
        private bool _isMainCharacter;
        /// <summary>
        /// 
        /// </summary>
        public bool IsMainCharacter
        {
            get
            {
                return _isMainCharacter;
            }
            private set
            {
                _isMainCharacter = value;
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        public void SetThisMainCharacter(bool set)
        {            
            IsMainCharacter = set;

            if (set)
            {
                SelectThisObject(false);
                SetLayerOutlineState(true,CollisionOutlineLayers.TotalRecallMC);
                SelectionState = SelectionState.Blocked;                
            }
            else
            {
                SelectionState = SelectionState.Unselected;
                SetLayerOutlineState(false, CollisionOutlineLayers.None);
                SelectThisObject(false);
            }
        }
    }
}
