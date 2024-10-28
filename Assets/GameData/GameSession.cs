using GameData.Data;
using UnityEngine;

namespace GameData
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        private void Awake()
        {
            if (IsSessionExit())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }
            return false;
        }

        /*private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("CoinPerClick", _data.CoinPerClick);
            PlayerPrefs.SetInt("CoinPerSecond", _data.CoinPerSecond);
            PlayerPrefs.SetInt("Coin", _data.Coin);
            PlayerPrefs.SetInt("CurrentEnergy", _data.CurrentEnergy);
        }*/
    }
}
