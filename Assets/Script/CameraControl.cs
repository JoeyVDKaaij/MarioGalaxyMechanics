using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    private CinemachineOrbitalFollow _cameraFollow;

    [SerializeField, Tooltip("Set the input that affects the camera.")]
    private InputActionReference cameraInput;

    [SerializeField, Tooltip("Set the input that affects the camera.")]
    private bool invertRotation;

    [SerializeField, Tooltip("Set the input that affects the camera.")]
    private bool rotateBasedOnPress;

    [SerializeField, Tooltip("Set the input that affects the camera."), Min(0)]
    private float horizontalSensitivity;

    [SerializeField, Tooltip("Set the input that affects the camera."), Min(0)]
    private float verticalSensitivity;

    [SerializeField, Tooltip("Set the maximum value on how far the camera can go upwards."), Min(0)]
    private float maxVerticalValue;

    private bool _rotating;

    private float _oldValue;
    private float _newValue;
    private float _timer;

    private float HorizontalValue 
    {
        get => _cameraFollow.HorizontalAxis.Value; 
        set => _cameraFollow.HorizontalAxis.Value = value;
    }

    private float VerticalValue 
    {
        get => _cameraFollow.VerticalAxis.Value; 
        set => _cameraFollow.VerticalAxis.Value = value;
    }

    private void Start()
    {
        _cameraFollow = GetComponent<CinemachineOrbitalFollow>();
        cameraInput.action.Enable();
        cameraInput.action.performed += ctx => OnCameraInputPressed();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (_rotating && rotateBasedOnPress) RotateCameraOverTime();
        else if (!rotateBasedOnPress) RotateCamera();
    }

    private void OnCameraInputPressed()
    {
        if (_rotating || !rotateBasedOnPress) return;
        
        _oldValue = HorizontalValue;
        
        switch (cameraInput.action.ReadValue<Vector2>().x)
        {
            case >0.5f:
                _newValue = _oldValue + (invertRotation ? -90 : 90);
                break;
            case <-0.5f:
                _newValue = _oldValue + (invertRotation ? 90 : -90);
                break;
        }
        
        _rotating = true;
    }

    private void RotateCameraOverTime()
    {
        _timer += Time.deltaTime;
        HorizontalValue = Mathf.Lerp(_oldValue, _newValue, _timer);

        if (Mathf.Abs(_newValue - HorizontalValue) < 0.1f)
        {
            _timer = 0;
            _rotating = false;
        }
    }

    private void RotateCamera()
    {
        Vector2 input = (invertRotation ? -1 : 1) * cameraInput.action.ReadValue<Vector2>();
        
        HorizontalValue += input.x * horizontalSensitivity;
        VerticalValue += input.y * verticalSensitivity;
        
        VerticalValue = Mathf.Clamp(VerticalValue, -maxVerticalValue, maxVerticalValue);
    }
}
