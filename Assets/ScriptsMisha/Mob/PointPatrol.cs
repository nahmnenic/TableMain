using System.Collections;
using UnityEngine;

namespace ScriptsMisha.Mob
{
    public class PointPatrol : Patrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _minDistanceToPoint;

        private Mobs _mob;
        private MobAi _mobAi;
        private int _currenPoint;

        private void Awake()
        {
            _mob = GetComponent<Mobs>();
            _mobAi = GetComponent<MobAi>(); 
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled && !_mobAi.Vision.IsTouchingLayer)
            {
                if (_mobAi._isdead) yield break;
                if (_mobAi.isHit) yield break;

                _mob.currentSpeed = _mob.walkingSpeed;
                _mobAi.SetSpeed();

                if (_mobAi.Agent.remainingDistance < _minDistanceToPoint)
                {
                    GoToNextPoint();
                }

                yield return null;
            }
        }

        private void GoToNextPoint()
        {
            _mobAi.SetDestination(_points[_currenPoint].position);
            _currenPoint = (_currenPoint + 1) % _points.Length;
        }
    }
}