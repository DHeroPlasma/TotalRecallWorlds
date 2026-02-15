using UnityEngine;
using UnityEngine.UI;
using ZukiniFun.TotalAgentCore;

namespace ZukiniFun.TotalAgentHelpers
{
    /// <summary>
    /// Generated helper that will fit the agents capsule collider.
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    public class AutoCapsuleFromRenderer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            FitCollider();
        }

        /// <summary>
        /// Public interface.
        /// </summary>
        public void FitCollider()
        {
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            Bounds bounds = renderers[0].bounds;
            foreach (var r in renderers)
            {
                bounds.Encapsulate(r.bounds);
            }

            col.direction = 1;
            col.height = bounds.size.y;
            col.radius = Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f;
            col.center = transform.InverseTransformPoint(bounds.center);
        }
    }
}