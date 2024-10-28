using UnityEngine;

namespace ScriptsMisha.Components
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        public void ApplyHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ModifyHealth(_hpDelta);
            }
        }
    }
}