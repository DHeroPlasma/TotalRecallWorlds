using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using ZukiniFun.TotalAgentCore;
using ZukiniFun.TotalInputCore;

namespace ZukiniFun.TotalGameCore
{
    /*
     * This class is supposed to manage, store and provide links to agent and object selection by the player.
     */
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
        /// Store the currently hovered object to qualify it for eventual main character selection user input.
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
        /// Static link for other classes to react to agent selection.
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
            InputManagerTRC.MouseRaycastChanged += OnMouseHoverSetCurrentHover;
            InputManagerTRC.LeftClickInScenePressed += OnClickManageSelection;
            InputManagerTRC.LeftDoubleClickInScenePressed += OnDoubleclickSetMainCharacter;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            InputManagerTRC.MouseRaycastChanged -= OnMouseHoverSetCurrentHover;
            InputManagerTRC.LeftClickInScenePressed -= OnClickManageSelection;
            InputManagerTRC.LeftDoubleClickInScenePressed -= OnDoubleclickSetMainCharacter;
        }

        /// <summary>
        /// React to player mouse navigation to highlight currently hovered objects.
        /// </summary>
        /// <param name="next"></param>
        private void OnMouseHoverSetCurrentHover(IHoverableTotalRecall next)
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
        /// React to player input for setting the main character.
        /// </summary>
        private void OnDoubleclickSetMainCharacter()
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
        /// React to player input for objet selection in the scene.
        /// </summary>
        private void OnClickManageSelection()
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

                        AddOrRemoveAgentSelection(agent);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Invoke the selection logic depending on whether this agent is already selected or not.
        /// </summary>
        /// <param name="agent"></param>
        private void AddOrRemoveAgentSelection(SelectableAgent agent)
        {
            if (agent.IsSelected())
            {
                RemoveAgentSelection(agent);
            }
            else
            {
                ManageAgentSelection(agent);
            }
        }

        /// <summary>
        /// De-select this agent and remove other selections aswell if the player doesn't have multiselection active.
        /// </summary>
        /// <param name="agent"></param>
        private void RemoveAgentSelection(SelectableAgent agent)
        {
            InvokeSelectionForAgent(agent, false);

            bool isMultiSelectionPossible = InputManagerTRC.IsMultiSelectionPossible();
            if (AllowMultiSelectionAgents && !isMultiSelectionPossible)
            {
                _selectedAgents.ToList().ForEach(x => InvokeSelectionForAgent(x, false));
            }
        }

        /// <summary>
        /// Select agent when no other agent is selected or add this agent to agent collection while player activates multiselection.
        /// If multiselection is not active, remove other selections.
        /// </summary>
        /// <param name="agent"></param>
        private void ManageAgentSelection(SelectableAgent agent)
        {
            bool isMultiSelectionPossible = InputManagerTRC.IsMultiSelectionPossible();
            if (!isMultiSelectionPossible || (!AllowMultiSelectionAgents && _selectedAgents.Count > 0))
            {
                _selectedAgents.ToList().ForEach(x => InvokeSelectionForAgent(x, false));
            }

            if (_selectedAgents.Count > 0 && isMultiSelectionPossible)
            {
                if (AllowMultiSelectionAgents)
                {
                    InvokeSelectionForAgent(agent, true);
                }
            }

            if (_selectedAgents.Count == 0)
            {
                InvokeSelectionForAgent(agent, true);
            }
        }

        /// <summary>
        /// Invokes the agent selection routine for the passed instance and adds it to the collection of selected agents.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="agent"></param>
        private void InvokeSelectionForAgent(SelectableAgent agent, bool set)
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
