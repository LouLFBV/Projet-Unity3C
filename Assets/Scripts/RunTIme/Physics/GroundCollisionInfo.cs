using UnityEngine;
/// <summary>
/// Stores information about the current ground collision state.
/// </summary>
public class GroundCollisionInfo : MonoBehaviour
{
    [HideInInspector]
    /// <summary>
    /// Direction along the detected ground surface.
    /// </summary>
    public Vector2 Right = Vector2.right;
    [HideInInspector]
    /// <summary>
    /// Normal direction of the detected ground surface.
    /// </summary>
    public Vector2 Up = Vector2.up;
    [HideInInspector]
    /// <summary>
    /// Indicates whether the object is currently considered grounded.
    /// </summary>
    public bool IsGrounded = false;
}
