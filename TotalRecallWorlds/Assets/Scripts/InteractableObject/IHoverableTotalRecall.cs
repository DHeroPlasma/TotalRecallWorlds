using System.Collections;
using UnityEngine;
namespace ZukiniFun.TotalInputCore
{
    /// <summary>
    /// 
    /// </summary>
    public enum SelectionState
    {
        Unselected = 0,
        Selected = 1,
        Blocked = 2
    }

    public interface IHoverableTotalRecall
    {
        /// <summary>
        /// 
        /// </summary>
        public void HoverEnter();

        /// <summary>
        /// 
        /// </summary>
        public void HoverExit();

        /// <summary>
        /// 
        /// </summary>
        public bool IsHovered();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSelected();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator InvokeHoverCooldown();
    }
}
