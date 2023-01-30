
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieMollie.Game
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        #region Variable Field
        [Header("Camera")]
        [SerializeField] private CameraViewType _cameraViewType = CameraViewType.ThirdPersonView;
        public CameraViewType ViewType
        {
            get => _cameraViewType;
        }
        [SerializeField] private Transform _cameraLookTarget = null;
        [SerializeField] private bool _lockCameraMovement = false;

        [Header("First Person View")]
        [SerializeField] private CinemachineVirtualCamera _firstPersonVirtualCam = null;
        [SerializeField] private float _fpcSpeedX = 120f;
        [SerializeField] private float _fpcSpeedY = 60f;

        [Header("Third Person View")]
        [SerializeField] private CinemachineFreeLook _thirdPersonVirtualCam = null;
        [SerializeField] private float _tpcSpeedX = 360f;
        [SerializeField] private float _tpcSpeedY = 6f;

        private PlayerInputActions _inputActions = null;
        private Vector2 _lookInput = Vector2.zero;
        private Vector2 _lookVelocity = Vector2.zero;
        public Vector2 LookVelocity
        {
            get => _lookVelocity;
        }
        private float _pitchAngle = 0f;

        #endregion

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            Cursor.lockState = CursorLockMode.Locked;

            _thirdPersonVirtualCam.Follow = _cameraLookTarget;
            _thirdPersonVirtualCam.LookAt = _cameraLookTarget;
            _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = _tpcSpeedX;
            _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = _tpcSpeedY;

            _firstPersonVirtualCam.Follow = _cameraLookTarget;
        }

        private void OnEnable()
        {
            _inputActions.Player.Look.performed += ReadLookInput;
            _inputActions.Player.Look.canceled += ReadLookInput;
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Look.performed -= ReadLookInput;
            _inputActions.Player.Look.canceled -= ReadLookInput;
            _inputActions.Disable();
        }

        private void Start()
        {
            LockCameraMovement(_lockCameraMovement);
            ChangeCameraView(_cameraViewType);
        }

        #region Subscribers
        private void ReadLookInput(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        #endregion

        #region Public Functions
        public void ChangeCameraView(CameraViewType type)
        {
            _cameraViewType = type;
            switch (type)
            {
                case CameraViewType.FirstPersonView:
                    _firstPersonVirtualCam.gameObject.SetActive(true);
                    _thirdPersonVirtualCam.gameObject.SetActive(false);
                    break;

                case CameraViewType.ThirdPersonView:
                    _thirdPersonVirtualCam.gameObject.SetActive(true);
                    _firstPersonVirtualCam.gameObject.SetActive(false);
                    break;
            }
        }

        public void LockCameraMovement(bool state)
        {
            _lockCameraMovement = state;
            switch (_cameraViewType)
            {
                case CameraViewType.FirstPersonView:
                    _firstPersonVirtualCam.gameObject.SetActive(true);
                    _thirdPersonVirtualCam.gameObject.SetActive(false);
                    break;

                case CameraViewType.ThirdPersonView:
                    if (state)
                    {
                        _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = 0;
                        _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = 0;
                    }
                    else
                    {
                        _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = _tpcSpeedX;
                        _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = _tpcSpeedY;
                    }
                    break;
            }
        }

        #endregion

        private void LateUpdate()
        {
            FirstPersonCameraMovement();
        }

        #region Private Functions
        private void FirstPersonCameraMovement()
        {
            if (_cameraViewType != CameraViewType.FirstPersonView) return;

            float yawVelocity = _lookInput.x * _fpcSpeedX * Time.deltaTime;
            float pitchVelocity = _lookInput.y * _fpcSpeedY * Time.deltaTime;
            _lookVelocity = new Vector2(yawVelocity, pitchVelocity);

            _pitchAngle -= pitchVelocity;
            _pitchAngle = Mathf.Clamp(_pitchAngle, -90f, 90f);

            _cameraLookTarget.localRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);
        }

        #endregion

    }

    public enum CameraViewType { ThirdPersonView, FirstPersonView }
}
