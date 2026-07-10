using System;
using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    private Vector3 _oldPosition;

    private void Start()
    {
        _oldPosition = transform.position;
    }

    private void Update()
    {
        if (_oldPosition == transform.position) return;

        Vector3 moveDir = transform.position - _oldPosition;
        
        float angle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        
        _oldPosition = transform.position;
    }
}
