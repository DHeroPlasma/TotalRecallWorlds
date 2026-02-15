using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using ZukiniFun.TotalAgentCore;

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
        public bool AllowMultiSelectionAgents
        {
            get; 
            private set; 
        }

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
        private HashSet<SelectableAgent> _selectableAgents;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private InputActionReference LeftClickActionReference;
        private InputAction _leftClickAction;
        public static UnityAction LeftClickInScenePressed;

        [SerializeField]
        private InputActionReference ShiftPressActionReference;
        private InputAction _shiftPressedAction;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _selectableAgents = new HashSet<SelectableAgent>();
            _leftClickAction = LeftClickActionReference.action;
            _shiftPressedAction = ShiftPressActionReference.action;

            LeftClickInScenePressed += ManageSceneSelections;

            AllowMultiSelectionAgents = true;
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
            SetSceneHoverState();

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
        private void SetSceneHoverState()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                SetCurrentHover(null);
                return;
            }

            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, maxDistance, hoverMask, QueryTriggerInteraction.Ignore))
            {
                var hoverable = hit.collider.GetComponentInParent<IHoverableTotalRecall>();
                SetCurrentHover(hoverable);
            }
            else
            {
                SetCurrentHover(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        private void SetCurrentHover(IHoverableTotalRecall next)
        {
            if (_currentHover == next)
            {
                return;
            }

            SelectableAgent agent = next as SelectableAgent;
            if (agent != null && agent.IsSelectable())
            {
                return;
            }

            _currentHover?.OnHoverExit();
            _currentHover = next;
            _currentHover?.OnHoverEnter();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ManageSceneSelections()
        {
            if (_currentHover != null)
            {
                switch (_currentHover)
                {
                    case SelectableAgent agent:

                        SetAgentsSelections(agent);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        private void SetAgentsSelections(SelectableAgent agent)
        {
            if (agent.IsSelected())
            {
                RemoveSelection(agent);
            }
            else
            {
                AddSelection(agent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        private void RemoveSelection(SelectableAgent agent)
        {
            ManageAgentSelection(agent, false);
            _currentHover = null;

            if (AllowMultiSelectionAgents && !_shiftPressedAction.IsPressed())
            {
                RemoveOtherSelections();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        private void AddSelection(SelectableAgent agent)
        {
            if (!_shiftPressedAction.IsPressed() || (!AllowMultiSelectionAgents && _selectableAgents.Count > 0))
            {
                RemoveOtherSelections();
            }

            if (_selectableAgents.Count > 0 && _shiftPressedAction.IsPressed())
            {
                if (AllowMultiSelectionAgents)
                {
                    ManageAgentSelection(agent, true);
                }
            }

            if (_selectableAgents.Count == 0)
            {
                ManageAgentSelection(agent, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveOtherSelections()
        {
            _selectableAgents.ToList().ForEach(x =>
            {
                ManageAgentSelection(x, false);
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="agent"></param>
        private void ManageAgentSelection(SelectableAgent agent, bool set)
        {
            if (set)
            {
                agent.SelectThisObject(true);
                _selectableAgents.Add(agent);
            }
            else
            {
                agent.SelectThisObject(false);
                _selectableAgents.Remove(agent);
            }
        }

        #endregion
    }
}
