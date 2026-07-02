using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Movement")] [SerializeField, Tooltip("Set the movement speed of the character."), Min(0)]
    private float moveSpeed = 10f;
    
    protected Vector3 moveDirection;
    private Rigidbody _rb;
    
    [Header("Jumping")]
    [SerializeField, Tooltip("Set the force behind a jump."), Min(0)]
    private float jumpForce = 12f;
    [SerializeField, Tooltip("Set the jump cooldown."), Min(0)]
    private float jumpCooldown = 0.25f;
    [SerializeField, Tooltip("Set the movement speed multiplier when the character is in the air."), Min(0)]
    private float airMultiplier = 0.4f;
    private bool _jumpReady = true;
    
    [Header("Ground Check")] 
    [SerializeField, Tooltip("Set the layer-mask of the ground.")]
    private LayerMask groundLayerMask;
    [SerializeField, Tooltip("Set the ground drag.")]
    private float groundDrag = 5f;
    
    private bool _grounded;
    private MeshRenderer _renderer;
    private float PlayerHeight => _renderer.bounds.size.y;
    
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        
        _renderer = GetComponent<MeshRenderer>();
    }

    protected virtual void Update()
    {
        GroundCheck();
        
        MovePlayer();

        SpeedControl();
    }

    /// <summary>
    /// Checks the ground and updates the drag accordingly.
    /// </summary>
    private void GroundCheck()
    {
        // Check if player is grounded.
        // The raycast length is half of the player height + some margin.
        _grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, groundLayerMask);
        
        _rb.linearDamping = _grounded ? groundDrag : 0;
    }

    /// <summary>
    /// Gets an direction and moves the player.
    /// </summary>
    private void MovePlayer()
    {
        // Update total movement speed depending on if the character is in the air or not.
        float totalMovementSpeed = moveSpeed * (_grounded ? 1 : airMultiplier);
        
        _rb.AddForce(moveDirection * totalMovementSpeed, ForceMode.Force);
    }

    /// <summary>
    /// Controls the speed so that the character will not move faster than the set movement speed.
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Makes the character jump.
    /// </summary>
    protected void Jump(InputAction.CallbackContext ctx = new InputAction.CallbackContext())
    {
        if (!_jumpReady || !_grounded) return;
        
        // Reset y velocity.
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // Sets up the jump cooldown.
        _jumpReady = false;
        Invoke(nameof(ReadyJump), jumpCooldown);
    }

    /// <summary>
    /// Readies the jump method.
    /// </summary>
    private void ReadyJump()
    {
        _jumpReady = true;
    }
}
