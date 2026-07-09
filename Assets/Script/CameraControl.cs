using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineOrbitalFollow), typeof(CinemachineInputAxisController))]
public class CameraControl : MonoBehaviour
{
    private CinemachineOrbitalFollow _cameraFollow;
    private CinemachineInputAxisController _cameraController;
    private float cameraDistance = 10;
    private InputActionReference cameraInput;

    private void Start()
    {
        _cameraFollow = GetComponent<CinemachineOrbitalFollow>();
        _cameraController = GetComponent<CinemachineInputAxisController>();
    }

    private void Update()
    {
        
    }
}
