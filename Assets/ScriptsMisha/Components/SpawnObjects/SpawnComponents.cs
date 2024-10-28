using ScriptsMisha.Utils;
using ScriptsMisha.Utils.ObjectPool;
using UnityEngine;

namespace ScriptsMisha.Components.SpawnObjects
{
    public class SpawnComponents : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _usePool;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SpawnInstance();
        }
        
        public GameObject SpawnInstance()
        {
            var targetPosition = _target.position;
            
            var instance = _usePool
                ? Pool.Instance.Get(_prefab, targetPosition)
                : SpawnUtils.Spawn(_prefab, targetPosition);
            
            instance.SetActive(true);
            return instance;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}
