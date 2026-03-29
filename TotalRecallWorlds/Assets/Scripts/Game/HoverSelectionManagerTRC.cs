using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using ZukiniFun.TotalAgentCore;
using ZukiniFun.TotalInputCore;

namespace ZukiniFun.TotalGameCore
{
    /// <summary>
    /// 
    /// </summary>
    public class HoverSelectionManagerTRC : MonoBehaviour
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
        private IHoverableTotalRecall _currentHover;

        /// <summary>
        /// 
        /// </summary>
        private IHoverableTotalRecall _potentialMc;

        /// <summary>
        /// 
        /// </summary>
        private HashSet<SelectableAgent> _selectedAgents;

        /// <summary>
        /// 
        /// </summary>
        private SelectableMainCharacter _selectedMainCharacter;

        /// <summary>
        /// 
        /// </summary>
        public static UnityAction<bool, SelectableObject[]> AgentSelected;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _selectedAgents = new HashSet<SelectableAgent>();
            AllowMultiSelectionAgents = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            InputManagerTRC.MouseRaycastChanged += SetCurrentHover;
            InputManagerTRC.LeftClickInScenePressed += ManageSceneSelections;
            InputManagerTRC.LeftDoubleClickInScenePressed += SetMainCharacter;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            InputManagerTRC.MouseRaycastChanged -= SetCurrentHover;
            InputManagerTRC.LeftClickInScenePressed -= ManageSceneSelections;
            InputManagerTRC.LeftDoubleClickInScenePressed -= SetMainCharacter;
        }

        /// <summary>
        /// Das hier muss aufs RayCast-Event von InputManager hören.
        /// </summary>
        /// <param name="next"></param>
        private void SetCurrentHover(IHoverableTotalRecall next)
        {
            _potentialMc = next;

            if (_currentHover == next)
            {
                return;
            }

            SelectableAgent agent = next as SelectableAgent;
            if (agent != null && agent.IsSelectable())
            {
                return;
            }

            _currentHover?.HoverExit();
            _currentHover = next;
            _currentHover?.HoverEnter();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMainCharacter()
        {
            if (_potentialMc != null)
            {
                switch (_potentialMc)
                {
                    case SelectableMainCharacter mc:

                        if (_selectedMainCharacter == mc)
                        {
                            _selectedMainCharacter?.SetThisMainCharacter(false);
                            _selectedMainCharacter = null;
                            return;
                        }
                        else
                        {
                            _selectedMainCharacter?.SetThisMainCharacter(false);
                            mc.SetThisMainCharacter(true);
                            _selectedMainCharacter = mc;
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ManageSceneSelections()
        {
            if (_currentHover != null)
            {
                if (_currentHover is SelectableMainCharacter mc && mc.IsMainCharacter)
                {
                    return;
                }

                switch (_currentHover)
                {
                    case SelectableAgent agent:

                        SetAgentSelection(agent);
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
        private void SetAgentSelection(SelectableAgent agent)
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

            bool isMultiSelectionPossible = InputManagerTRC.IsMultiSelectionPossible();
            if (AllowMultiSelectionAgents && !isMultiSelectionPossible)
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
            bool isMultiSelectionPossible = InputManagerTRC.IsMultiSelectionPossible();
            if (!isMultiSelectionPossible || (!AllowMultiSelectionAgents && _selectedAgents.Count > 0))
            {
                RemoveOtherSelections();
            }

            if (_selectedAgents.Count > 0 && isMultiSelectionPossible)
            {
                if (AllowMultiSelectionAgents)
                {
                    ManageAgentSelection(agent, true);
                }
            }

            if (_selectedAgents.Count == 0)
            {
                ManageAgentSelection(agent, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveOtherSelections()
        {
            _selectedAgents.ToList().ForEach(x =>
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
                AgentSelected.Invoke(true, new SelectableAgent[] { agent });
                _selectedAgents.Add(agent);
            }
            else
            {
                AgentSelected.Invoke(false, new SelectableAgent[] { agent });
                _selectedAgents.Remove(agent);
            }
        }

    }
}
