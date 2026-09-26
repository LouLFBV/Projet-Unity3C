using UnityEngine;
/// <summary>
/// Represents an object that defines a friction or drag coefficient.
/// </summary>
public class FrictionObject : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// The magnitude of the drag coefficient applied by this object.
    /// </summary>
    [SerializeField,Range(0,float.MaxValue)] private float _dragCoefficient = 10;
    /// <summary>
    /// Gets the drag coefficient as a negative value for friction calculations.
    /// </summary>
    public float DragCoefficient => -_dragCoefficient;

    
}
