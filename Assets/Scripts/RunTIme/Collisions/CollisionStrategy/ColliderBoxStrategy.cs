using UnityEngine;
/// <summary>
/// Collision strategy that performs collision detection using a <see cref="BoxCollider2D"/>.
/// </summary>
public class ColliderBoxStrategy : ColliderStrategy
{
    /// <summary>
    /// Box collider used for collision detection.
    /// </summary>
    [Header("ChildProperties")]
    [SerializeField] private BoxCollider2D _boxCollider;
    /// <summary>
    /// Initializes the box collider used by this collision strategy.
    /// Attempts to retrieve the collider from the current GameObject when none is assigned.
    /// </summary>
    private void Awake()
    {
        if (!_boxCollider)
            _boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (!_boxCollider)
            Debug.LogError("no collider found set it manualy");
        else
            _collider = _boxCollider;
    }
    /// <summary>
    /// Performs a box cast using the current collider and frame movement data.
    /// </summary>
    /// <param name="data">Physics data containing the movement direction magnitude and deltaPos.</param>
    /// <param name="rayCasts">Array used to store the collision results.</param>
    /// <param name="Filter">Contact filter used to determine which colliders are detected.</param>
    /// <returns>The number of colliders detected by the box cast.</returns>
    public override int ProcessRayCast(FramePhysicsData data,RaycastHit2D[] rayCasts, ContactFilter2D Filter) 
    {
        if (!_boxCollider)
            return 0;
       return  Physics2D.BoxCast((_boxCollider.transform.position - (Vector3)data.DeltaPos)+ (Vector3 )_boxCollider.offset, _boxCollider.size + SizeSkinVec, _collider.transform.rotation.z, data.MoveNormalized, Filter,rayCasts, data.MoveMagnitude);
    }
    /// <summary>
    /// Calculates the distance between the current box collider and the target collider.
    /// The collision skin is temporarily added to the box collider during the calculation.
    /// </summary>
    /// <param name="target">Collider against which the distance is calculated.</param>
    /// <returns>The calculated distance information between the two colliders.</returns>
    public override ColliderDistance2D ProcessDistance( Collider2D target) 
    {
        _boxCollider.size += SizeSkinVec;
        ColliderDistance2D result = Physics2D.Distance(_boxCollider, target);
        _boxCollider.size -= SizeSkinVec;
        return result;
    }
}
