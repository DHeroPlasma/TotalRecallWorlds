using System;
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
            if (_potentialMc == null)
            {
                return;
            }

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

        /// <summary>
        /// React to player input for objet selection in the scene.
        /// </summary>
        private void OnClickManageSelection()
        {
            switch (_currentHover)
            {
                case SelectableMainCharacter mc:

                    if (!mc.IsMainCharacter)
                    {
                        OnAgentSelection(mc);
                    }
                    break;

                case SelectableAgent agent:

                    OnAgentSelection(agent);
                    break;

                default:

                    if (!InputManagerTRC.IsMultiSelectionPossible())
                    {
                        _selectedAgents.ToList().ForEach(x => InvokeSelectionForAgent(x, false));
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        private void OnAgentSelection(SelectableAgent agent)
        {
            if (!InputManagerTRC.IsMultiSelectionPossible())
            {
                // Create Context Interaction Menu
            }
            else
            {
                AddOrRemoveAgentSelection(agent);
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
                InvokeSelectionForAgent(agent, false);
            }
            else
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
