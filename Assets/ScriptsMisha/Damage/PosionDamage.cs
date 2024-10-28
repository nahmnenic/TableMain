using System;
using System.Collections;
using ScriptsMisha.Components;
using ScriptsMisha.Utils;
using UnityEngine;

namespace ScriptsMisha.Damage
{
    public class PosionDamage : MonoBehaviour
    {
        [SerializeField] private float _coolDown;
        [SerializeField] private GameObject _player;
        
        private DamageComponent _damageComp;
        private LayerCheck _layerCheck;

        private void Awake()
        {
            _damageComp = GetComponent<DamageComponent>();
            _layerCheck = GetComponent<LayerCheck>();
        }

        public void StartHitting()
        {
            StartCoroutine(PozionHit());
        }

        private IEnumerator PozionHit()
        {
            while(_layerCheck.IsTouchingLayer)
            {
                _damageComp.ApplyHealth(_player);
                yield return new WaitForSeconds(_coolDown);
            }
            
            StopCoroutine(PozionHit());
        }
    }
}