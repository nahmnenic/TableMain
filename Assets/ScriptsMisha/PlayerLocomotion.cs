using UnityEngine;

namespace ScriptsMisha
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager _palyerManager;
        private AnimatorManager _animatorManager;
        private InputManager inputManager;
        private Vector3 _moveDirection;
        private Transform _cameraObj;
        private Rigidbody _rb;
        private bool _allowDoubleJump;

        [Header("Layer Check")]
        public LayerMask pozionLayer;
        public LayerMask slowLayer;

        [Header("Falling")] 
        public float inAirTimer;
        public float fallingVelocity;
        public float leapingVelocity;
        public float rayCastOffSet = 0.5f;
        public LayerMask groundLayer;

        [Header("Movement Flags")]
        public bool IsSprinting;
        public bool IsGrounded;
        public bool IsJumping;
        public bool IsCrouching;
        
        [Header("Other Speed")]
        public float pozionSpeed;
        public float slowlySpeed;

        [Header("Movement Speed")]
        public float walkingSpeed;
        public float runningSpeed;
        public float sprintingSpeed;
        public float aimingSpeed;

        [Header("Jump Speed")] 
        public float gravityIntensity;
        public float jumpHight;
        public float secondJumpHight;

        [Header("OnlyWalk")]
        public bool IsAiming;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            _rb = GetComponent<Rigidbody>();
            _animatorManager = GetComponent<AnimatorManager>();
            _palyerManager = GetComponent<PlayerManager>();
            _cameraObj = Camera.main.transform;
        }

        public void HandleAllMovement()
        {
            HandleFallingAndLanding();

            if (_palyerManager.isInteracting)
                return;
            HandleMovement();
        }
        
        private void HandleMovement()
        {
            if (IsJumping)
                return;
            
            _moveDirection = _cameraObj.forward * inputManager.verticalInput;
            _moveDirection += _cameraObj.right * inputManager.horizontalalInput;
            _moveDirection.Normalize();
            _moveDirection.y = 0;

            if (CheckSurface(pozionLayer))
            {
                _moveDirection *= pozionSpeed;
            }
            else if (CheckSurface(slowLayer))
            {
                _moveDirection *= slowlySpeed;
            }
            else
            {
                if (IsSprinting && IsGrounded && !IsAiming && !IsCrouching)
                {
                    _moveDirection *= sprintingSpeed;
                }
                else
                {
                    if (inputManager.moveAmount >= 0.5f && !IsAiming && !IsCrouching)
                    {
                        _moveDirection *= runningSpeed;
                    }
                    else if (IsAiming)
                    {
                        _moveDirection *= aimingSpeed;
                    }
                    else if (IsCrouching)
                    {
                        _moveDirection *= walkingSpeed;
                    }
                    else
                    {
                        _moveDirection *= walkingSpeed;
                    }
                }
            }

            Vector3 movementVelocity = _moveDirection;
            _rb.velocity = movementVelocity;
        }

        private void HandleFallingAndLanding()
        {
            RaycastHit hit;
            Vector3 rayCastOrirgin = transform.position;
            Vector3 targetPosition;
            rayCastOrirgin.y += rayCastOffSet;
            targetPosition = transform.position;
            
            if (!IsGrounded && !IsJumping)
            {
                if (!_palyerManager.isInteracting)
                {
                    _animatorManager.PlayTargetAnimation("Falling", true);
                }

                inAirTimer += Time.deltaTime;
                _rb.AddForce(transform.forward * leapingVelocity);
                _rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            }

            if (Physics.SphereCast(rayCastOrirgin, 0.2f, -Vector3.up, out hit,1f,groundLayer))
            {
                if (!IsGrounded && !_palyerManager.isInteracting)
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

            if (IsGrounded && !IsJumping)
            {
                if (_palyerManager.isInteracting || inputManager.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (IsGrounded)
            {
                _allowDoubleJump = true;
                _animatorManager._animator.SetBool("isJumping", true);
                _animatorManager.PlayTargetAnimation("Jump", false);

                float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHight);
                Vector3 playerVelocity = _moveDirection;
                playerVelocity.y = jumpingVelocity;
                _rb.velocity = playerVelocity;
            }
            else if (_allowDoubleJump)
            {
                _animatorManager._animator.SetBool("isJumping", true);
                _animatorManager.PlayTargetAnimation("Jump", false);

                float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * secondJumpHight);
                Vector3 playerVelocity = _moveDirection;
                playerVelocity.y = jumpingVelocity;
                _rb.velocity = playerVelocity;
                _allowDoubleJump = false;
            }
        }

        private bool CheckSurface(int layer)
        {
            RaycastHit hit;
            Vector3 rayCastOrirgin = transform.position;
            rayCastOrirgin.y += rayCastOffSet;
            if (Physics.SphereCast(rayCastOrirgin, 0.2f, -Vector3.up, out hit,1f, layer))
            {
                return true;
            }

            return false;
        }
    }
}