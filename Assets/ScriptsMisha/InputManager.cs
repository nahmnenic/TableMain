using ScriptsMisha.Components.Weapon;
using ScriptsMisha.Player;
using UnityEngine;

namespace ScriptsMisha
{
    public class InputManager : MonoBehaviour
    {
        private InputSystem _inputSystem;
        private PlayerLocomotion _playerLococmotion;
        private PlayerActions _playerActions;
        private WeaponComponent weapon;
        private CameraController _cam;
        private AnimatorManager _animatorManager;
        private Player1 _player;
        private Animator _anim;

        private static readonly int Crouching = Animator.StringToHash("isCrouching");
        
        public Vector2 movementInput;
        public float moveAmount;
        public float verticalInput;
        public float horizontalalInput;

        public bool shift_Input;
        public bool space_Input;
        public bool e_Input;
        public bool lb_Input;
        public bool rb_Input;
        public bool ctrl_Input;

        private void Awake()
        {
            _animatorManager = GetComponent<AnimatorManager>();
            _cam = GetComponent<CameraController>();
            _anim = GetComponent<Animator>();
            _playerActions = GetComponent<PlayerActions>();
            _playerLococmotion = GetComponent<PlayerLocomotion>();
            _player = GetComponent<Player1>();
            weapon = GetComponentInChildren<WeaponComponent>();
        }

        private void OnEnable()
        {
            if (_inputSystem == null)
            {
                _inputSystem = new InputSystem();
                
                _inputSystem.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

                _inputSystem.PlayerActions.Shift.performed += i => shift_Input = true;
                _inputSystem.PlayerActions.Shift.canceled += i => shift_Input = false;
                _inputSystem.PlayerActions.Aim.performed += i => rb_Input = true;
                _inputSystem.PlayerActions.Aim.canceled += i => rb_Input = false;
                _inputSystem.PlayerActions.Jump.performed += i => space_Input = true;
                _inputSystem.PlayerActions.Interaction.performed += i => e_Input = true;
                _inputSystem.PlayerActions.Fire.performed += i => lb_Input = true;
                _inputSystem.PlayerActions.Fire.canceled += i => lb_Input = false;
                _inputSystem.PlayerActions.Crouch.performed += i => ctrl_Input = true;
                _inputSystem.PlayerActions.Crouch.canceled += i => ctrl_Input = false;
            }
            
            _inputSystem.Enable();
        }

        private void OnDisable()
        {
            _inputSystem.Disable();
        }

        public void HandleAllInput()
        {
            HandleMovementInput();
            HandleSprintingInput();
            HandleJumpingInput();
            HandleInteractionInput();
            HandleFireInput();
            HandleAimInput();
            HandleCrouchInput();
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalalInput) + Mathf.Abs(verticalInput));
            _animatorManager.UpdateAnimatorValues(horizontalalInput, verticalInput, _playerLococmotion.IsSprinting);
        }

        private void HandleSprintingInput()
        {
            if (shift_Input && moveAmount > 0.5f && _playerLococmotion.IsGrounded && verticalInput >= 0)
            {
                _playerLococmotion.IsSprinting = true;
            }
            else
            {
                _playerLococmotion.IsSprinting = false;
            }
        }

        private void HandleJumpingInput()
        {
            if (space_Input)
            {
                space_Input = false;
                _playerLococmotion.HandleJumping();
            }
        }

        private void HandleCrouchInput()
        {
            if (ctrl_Input)
            {
                _playerLococmotion.IsCrouching = true;
                _anim.SetBool(Crouching, ctrl_Input);
            }
            else
            {
                _playerLococmotion.IsCrouching = false;
                _anim.SetBool(Crouching, ctrl_Input);
            }
        }

        private void HandleInteractionInput()
        {
            if (e_Input)
            {
                e_Input = false;
                _player.Interact();
            }
        }
        
        private void HandleFireInput()
        {
            if (lb_Input && rb_Input)
            {
                weapon.IsFire = true;
            }
            else
            {
                weapon.IsFire = false;
            }
        }

        private void HandleAimInput()
        {
            if (rb_Input && !ctrl_Input)
            {
                _cam.IsAim = true;
                _playerLococmotion.IsAiming = true;
                _playerActions.IsAiming = true;
                _playerActions.HandleAiming();
            }
            else
            {
                _cam.IsAim = false;
                _playerLococmotion.IsAiming = false;
                _playerActions.IsAiming = false;
            }
        }
    }
}
