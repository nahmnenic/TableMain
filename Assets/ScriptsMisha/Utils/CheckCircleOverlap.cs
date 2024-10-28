using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptsMisha.Utils
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;

        public Transform _midBody;

        private readonly Collider[] _interactResult = new Collider[10];

        [SerializeField] private OnOverlapEvent _onOverLap;

        /*private void OnDrawGizmosSelected()
        {
            Handles.color = Color.red;
            Handles.DrawSolidDisc(_midBody.position, Vector3.forward, _radius);
        }*/

        public void Check()
        {
            var size = Physics.OverlapSphereNonAlloc(_midBody.position, _radius, _interactResult, _mask);
            
            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                {
                    _onOverLap?.Invoke(overlapResult.gameObject);
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {

        }
    }
}