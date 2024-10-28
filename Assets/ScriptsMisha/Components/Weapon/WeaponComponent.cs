using UnityEngine;

namespace ScriptsMisha.Components.Weapon
{
    public class WeaponComponent : MonoBehaviour
    {
        private CameraController _camCont;
        private AmmoSystem _ammo;
        
        [Header("Fire Rate")] 
        [SerializeField] private float fireRate;
        [SerializeField] private bool semiAuto;
        public bool IsFire;
        private float _fireRateTimer;

        [Header("Bullet Properties")] 
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform barrelPos;
        [SerializeField] private float bulletVelocity;
        [SerializeField] private int bulletPerShot;

        private void Awake()
        {
            _ammo = GetComponent<AmmoSystem>();
            _camCont = GetComponentInParent<CameraController>();
        }

        private void Start()
        {
            _fireRateTimer = fireRate;
        }

        private void Update()
        {
            if (ShouldFire()) Fire();
            if (IsFire && _ammo.currentAmmo > 0) _ammo.Fire = true;
            else _ammo.Fire = false;
        }

        private bool ShouldFire()
        {
            _fireRateTimer += Time.deltaTime;
            if (_fireRateTimer < fireRate) return false;
            if (_ammo.currentAmmo < 0.1) return false;
            if (semiAuto && IsFire) return true;
            if (!semiAuto && IsFire) return true;
            return false;
        }

        private void Fire()
        {
            _fireRateTimer = 0;
            barrelPos.LookAt(_camCont.aimPos);
            for (int i = 0; i < bulletPerShot; i++)
            {
                GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
                Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
                rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
            }
        }
    }
}