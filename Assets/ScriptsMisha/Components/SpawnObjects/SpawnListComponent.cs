using ScriptsMisha.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptsMisha.Components.SpawnObjects
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] SpawnPoints;
        private GameObject _currentGameObj;
        private float _timer;

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        public void Spawn()
        {
            var point = Random.Range(0, SpawnPoints.Length);
            SpawnPoints[point].GetComponent<SpawnComponents>().Spawn();
        }
    }
}