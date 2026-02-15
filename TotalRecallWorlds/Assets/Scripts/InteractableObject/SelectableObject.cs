using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ZukiniFun.TotalAgentCore;
using ZukiniFun.TotalInputCore;

namespace ZukiniFun.TotalInputCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SelectableObject : MonoBehaviour, IHoverableTotalRecall
    {       
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
        private bool _isHovered
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _hoverCooldownActive
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Renderer[] _objectRenderers;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private const float HOVERCOOLDOWN = 0.25f;


        /// <summary>
        /// 
        /// </summary>
        protected UnityAction<bool> PlayerSelectedObjectInternal;

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

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AwakeOverride()
        {
            PlayerSelectedObjectInternal += OnObjectSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void StartOverride()
        {
            _objectRenderers = GetComponentsInChildren<Renderer>();
            OnHoverExit();
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
            PlayerSelectedObjectInternal -= OnObjectSelected;
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
        protected virtual void OnObjectSelected(bool selected)
        {
            ChangeObjectSelectionState(selected ? SelectionState.Selected : SelectionState.Unselected);
        }

        #endregion

        #region IHoverable Implementation

        /// <summary>
        /// 
        /// </summary>
        public void OnHoverEnter()
        {
            if (SelectionState.Equals(SelectionState.Selected) || _hoverCooldownActive)
            {
                return;
            }

            SetOutlineState(true);
            _isHovered = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnHoverExit()
        {
            if (SelectionState.Equals(SelectionState.Selected))
            {
                return;
            }

            SetOutlineState(false);
            _isHovered = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsHovered()
        {
            return _isHovered;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSelected()
        {
            return SelectionState.Equals(SelectionState.Selected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable()
        {
            return _hoverCooldownActive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator InvokeHoverCooldown()
        {
            _hoverCooldownActive = true;
            yield return new WaitForSeconds(HOVERCOOLDOWN);
            _hoverCooldownActive = false;
        }

        #endregion

        #region public namespace

        /// <summary>
        /// 
        /// </summary>
        public void SelectThisObject(bool set)
        {
            if (!set)
            {
                StartCoroutine(InvokeHoverCooldown());
            }

            PlayerSelectedObjectInternal.Invoke(set);
        }

        #endregion

        #region protected namespace

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSelectionState"></param>
        protected virtual void ChangeObjectSelectionState(SelectionState newSelectionState)
        {
            if (newSelectionState.Equals(SelectionState.Unselected))
            {
                SetOutlineState(false);
            }

            SelectionState = newSelectionState;
        }

        #endregion

        #region private namespace

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        private void SetOutlineState(bool set)
        {
            if (_objectRenderers == null || _objectRenderers.Length == 0)
            {
                return;
            }

            foreach (var item in _objectRenderers)
            {
                item.gameObject.layer = set ? LayerMask.NameToLayer("TotalRecallOutline") : LayerMask.NameToLayer("TotalRecallHoverable");
            }
        }

        #endregion
    }
}
