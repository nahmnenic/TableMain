using System;
using ScriptsMisha.Player;
using UnityEngine;

namespace ScriptsMisha
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator _animator;
        private PlayerLocomotion _locomotion;
        private PlayerActions _playerActions;
        private int horizontal;
        private int vertical;
        private int magnitude;
        private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _locomotion = GetComponent<PlayerLocomotion>();
            _playerActions = GetComponent<PlayerActions>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
            magnitude = Animator.StringToHash("InputMagnitude");
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            _animator.SetBool(IsInteracting, isInteracting);
            _animator.CrossFade(targetAnimation, 0.2f);
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;
            
            #region Snapped Horizontal
            /*if (_playerActions.OnlyWalk())
            {
                if (horizontalMovement > 0)
                {
                    snappedHorizontal = 0.5f;
                }
                else if (horizontalMovement < 0 )
                {
                    snappedHorizontal = -0.5f;
                }
                else
                {
                    snappedHorizontal = 0;
                }
            }
            else*/
            {
                if (horizontalMovement > 0 && horizontalMovement < 0.55f)
                {
                    snappedHorizontal = 0.5f;
                }
                else if (horizontalMovement > 0.55f)
                {
                    snappedHorizontal = 1f;
                }
                else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
                {
                    snappedHorizontal = -0.5f;
                }
                else if (horizontalMovement < -0.55f)
                {
                    snappedHorizontal = -1f;
                }
                else
                {
                    snappedHorizontal = 0;
                }
            }
            #endregion
            #region Snapped Vertical
            /*if (_playerActions.OnlyWalk())
            {
                if (verticalMovement > 0)
                {
                    snappedVertical = 0.5f;
                }
                else if (verticalMovement < 0)
                {
                    snappedVertical = -0.5f;
                }
                else
                {
                    snappedVertical = 0;
                }
            }
            else*/
            {
                if (verticalMovement > 0 && verticalMovement < 0.55f)
                {
                    snappedVertical = 0.5f;
                }
                else if (verticalMovement > 0.55f)
                {
                    snappedVertical = 1f;
                }
                else if (verticalMovement < 0 && verticalMovement > -0.55f)
                {
                    snappedVertical = -0.5f;
                }
                else if (verticalMovement < -0.55f)
                {
                    snappedVertical = -1f;
                }
                else
                {
                    snappedVertical = 0;
                }
            }
            #endregion

            if (isSprinting && !_locomotion.IsAiming)
            {
                snappedHorizontal *= 4;
                snappedVertical *= 4;
            }

            float delta;
            delta = Mathf.Abs(snappedHorizontal) + Mathf.Abs(snappedVertical);

            _animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            _animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
            _animator.SetFloat(magnitude, delta, 0.1f, Time.deltaTime);
        }
    }
}
