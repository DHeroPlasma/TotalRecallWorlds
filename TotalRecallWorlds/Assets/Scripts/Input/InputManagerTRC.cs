using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using ZukiniFun.TotalAgentCore;

namespace ZukiniFun.TotalInputCore
{
    /*
     * Use this class to create interfaces to action map events.
     */
    public class InputManagerTRC : MonoBehaviour
    {
        /// <summary>
        /// Link to the project's action asset.
        /// </summary>
        public InputActionAsset InputActions;

        /// <summary>
        /// Link to a specific action map.
        /// </summary>
        private InputActionMap _inputActionMap;

        /// <summary>
        /// Link to the main camera for player-centric raycasting.
        /// </summary>
        [SerializeField]
        private Camera _mainCam;
        [SerializeField]
        private LayerMask hoverMask;
        [SerializeField]
        private float maxDistance = 500f;

        /// <summary>
        /// Stores the currently intersected agent or object from the player's point of view.
        /// </summary>
        private static IHoverableTotalRecall _currentRaycastHit;
        public static UnityAction<IHoverableTotalRecall> MouseRaycastChanged;

        /// <summary>
        /// Reference for doubleclick target comparison.
        /// </summary>
        private IHoverableTotalRecall _firstKlickObject;

        /// <summary>
        /// Action reference interface for simple left clicks.
        /// </summary>
        [SerializeField]
        private InputActionReference LeftClickActionReference;
        private InputAction _leftClickAction;
        public static UnityAction LeftClickInScenePressed;

        /// <summary>
        /// Action reference interface for double left clicks.
        /// </summary>
        [SerializeField]
        private InputActionReference LeftDoubleClickActionReference;
        private InputAction _leftDoubleClickAction;
        public static UnityAction LeftDoubleClickInScenePressed;

        /// <summary>
        /// Action reference interface for simple right clicks.
        /// </summary>
        [SerializeField]
        private InputActionReference RightClickActionReference;
        private InputAction _rightClickAction;
        public static UnityAction RightClickInScenePressed;

        /// <summary>
        /// Action reference interface for multi selection.
        /// </summary>
        [SerializeField]
        private InputActionReference ShiftPressActionReference;
        private InputAction _shiftPressedAction;
        private static bool IsShiftPressedFlag;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _leftClickAction = LeftClickActionReference.action;
            _leftDoubleClickAction = LeftDoubleClickActionReference.action;
            _rightClickAction = RightClickActionReference.action;
            _shiftPressedAction = ShiftPressActionReference.action;
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

            _leftDoubleClickAction.started += FirstClick;
            _leftDoubleClickAction.performed += SecondClick;
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
            _leftDoubleClickAction.started -= FirstClick;
            _leftDoubleClickAction.performed -= SecondClick;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            SetSceneHoverState();

            if (_leftClickAction.WasPressedThisFrame())
            {
                LeftClickInScenePressed.Invoke();
            }

            if (_rightClickAction.WasPressedThisFrame())
            {
                //RightClickInScenePressed.Invoke();
            }

            IsShiftPressedFlag = _shiftPressedAction.IsPressed();
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

        #region public static namespace

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsMultiSelectionPossible()
        {
            return IsShiftPressedFlag;
        }

        #endregion

        #region private namespace

        /// <summary>
        /// Store the clicked object for comparison.
        /// </summary>
        /// <param name="context"></param>
        private void FirstClick(InputAction.CallbackContext context)
        {
            if (_currentRaycastHit == null)
            {
                _firstKlickObject = null;
                return;
            }

            _firstKlickObject = _currentRaycastHit;
        }

        /// <summary>
        /// Event is only invoked if the first click hit the same object as the second one.
        /// </summary>
        /// <param name="context"></param>
        private void SecondClick(InputAction.CallbackContext context)
        {
            if (_currentRaycastHit == null)
            {
                return;
            }

            if (_currentRaycastHit == _firstKlickObject)
            {
                LeftDoubleClickInScenePressed.Invoke();
            }
        }

        /// <summary>
        /// Detect which object is intersected by the ray sent from the players pov mouse position and pass it to the link event.
        /// </summary>
        private void SetSceneHoverState()
        {
            IHoverableTotalRecall previousHover = _currentRaycastHit;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                _currentRaycastHit = null;
                return;
            }

            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, maxDistance, hoverMask, QueryTriggerInteraction.Ignore))
            {
                var hoverable = hit.collider.GetComponentInParent<IHoverableTotalRecall>();
                _currentRaycastHit = hoverable;
            }
            else
            {
                _currentRaycastHit = null;
            }

            if (previousHover != _currentRaycastHit)
            {                
                MouseRaycastChanged.Invoke(_currentRaycastHit);
            }
        }


        #endregion

        #region public namespace

        /// <summary>
        /// Check whether this object is currently intersected by the raycast sent from the player's pov mouse position.
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool IsThisObjectHovered(IHoverableTotalRecall check)
        {
            return _currentRaycastHit == check;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyObjectHovered()
        {
            return _currentRaycastHit != null;
        }

        #endregion
    }
}
