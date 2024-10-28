using UnityEngine;

namespace ScriptsMisha.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;

        public void DestroyObject()
        {
            Destroy(_objToDestroy);
        }
    }
}