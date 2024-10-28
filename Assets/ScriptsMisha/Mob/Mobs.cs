using System;
using ScriptsMisha.Utils;
using UnityEngine;

namespace ScriptsMisha.Mob
{
    public class Mobs : MonoBehaviour
    {
        [Header("Falling")] 
        public float inAirTimer;
        public float fallingVelocity;
        public float leapingVelocity;
        public float rayCastOffSet = 0.5f;
        public LayerMask groundLayer;
        public bool isInteracting;
        public bool IsGrounded;
        private AnimatorManager _animatorManager;
        
        [Header("Spees Properties")]
        public float currentSpeed;
        public float walkingSpeed;
        public float runningSpeed;
        
        private Vector3 _moveDirection;
        private Rigidbody _rb;
        
        [SerializeField] private CheckCircleOverlap _attackMelee;
        
        [Header("Bullet Properties")] 
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform barrelPos;
        [SerializeField] private float bulletVelocity;
        [SerializeField] private int bulletPerShot;
        
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        public float rotationSpeed = 20f;
        private MobAi _mobAi;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _animatorManager = GetComponent<AnimatorManager>();
            _mobAi = GetComponent<MobAi>();
        }
        
        private void FixedUpdate()
        {
            HandleFallingAndLanding();
            if (isInteracting)
                return;
            
            //var dir = _mobAi.Agent.destination.normalized;
            //_rb.velocity = new Vector3(dir.x, dir.y, dir.z);
        }
        
        private void LateUpdate()
        {
            isInteracting = _animator.GetBool("isInteracting");
            _animator.SetBool("isGrounded", IsGrounded);
        }
        
        private void HandleFallingAndLanding()
        {
            RaycastHit hit;
            Vector3 rayCastOrirgin = transform.position;
            Vector3 targetPosition;
            rayCastOrirgin.y += rayCastOffSet;
            targetPosition = transform.position;
            
            if (!IsGrounded)
            {
                if (!isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Falling", true);
                }

                inAirTimer += Time.deltaTime;
                _rb.AddForce(transform.forward * leapingVelocity);
                _rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            }

            if (Physics.SphereCast(rayCastOrirgin, 0.1f, -Vector3.up, out hit,1f,groundLayer))
            {
                if (!IsGrounded && !isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Land", true);
                }

                Vector3 rayCastHitPoint = hit.point;
                targetPosition.y = rayCastHitPoint.y;
                inAirTimer = 0;
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }

            if (IsGrounded)
            {
                if (isInteracting)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        public void SetDirection(Vector3 direction)
        {
            _moveDirection = direction;
            SetSpeedAnimation();
           // SetAngleDefualt();
        }

        public void SetSpeedAnimation()
        {
            _animator.SetFloat(Speed, currentSpeed);
        }

        private void SetAngleDefualt()
        {
            if (_moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);                                                              // 4
            }
        }

        public void RangeAttack()
        {
            SetSpeedAnimation();
            for (int i = 0; i < bulletPerShot; i++)
            {
                GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
                Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
                rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
            }
        }

        public void MeleeAttack()
        {
            _attackMelee.Check();
        }
    }
}