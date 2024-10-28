using GameData.Definitions;
using ScriptsMisha;
using UnityEngine;

namespace GameData
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject gO)
        {
            var player = gO.GetComponent<Player1>();
            if (player != null)
                player.AddInInventory(_id, _count);
        }
    }
}