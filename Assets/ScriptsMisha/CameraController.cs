using System;
using Cinemachine;
using UnityEngine;

namespace ScriptsMisha
{
    public class CameraController : MonoBehaviour
    {
        public Transform _currentCamFollowTransform;
        private InputSystem _inputSystem;
        public Vector2 rightStickInput;
        [HideInInspector] public float mouseSense;
        [HideInInspector] public float rotSmoothSpeed;
        public float xAxis, yAxis;
        private GameObject _enemyMid;

        private CinemachineVirtualCamera _vCam;
        private Cinemachine3rdPersonFollow _3rdPersonFollow;

        [HideInInspector] public float nextZ;
        [HideInInspector] public float nextY;
        [HideInInspector] public float nextX;
        [HideInInspector] public float rotation;
        [HideInInspector] public float yRotate;
        [HideInInspector] public bool IsAim;
        private float _progressRotate;
        [HideInInspector] public float camSmoothSpeed;

        [Header("RayCast Aim")]
        [SerializeField] private float _aimSmoothSpeed;
        [SerializeField] private LayerMask aimMask;
        public Transform aimPos;

        private void Start()
        {
            _vCam = GetComponentInChildren<CinemachineVirtualCamera>();
            _3rdPersonFollow = _vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            yRotate = _currentCamFollowTransform.localEulerAngles.y;
        }

        private void Update()
        {
            xAxis += rightStickInput.x * mouseSense;
            yAxis -= rightStickInput.y * mouseSense;
            yAxis = Mathf.Clamp(yAxis,-16, 18);

                SwitchPositionCamera();
            if (_progressRotate == 0) rotation = _currentCamFollowTransform.localEulerAngles.y;
            _progressRotate = rotSmoothSpeed * Time.fixedDeltaTime;

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            {
                if (hit.transform.GetComponentInChildren<AutoAim>() && IsAim)
                {
                    _enemyMid = hit.transform.GetComponentInChildren<AutoAim>().gameObject;
                    _currentCamFollowTransform.forward = _enemyMid.transform.position;
                    aimPos.position = Vector3.Lerp(aimPos.position, _enemyMid.transform.position, _aimSmoothSpeed * Time.deltaTime);
                }
                else
                {
                    _enemyMid = null;
                    aimPos.position = Vector3.Lerp(aimPos.position, hit.point, _aimSmoothSpeed * Time.deltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            if (IsAim && _enemyMid != null)
            {
                _currentCamFollowTransform.LookAt(_enemyMid.transform);
                rightStickInput = Vector2.zero;
                yAxis = ClampAngle(_currentCamFollowTransform.transform.localEulerAngles.x);
                xAxis += ClampAngle(_currentCamFollowTransform.localEulerAngles.y);
                
                _inputSystem.Disable();
            }
            else
            {
                _inputSystem.Enable();
                yRotate = Mathf.Lerp(yRotate, rotation, _progressRotate);
                _currentCamFollowTransform.localEulerAngles = new Vector3(yAxis, yRotate,
                    _currentCamFollowTransform.localEulerAngles.z);
            }
            
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
        }

        float ClampAngle(float angle)
        {
            return angle > 18 ? angle - 360 : angle;
        }

        private void SwitchPositionCamera()
        {
            var newVec = new Vector3(nextX, nextY, nextZ);
            _3rdPersonFollow.ShoulderOffset = Vector3.Lerp(_3rdPersonFollow.ShoulderOffset, newVec, camSmoothSpeed * Time.deltaTime);
        }
        
        private void OnEnable()
        {
            if (_inputSystem == null)
            {
                _inputSystem = new InputSystem();
                _inputSystem.PlayerMovement.Camera.performed += i => rightStickInput = i.ReadValue<Vector2>();
            }
            
            _inputSystem.Enable();
        }

        private void OnDisable()
        {
            _inputSystem.Disable();
        }
    }
}
