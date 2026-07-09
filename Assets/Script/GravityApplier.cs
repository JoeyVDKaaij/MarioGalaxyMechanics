using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GravityApplier : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField, Tooltip("Set where the direction is headed.")]
    private AtmosphereType atmosphereType = AtmosphereType.OneDirection;
    [SerializeField, Tooltip("Reverse the gravity direction.")]
    private bool reverseGravityDirection;
    
    [Header("Single Direction")]
    [SerializeField, Tooltip("Set new gravity direction.")]
    private Vector3 gravityDir = Physics.gravity;
    
    [Header("Single Point Gravity")]
    [SerializeField, Tooltip("Enables the option to precisely place the centerpoint.")]
    private bool useCustomCenterPoint;
    [SerializeField, Tooltip("Base the center on the transform of the object.")]
    private Transform transformCenterPoint;
    [SerializeField, Tooltip("Set the centerpoint location.")]
    private Vector3 vector3CenterPoint;
    [SerializeField, Tooltip("Set the strength of the gravity.")]
    private float gravityStrength = Physics.gravity.magnitude;

    [Header("Gravity On Exit")]
    [SerializeField, Tooltip("Enables applying a gravity direction when leaving the Trigger.")]
    private bool changeGravityDirOnExit;
    [SerializeField, Tooltip("Set the gravity direction.")]
    private Vector3 gravityDirOnExit = Physics.gravity;

    // Ensures that the Trigger is active during play.
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    #region Trigger Methods
    
    // Changes gravity when a character enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (atmosphereType != AtmosphereType.OneDirection || !other.TryGetComponent(out Movement movement)) return;
        
        ApplyDirectionalGravity(movement, gravityDir);
    }

    // Changes gravity every frame the player stays in the trigger.
    private void OnTriggerStay(Collider other)
    {
        if (atmosphereType != AtmosphereType.TowardsSinglePoint || !other.TryGetComponent(out Movement movement)) return;
        
        ApplyPointGravity(movement);
    }

    // Change gravity when the character leaves the trigger (if enabled).
    private void OnTriggerExit(Collider other)
    {
        if (!changeGravityDirOnExit || !other.TryGetComponent(out Movement movement)) return;
        
        ApplyDirectionalGravity(movement, gravityDirOnExit);
    } 
    
    #endregion
    
    #region Gravity Applying Methods
    
    /// <summary>
    /// Applies gravity to a single direction.
    /// </summary>
    /// <param name="pMovement">The Movement component</param>
    /// <param name="pGravityDir">The gravity direction</param>
    private void ApplyDirectionalGravity(Movement pMovement, Vector3 pGravityDir)
    {
        pMovement.ChangeGravityDirection(reverseGravityDirection ? -pGravityDir : pGravityDir);
    }

    /// <summary>
    /// Applies gravity to a single point.
    /// </summary>
    /// <param name="pMovement">The Movement component</param>
    private void ApplyPointGravity(Movement pMovement)
    {
        Vector3 centerPoint;

        if (useCustomCenterPoint)
        {
            centerPoint = vector3CenterPoint;
        }
        else
        {
            centerPoint = transformCenterPoint == null ? Vector3.zero : transformCenterPoint.position;
        }
        
        Vector3 centerPointDir = centerPoint - pMovement.transform.position;
        
        Vector3 centerPointGravityDir = centerPointDir.normalized * gravityStrength;
        
        pMovement.ChangeGravityDirection(reverseGravityDirection ? -centerPointGravityDir : centerPointGravityDir);
    }
    
    #endregion
}