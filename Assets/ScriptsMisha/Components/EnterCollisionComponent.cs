using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptsMisha.Components
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(collision.gameObject);
                Destroyoject();
            }
        }

        public void Destroyoject()
        {
            Destroy(_objToDestroy);
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}