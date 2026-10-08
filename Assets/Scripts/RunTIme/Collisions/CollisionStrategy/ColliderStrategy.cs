using UnityEngine;
/// <summary>
/// Base class for collider-specific strategies used to perform
/// raycast and distance-based collision queries.
/// </summary>
public abstract class ColliderStrategy : MonoBehaviour
{
    #region --- COLLIDER ---

    /// <summary>
    /// Collider associated with this strategy.
    /// </summary>
    protected Collider2D _collider;
    /// <summary>
    /// Gets the collider associated with this strategy.
    /// </summary>
    public Collider2D Collider => _collider;
    #endregion

    #region --- COLLISION SETTINGS ---

    /// <summary>
    /// Defines the half-width of the collision skin.
    /// </summary>
    [Header("Properties")]
    [SerializeField, Range(0.0f, 1.0f)] private float _halfSkin = 0.05f;
    /// <summary>
    /// Gets the half-width of the collision skin.
    /// </summary>
    public float HalfSkin => _halfSkin;
    /// <summary>
    /// Gets the full width of the collision skin.
    /// </summary>
    public float Skin => HalfSkin * 2.0f;
    /// <summary>
    /// Gets a vector containing the half-skin value on both axes.
    /// </summary>
    public Vector2 HalfSizeSkinVec => Vector2.one * HalfSkin;
    /// <summary>
    /// Gets a vector containing the full skin size on both axes.
    /// </summary>
    public Vector2 SizeSkinVec => 2.0f * HalfSizeSkinVec;
    #endregion

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes raycasts using the provided frame physics data and contact filter.
    /// </summary>
    /// <param name="data">
    /// Physics data for the current frame.
    /// </param>
    /// <param name="rayCasts">
    /// Array used to store the raycast results.
    /// </param>
    /// <param name="Filter">
    /// Contact filter used to determine which colliders can be detected.
    /// </param>
    /// <returns>
    /// The number of valid raycast results.
    /// </returns>

    public abstract int ProcessRayCast(FramePhysicsData data, RaycastHit2D[] rayCasts, ContactFilter2D Filter);
    /// <summary>
    /// Processes the distance between this strategy's collider and the target collider.
    /// </summary>
    /// <param name="target">
    /// Collider against which the distance is evaluated.
    /// </param>
    /// <returns>
    /// The calculated distance information between the two colliders.
    /// </returns>
    public abstract ColliderDistance2D ProcessDistance(FramePhysicsData data, Collider2D target);
#endregion
}