using System;
using System.Linq;
using UnityEngine;

public class CollisionForceObject : MonoBehaviour
{
    //TODO remove this by generic
    [SerializeField] private BoxCollider2D collider;
    public Collider2D Collider => collider;
    [SerializeField] private string layerName = "Ground";
    public Collider2D[] SolveCollision()
    { 
        Collider2D[] result = Physics2D.OverlapBoxAll(collider.transform.position, collider.size, collider.transform.eulerAngles.z,LayerMask.GetMask(layerName));
        return result;
    }
    public RaycastHit2D SolveCollisionTest(ForceObject  force)
    {
        RaycastHit2D result = Physics2D.BoxCast(collider.transform.position, collider.size, collider.transform.eulerAngles.z, force.Velocity.normalized, force.Velocity.magnitude, LayerMask.GetMask(layerName));
        return result;
    }
}
