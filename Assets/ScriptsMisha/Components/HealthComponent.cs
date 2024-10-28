using System;
using ScriptsMisha.Components.SpawnObjects;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptsMisha.Components
{
    public class HealthComponent : MonoBehaviour
    {
        public int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private HealthChangeEvent _onChange;

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            _health += healthDelta;

            _onChange?.Invoke(_health);

            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (healthDelta > 0)
            {
                _onHealth?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }
        public void SetHealth(int health)
        {
            _health = health;
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}