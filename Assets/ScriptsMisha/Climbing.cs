using ScriptsMisha.Mob;
using UnityEngine;

namespace ScriptsMisha
{
    public class Climbing : MonoBehaviour
    {
        [Header("References")]
        public Transform orientation;
        public Rigidbody rb;
        public Mobs mob;
        public LayerMask whatIsWall;

        [Header("Climbing")]
        public float climbSpeed;
        public float maxClimbTime;
        private float climbTimer;

        private bool climbing;

        [Header("Detection")]
        public float detectionLength;
        public float sphereCastRadius;
        public float maxWallLookAngle;
        private float wallLookAngle;

        private RaycastHit frontWallHit;
        private bool wallFront;

        private Transform lastWall;
        private Vector3 lastWallNormal;
        public float minWallNormalAngleChange;

        [Header("Exiting")]
        public bool exitingWall;
        public float exitWallTime;
        private float exitWallTimer;

        private void Update()
        {
            WallCheck();
            StateMachine();

            if (climbing && !exitingWall) ClimbingMovement();
        }

        private void StateMachine()
        {
            // State 1 - Climbing
            if (wallFront && wallLookAngle < maxWallLookAngle && !exitingWall)
            {
                if (!climbing && climbTimer > 0) StartClimbing();
                
                if (climbTimer > 0) climbTimer -= Time.deltaTime;
                if (climbTimer < 0) StopClimbing();
            }

            // State 2 - Exiting
            else if (exitingWall)
            {
                if (climbing) StopClimbing();

                if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
                if (exitWallTimer < 0) exitingWall = false;
            }

            // State 3 - None
            else
            {
                if (climbing) StopClimbing();
            }
        }

        private void WallCheck()
        {
            wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

            bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

            if ((wallFront && newWall) || mob.IsGrounded)
            {
                climbTimer = maxClimbTime;
            }
        }

        private void StartClimbing()
        {
            climbing = true;

            lastWall = frontWallHit.transform;
            lastWallNormal = frontWallHit.normal;
        }

        private void ClimbingMovement()
        {
            //rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
            var position = mob.transform.position;
            position = new Vector3(position.x, position.y + climbSpeed, position.z);
            mob.transform.position = position;
        }

        private void StopClimbing()
        {
            climbing = false;
        }
    }
}
