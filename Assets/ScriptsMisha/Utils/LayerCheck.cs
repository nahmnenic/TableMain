using UnityEngine;

namespace ScriptsMisha.Utils
{
    public class LayerCheck : MonoBehaviour
    {
        public bool IsTouchingLayer;
        public string layerName;

        private void OnTriggerEnter(Collider other)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (other.gameObject.layer == layer)
            {
                IsTouchingLayer = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (other.gameObject.layer == layer)
            {
                IsTouchingLayer = false;
            }
        }
    }
}