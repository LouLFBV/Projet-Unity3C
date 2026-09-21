using System;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public enum ForceType
{
    Acceleration,
    Force,
    Impulse,
    Velocity
}

public class ForceObject : MonoBehaviour
{
    [SerializeField]
    private float _mass;
    // _mass in KG
    public float Mass => _mass;
    // _mass in G
    public float MassG => _mass * 1000;
    // Velocity in m . s^-2
    public Vector2 Velocity { get; private set; } = Vector2.zero;
    private Vector2 m_allForces = Vector2.zero;
    [SerializeField] private CollisionForceObject collide; 
     public bool IsGrounded { get; private set; } = false;
    // force in N . m . s^-2
    public void AddForce(Vector2 force,ForceType type = ForceType.Acceleration)
    {
        switch (type)
        {
            case ForceType.Acceleration:
                m_allForces += force;
                break;
            case ForceType.Force:
                m_allForces += force * Mass;
                break;
            case ForceType.Impulse:
                Velocity += (force / Mass);
                break;
            case ForceType.Velocity:
                Velocity += force;
                break;
        }
    }

    public void ClearForces()
    {
        m_allForces = Vector2.zero;
    }

    public void SetVelocity(Vector2 velocity)
    {
        Velocity = velocity;
    }
    void SetMass(float mass)
    {
        _mass = mass;
    }
    void FixedUpdate()
    {
        // if mass null static object
        if (Mass == 0)
        {
            Velocity = Vector2.zero;
            return;
        }
        // calculate forces
        Velocity += (m_allForces / Mass) * Time.fixedDeltaTime;

        if(collide)
        {
           RaycastHit2D hit = collide.SolveCollision(this);

           if (hit.collider &&  hit.distance <= 0.03f)  
           {
                ColliderDistance2D distance = gameObject.GetComponent<BoxCollider2D>().Distance(hit.collider);
                Vector3 correction = distance.normal * (distance.distance);
                transform.Translate(correction);

                float vn = Vector2.Dot(Velocity, hit.normal);
                Velocity -= vn * hit.normal;
           }
           
        }

        Vector2 pos = Velocity * Time.fixedDeltaTime;
        gameObject.transform.Translate(pos);

        this.ClearForces();
    }
}
