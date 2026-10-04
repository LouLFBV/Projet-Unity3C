using NUnit.Framework;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//TODO handle complex collision with multiple raycasts  

/// <summary>
/// Handles ground collision detection and resolution for a <see cref="PhysicBody"/>.
/// Determines whether the body is grounded, calculates the ground normal,
/// and adjusts the body's movement and velocity when a collision occurs.
/// </summary>
public class GroundCollisionCheck : CollisionCheck
{
    [Header("ChildProperties")]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /// <summary>
    /// Physics body whose movement and velocity are handled by this collision check.
    /// </summary>
    [SerializeField] private PhysicBody _body;
    /// <summary>
    /// Collision information updated with the current ground state and surface directions.
    /// </summary>


    Collider2D _collider = null;
    FramePhysicsData _physicData = new();

    private Vector2 _minNormal = Vector2.zero;
    [SerializeField, UnityEngine.Range(1, 10)] private int _maxIteration = 5;
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {

        if (!_body)
        {
            Debug.LogError("no body set please set it manualy");
            return;
        }



        int iteration = 0;
        for (int i = 0; i < _maxIteration; i++, iteration++)
        {
            if (!StepCollision2(hits))
                break;
        }


    }

    private bool StepCollision2(RaycastHit2D[] rayCasts)
    {
        _physicData.Move = _body.Velocity * Time.fixedDeltaTime;
        _physicData.Pos = _body.Position;
        int rayCount = this.ProcessRayCasts(ref _physicData, rayCasts);
        if (rayCount == 0)
            return false;

        float minDist = float.MaxValue;
        _minNormal = Vector2.zero;
        int minIndex = 0;
        for (int i = 0; i < rayCount; ++i)
        {
            RaycastHit2D hit = rayCasts[i];
            if (hit.collider == Collider) continue;
            if (Vector2.Dot(_body.Velocity, hit.normal) >= 0f) continue;
            if (hit.distance < minDist)
            {
                minDist = hit.distance;
                _minNormal = hit.normal;
                minIndex = i;
            }

        }

        if (_minNormal == Vector2.zero)
            return false;
        Vector2 move = new();
        if (minDist <= 0.0001f)
            ResolveOverlap(rayCasts[minIndex].collider, ref move);
        else
            move = _body.Velocity.normalized * minDist;
        //_body.SetPosition(_body.Position + move);

        Vector2 velocity = move / Time.fixedDeltaTime;
        Vector2 remainingVelocity = _body.Velocity - velocity;
        float dot = Vector2.Dot(remainingVelocity, _minNormal);
        if (dot < 0)
        {
            remainingVelocity -= _minNormal * dot;
        }

        _body.SetVelocity(velocity + remainingVelocity);
        return true;
    }

    private int ProcessRayCasts(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts)
    {
        return Strategy.ProcessRayCast(frameData, rayCasts, Filter);
    }
    private void ResolveOverlap(Collider2D other, ref Vector2 move)
    {
        ColliderDistance2D dist = Strategy.ProcessDistance(_physicData, other);
        if (dist.distance >= 0)
        {

            move = dist.normal * Mathf.Max(dist.distance - 0.015f, 0);
        }
        else
        {

            move = dist.normal * Mathf.Min(dist.distance + 0.015f, 0);
        }
    }
}

