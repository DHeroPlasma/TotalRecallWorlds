using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ZukiniFun.TotalAgentHelpers;

namespace ZukiniFun.TotalAgentCore
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectableAgent : MonoBehaviour, ISelectableAgentLocomotion
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        protected InputActionReference[] inputActions;

        /// <summary>
        /// 
        /// </summary>
        public SelectionState SelectionState 
        { 
            get; 
            private set; 
        }

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
        private UnityAction<bool> PlayerSelectedAgent;

        /// <summary>
        /// 
        /// </summary>
        public UnityAction<SelectionState> AgentSelectionStateChanged;

        /// <summary>
        /// 
        /// </summary>
        public UnityAction<LocomotionState> AgentLocomotionStateChanged;

        #region MonoBehavior

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            AwakeOverride();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            StartOverride();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            UpdateOverride();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            DestroyOverride();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
           EnableOverride(); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            DisableOverride();
        }
        #endregion

        #region MonoBehavior Overrides

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AwakeOverride()
        {
            PlayerSelectedAgent += OnAgentSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void StartOverride()
        {
            LocomotionState = LocomotionState.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdateOverride()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DestroyOverride()
        {
            PlayerSelectedAgent -= OnAgentSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void EnableOverride()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisableOverride()
        {
        }

        #endregion

        #region Event Callbacks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected"></param>
        protected virtual void OnAgentSelected(bool selected)
        {
            ChangeSelectionState(selected ? SelectionState.Selected : SelectionState.Unselected);
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

        #region public namespace

        /// <summary>
        /// 
        /// </summary>
        public void SelectThisAgent()
        {
            PlayerSelectedAgent.Invoke(this);
        }

        #endregion

        #region protected namespace

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSelectionState"></param>
        protected virtual void ChangeSelectionState(SelectionState newSelectionState)
        {
            SelectionState = newSelectionState;
            AgentSelectionStateChanged.Invoke(newSelectionState);
        }

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
