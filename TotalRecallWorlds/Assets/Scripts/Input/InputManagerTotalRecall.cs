using UnityEngine;
using UnityEngine.Events;
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
        private InputActionReference LeftClickActionReference;
        private InputAction _leftClickAction;
        public static UnityAction LeftClickInScenePressed;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _leftClickAction = LeftClickActionReference.action;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            _inputActionMap = InputActions.FindActionMap("SceneInteraction");
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
        private void Update()
        {
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
    }
}
