using UnityEngine;

public class ColliderCapsuleStrategy : ColliderStrategy
{
    /// <summary>
    /// Collision strategy that performs collision detection using a <see cref="CapsuleCollider2D"/>.
    /// </summary>

    [Header("ChildProperties")]
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    /// <summary>
    /// Initializes the capsule collider used by this collision strategy.
    /// Attempts to retrieve the collider from the current GameObject when none is assigned.
    /// </summary>
    private void Awake()
    {
        if (!_capsuleCollider)
            _capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
        if (!_capsuleCollider)
            Debug.LogError("no collider found set it manualy");
        else
            _collider = _capsuleCollider;
    }
    /// <summary>
    /// Performs a capsule cast using the current collider and frame movement data.
    /// </summary>
    /// <param name="data">Physics data containing the movement direction and magnitude.</param>
    /// <param name="rayCasts">Array used to store the collision results.</param>
    /// <param name="Filter">Contact filter used to determine which colliders are detected.</param>
    /// <returns>The number of colliders detected by the capsule cast.</returns>
    public override int ProcessRayCast(FramePhysicsData data, RaycastHit2D[] rayCasts, ContactFilter2D Filter) 
    {
        if (!_capsuleCollider)
            return 0;
        return Physics2D.CapsuleCast(_capsuleCollider.transform.position, _capsuleCollider.size + SizeSkinVec,_capsuleCollider.direction, _collider.transform.rotation.z, data.MoveNormalized, Filter, rayCasts, data.MoveMagnitude );
    }
    /// <summary>
    /// Calculates the distance between the current capsule collider and the target collider.
    /// The collision skin is temporarily added to the capsule collider during the calculation.
    /// </summary>
    /// <param name="target">Collider against which the distance is calculated.</param>
    /// <returns>The calculated distance information between the two colliders.</returns>
    public override ColliderDistance2D ProcessDistance(Collider2D target)
    {
        _capsuleCollider.size += SizeSkinVec;
        ColliderDistance2D result = Physics2D.Distance(_capsuleCollider, target);
        _capsuleCollider.size -= SizeSkinVec;
        return result;
    }
}
