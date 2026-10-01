using UnityEngine;
/// <summary>
/// Stores information about the current ground collision state.
/// </summary>
public class GroundCollisionInfo : MonoBehaviour
{
    /// <summary>
    /// Direction along the detected ground surface.
    /// </summary>
    public Vector2 Right { get; set; } = Vector2.right;
    /// <summary>
    /// Normal direction of the detected ground surface.
    /// </summary>
    public Vector2 Up { get; set; } = Vector2.up;
    /// <summary>
    /// Indicates whether the object is currently considered grounded.
    /// </summary>
    public bool IsGrounded { get; set; } = false;
}
