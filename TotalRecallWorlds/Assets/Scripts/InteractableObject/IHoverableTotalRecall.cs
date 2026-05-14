using System.Collections;
using UnityEngine;
namespace ZukiniFun.TotalInputCore
{
    /// <summary>
    /// Possible selection states, some agents or objects cannot be selected like current main characters.
    /// </summary>
    public enum SelectionState
    {
        Unselected = 0,
        Selected = 1,
        Blocked = 2
    }

    /*
     * Specifies which functionalities a hoverable 3D-object in the scene offers.
     */
    public interface IHoverableTotalRecall
    {
        /// <summary>
        /// Execute custom code when the player hovers the instance via mouse position.
        /// </summary>
        public void HoverEnter();

        /// <summary>
        /// Execute custom code when the player hover leaves the instance via mouse position.
        /// </summary>
        public void HoverExit();

        /// <summary>
        /// Return whether the instance is hovered by the player via mouse position.
        /// </summary>
        public bool IsHovered();

        /// <summary>
        /// Return whether the instance is in selected state.
        /// </summary>
        /// <returns></returns>
        public bool IsSelected();

        /// <summary>
        /// Return whether the instance can be selected by the player.
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable();

        /// <summary>
        /// When the instance is deselected, a specific cooldown for re-selection should be applied.
        /// </summary>
        /// <returns></returns>
        public IEnumerator InvokeHoverCooldown();
    }
}
