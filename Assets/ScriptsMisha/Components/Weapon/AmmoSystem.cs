using System;
using ScriptsMisha.UI;
using UnityEngine;

namespace ScriptsMisha.Components.Weapon
{
    public class AmmoSystem : MonoBehaviour
    {
        public float currentAmmo;
        public float clipSize;

        [Header("Fire")]
        public bool Fire;
        private float _progressFire;
        public float timeFire;
        
        [Header("Reload")]
        private float _progressReload;
        public float timeReload;

        private UIGame _ui;

        private void Start()
        {
            currentAmmo = clipSize;
            _ui = FindObjectOfType<UIGame>();
        }
        
        private void Update()
        {
            _progressReload = timeReload * Time.deltaTime;
            _progressFire = timeFire * Time.deltaTime;

            if (Fire)
            {
                currentAmmo -= timeFire;
            }
            else if (!Fire && currentAmmo < clipSize)
            {
                currentAmmo += timeReload;
            }

            _ui.ChangeBullet(currentAmmo);
        }
    }
}
