using UnityEngine;
/// <summary>
/// Collision strategy that performs collision detection using a <see cref="CircleCollider2D"/>.
/// </summary>
public class ColliderCircleStrategy : ColliderStrategy
{
    /// <summary>
    /// Circle collider used for collision detection.
    /// </summary>

    [Header("ChildProperties")]
    [SerializeField] private CircleCollider2D _circleCollider;
    /// <summary>
    /// Initializes the circle collider used by this collision strategy.
    /// Attempts to retrieve the collider from the current GameObject when none is assigned.
    /// </summary>
    private void Awake()
    {
        if (!_circleCollider)
            _circleCollider = gameObject.GetComponent<CircleCollider2D>();

        if (!_circleCollider)
            Debug.LogError("no collider found set it manualy");
        else
            _collider = _circleCollider;
    }
    /// <summary>
    /// Performs a circle cast using the current collider and frame movement data.
    /// </summary>
    /// <param name="data">Physics data containing the movement direction magnitude and deltaPos.</param>
    /// <param name="rayCasts">Array used to store the collision results.</param>
    /// <param name="Filter">Contact filter used to determine which colliders are detected.</param>
    /// <returns>The number of colliders detected by the circle cast.</returns>
    public override int ProcessRayCast(FramePhysicsData data, RaycastHit2D[] rayCasts, ContactFilter2D Filter)
    {
        if (!_circleCollider)
            return 0;
        return Physics2D.CircleCast((Vector3)data.Pos + (Vector3)_circleCollider.offset, _circleCollider.radius + HalfSkin, data.MoveNormalized, Filter, rayCasts, data.MoveMagnitude);
    }
    /// <summary>
    /// Calculates the distance between the current circle collider and the target collider.
    /// The collision skin is temporarily added to the circle collider during the calculation.
    /// </summary>
    /// <param name="target">Collider against which the distance is calculated.</param>
    /// <returns>The calculated distance information between the two colliders.</returns>
    public override ColliderDistance2D ProcessDistance(FramePhysicsData data, Collider2D target)
    {
        Vector2 pos = _circleCollider.transform.position;
        _circleCollider.transform.position = data.Pos + _circleCollider.offset;
        _circleCollider.radius += HalfSkin;
        ColliderDistance2D result = Physics2D.Distance(_circleCollider, target);
        _circleCollider.radius -= HalfSkin;
        _circleCollider.transform.position = pos;

        return result;
    }
}
