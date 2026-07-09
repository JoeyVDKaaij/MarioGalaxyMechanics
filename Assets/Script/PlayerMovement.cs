using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Movement
{
    [Header("Input")]
    [SerializeField, Tooltip("Set the input action of the movement.")]
    private InputActionReference moveRef;
    [SerializeField, Tooltip("Set input reference to jump.")]
    private InputActionReference jumpRef;

    protected override void Start()
    {
        base.Start();
        
        moveRef.action.Enable();
        
        jumpRef.action.Enable();
        jumpRef.action.performed += ctx => Jump();
    }

    protected override void Update()
    {
        PlayerDirection();
        
        base.Update();
    }

    /// <summary>
    /// Sets the direction of the player based on input.
    /// </summary>
    private void PlayerDirection()
    {
        if (moveRef == null) return;
        
        Vector2 moveDirection2D = moveRef.action.ReadValue<Vector2>();
        moveDirection = new Vector3(moveDirection2D.x, 0, moveDirection2D.y);
    }
}