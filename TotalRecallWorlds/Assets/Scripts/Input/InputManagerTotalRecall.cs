using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ZukiniFun.TotalInputCore
{
    /// <summary>
    /// 
    /// </summary>
    public class InputManagerTotalRecall : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public InputActionAsset InputActions;

        /// <summary>
        /// 
        /// </summary>
        private InputActionMap _inputActionMap;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private Camera _mainCam;
        [SerializeField]
        private LayerMask hoverMask;
        [SerializeField]
        private float maxDistance = 500f;

        /// <summary>
        /// 
        /// </summary>
        private IHoverableTotalRecall _currentHover;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private InputActionReference LeftClickActionReference;
        private InputAction _leftClickAction;
        public static UnityAction LeftClickInScenePressed;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _leftClickAction = LeftClickActionReference.action;

            LeftClickInScenePressed += ManageSceneSelections;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }

            _inputActionMap = InputActions.FindActionMap("TotalRecallInput");
            if (_inputActionMap != null)
            {
                _inputActionMap.Enable();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            if (_inputActionMap != null)
            {
                _inputActionMap.Disable();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDestroy()
        {
            LeftClickInScenePressed -= ManageSceneSelections;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            SetHoverState();

            if (_leftClickAction.WasPressedThisFrame())
            {
                LeftClickInScenePressed.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FixedUpdate()
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {            
        }

        #region private namespace

        /// <summary>
        /// 
        /// </summary>
        private void SetHoverState()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                SetHover(null);
                return;
            }

            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, maxDistance, hoverMask, QueryTriggerInteraction.Ignore))
            {
                var hoverable = hit.collider.GetComponentInParent<IHoverableTotalRecall>();
                SetHover(hoverable);
            }
            else
            {
                SetHover(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        private void SetHover(IHoverableTotalRecall next)
        {
            if (_currentHover == next)
            {
                return;
            }

            _currentHover?.OnHoverExit();
            _currentHover = next;
            _currentHover?.OnHoverEnter();
        }


        private void ManageSceneSelections()
        {
            // Check if click-target is hovered, if so, add it to a hash set of collections.
        }

        #endregion
    }
}
