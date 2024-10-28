using System;
using System.Linq;
using ScriptsMisha.Components;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptsMisha.UI
{
    public class UIGame : MonoBehaviour
    {
        [SerializeField] private Text _hp;
        [SerializeField] private Transform _player;
        [SerializeField] private Slider _bullet;

        private void Update()
        {
            var hp = _player.GetComponent<HealthComponent>();
            _hp.text = hp._health.ToString();
        }

        public void ChangeBullet(float bullet)
        {
            _bullet.value = bullet;
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("wdwe");
        }
    }
}
