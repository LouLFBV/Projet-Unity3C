using System;
using System.Linq;
using UnityEngine;

public class CollisionForceObject : MonoBehaviour
{
    //TODO remove this by generic
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private string layerName = "Ground";
    public RaycastHit2D SolveCollision(ForceObject force)
    { 
        RaycastHit2D result = Physics2D.BoxCast(collider.transform.position, collider.size, collider.transform.eulerAngles.z, force.Velocity.normalized,force.Velocity.magnitude,LayerMask.GetMask(layerName));
        return result;
    }
}
