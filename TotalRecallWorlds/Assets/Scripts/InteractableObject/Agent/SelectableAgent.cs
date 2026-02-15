using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ZukiniFun.TotalAgentHelpers;
using ZukiniFun.TotalInputCore;

namespace ZukiniFun.TotalAgentCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SelectableAgent : SelectableObject, ISelectableAgentLocomotion
    { 
        /// <summary>
        /// 
        /// </summary>
        public LocomotionState LocomotionState
        {
            get; 
            private set; 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanMove
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public UnityAction<SelectableAgent, SelectionState> AgentSelectionStateChanged;

        /// <summary>
        /// 
        /// </summary>
        public UnityAction<LocomotionState> AgentLocomotionStateChanged;

        #region MonoBehavior Overrides

        /// <summary>
        /// 
        /// </summary>
        protected override void AwakeOverride()
        {
            base.AwakeOverride();
            LocomotionState = LocomotionState.Idle;
        }

        #endregion

        #region ISelectableAgentLocomotion Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDestinationInput"></param>
        public virtual void GoToDestination(Vector3 userDestinationInput)
        {
            ChangeLocomotionState(LocomotionState.GoingTo);
        }

        #endregion

        #region IHoverableTotalRecall Override

        protected override void ChangeObjectSelectionState(SelectionState newSelectionState)
        {
            base.ChangeObjectSelectionState(newSelectionState);
            //AgentSelectionStateChanged.Invoke(this, newSelectionState);
        }

        #endregion

        #region protected namespace

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocomotionState"></param>
        protected virtual void ChangeLocomotionState(LocomotionState newLocomotionState)
        {
            LocomotionState = newLocomotionState;
            AgentLocomotionStateChanged.Invoke(newLocomotionState);
            RefreshAgentCapsuleCollider();
        }

        #endregion

        #region private namespace

        /// <summary>
        /// 
        /// </summary>
        private void RefreshAgentCapsuleCollider()
        {
            AutoCapsuleFromRenderer autoCapsuleFunctionality = GetComponent<AutoCapsuleFromRenderer>();
            if (autoCapsuleFunctionality != null)
            {
                autoCapsuleFunctionality.FitCollider();
            }
        }

        #endregion
    }
}
