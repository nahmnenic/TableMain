using System;
using System.Collections;
using ScriptsMisha.Components.SpawnObjects;
using ScriptsMisha.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScriptsMisha.Mob
{
    public class MobAi : MonoBehaviour
    {
        [Header("Mob Interaction")]
        public LayerCheck Vision;
        [SerializeField] private LayerCheck _canAttack;
        
        [Header("Cool Down")]
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _coolDownAttack = 1f;
        
        [HideInInspector] public bool isHit;
        [HideInInspector] public bool _isdead;

        [Header("AI Properties")] 
        public bool FireAndPatrol;
        public bool MeleeAttack;
        public bool SpawnerMob;
        
        private bool _meleAttack;
        
        private static readonly int IsDieKey = Animator.StringToHash("IsDead");
        private static readonly int Hit1 = Animator.StringToHash("Hit");

        public Transform _player;
        
        private Mobs _mob;
        private Patrol _patrol;
        private Coroutine _currentCor;
        private GameObject _target;
        private Animator _animator;
        private SpawnListComponent _spawn;
        public NavMeshAgent Agent;


        private void Awake()
        {
            _mob = GetComponent<Mobs>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            _player = FindObjectOfType<Player1>().transform;
            _spawn = FindObjectOfType<SpawnListComponent>();
        }

        private void Start()
        {
            if (SpawnerMob)
            {
            }
            else
            {
                StartState(_patrol.DoPatrol());
            }
        }

        public void OnPlayerInVision(GameObject go)
        {
            if(_isdead) return;
            
            _target = go;

            StartState(AgroToPlayer());
        }

        private IEnumerator AgroToPlayer()
        {
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToPlayer());
        }
        
        private IEnumerator GoToPlayer()
        {
            while (Vision.IsTouchingLayer)
            {
                _animator.SetBool("MelleAttack", false);
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(MeleeAttackMob());
                }
                else
                {
                    _mob.currentSpeed = _mob.runningSpeed;
                    SetDestination(_player.position);
                }
                
                yield return null;
            }
            
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator RangeAttack()
        {
            while (Vision.IsTouchingLayer)
            {
                _mob.currentSpeed = 0f;

                if (_canAttack.IsTouchingLayer)
                {
                    _mob.RangeAttack();
                }
                
                yield return null;
            }
            
            StartState(_patrol.DoPatrol());
        }

        private IEnumerator MeleeAttackMob()
        {
            while (_canAttack.IsTouchingLayer)
            {
                SetDestination(Vector3.zero);
                _animator.SetBool("MelleAttack", true);
                _mob.currentSpeed = 0f;
                SetSpeed();
                _mob.SetSpeedAnimation();

                if (_canAttack.IsTouchingLayer)
                {
                    _mob.MeleeAttack();
                    yield return new WaitForSeconds(_coolDownAttack);
                }
                
                yield return null;
            }
            
            StartState(GoToPlayer());
        }
        
        private IEnumerator Hit()
        {
            isHit = true;
            _mob.SetDirection(Vector3.zero);
            Agent.isStopped = true;
            _mob.currentSpeed = 0f;
            SetSpeed();
            _animator.SetTrigger(Hit1);
            yield return new WaitForSeconds(0.667f);
            Agent.isStopped = false;
            isHit = false;
            StartState(GoToPlayer());
        }

        private IEnumerator Die()
        {
            _isdead = true;
            if (SpawnerMob)
            {
                yield return new WaitForSeconds(.1f);
                _spawn.Spawn();
            }
            _animator.SetBool(IsDieKey, true);
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
            if (_currentCor != null)
                StopCoroutine(_currentCor);
        }

        public void SetDestination(Vector3 destination)
        {
            SetSpeed();
            Agent.destination = destination;
            destination.y = 0;
            _mob.SetDirection(destination.normalized);
        }

        private void StartState(IEnumerator coroutine)
        {
            _mob.SetDirection(Vector3.zero);
            
            if (_currentCor != null)
                StopCoroutine(_currentCor);
            
            if(_isdead) return;
            _currentCor = StartCoroutine(coroutine);
        }
        
        public void OnDie()
        {
            StartState(Die());
        }
        
        public void HitTrigger()
        {
            StartState(Hit());
        }

        public void SetSpeed()
        {
            Agent.speed = _mob.currentSpeed;
            _mob.SetSpeedAnimation();
        }
    }
}
