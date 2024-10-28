using UnityEngine;

namespace ScriptsMisha.Utils
{
    public class SpawnUtils
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string contanierName = ContainerName)
        {
            var container = GameObject.Find(contanierName);
            if (container == null)
                container = new GameObject(contanierName);

            return Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }   
    }
}