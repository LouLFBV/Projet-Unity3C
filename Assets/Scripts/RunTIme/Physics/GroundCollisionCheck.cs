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
    #region --- REFERENCES ---
    /// <summary>
    /// Physics body whose movement and velocity are handled by this collision check.
    /// </summary>
    [Header("ChildProperties")]
    [SerializeField] private PhysicBody _body;
    /// <summary>
    /// Collider associated with this collision check.
    /// </summary>


    Collider2D _collider = null;
    #endregion

    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the current collision frame.
    /// </summary>
    FramePhysicsData _physicData = new();

    /// <summary>
    /// Minimum collision normal detected during the current collision step.
    /// </summary>
    private Vector2 _minNormal = Vector2.zero;
    /// <summary>
    /// Maximum number of iterations performed when resolving collisions.
    /// </summary>
    [SerializeField, UnityEngine.Range(1, 10)] private int _maxIteration = 5;
    #endregion

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes ground collisions and iteratively resolves detected overlaps
    /// and movement constraints.
    /// </summary>
    /// <param name="hits">Array of raycast hits detected during the collision check.</param>
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
       
        if(!_body)
        {
            Debug.LogError("no body set please set it manualy");
            return;
        }

       
       
        int iteration = 0;
        for (int i = 0; i < _maxIteration; i++ , iteration++)
        {
            if (!StepCollision2(hits))
                break;
        }
        
    
    }
    /// <summary>
    /// Performs a single collision resolution step using the detected raycasts.
    /// Updates the body's velocity to prevent movement into the collision surface.
    /// </summary>
    /// <param name="rayCasts">Array of raycast hits used for collision processing.</param>
    /// <returns>
    /// <c>true</c> if a valid collision was found and resolved; otherwise, <c>false</c>.
    /// </returns>
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
            move = _body.Velocity.normalized  * minDist;

        Vector2 velocity = move / Time.fixedDeltaTime;
        Vector2 remainingVelocity = _body.Velocity - velocity;
        float dot = Vector2.Dot(remainingVelocity, _minNormal);
        if (dot < 0)
        {
            remainingVelocity -= _minNormal * dot ;
        }

        _body.SetVelocity(velocity + remainingVelocity);
        return true;
    }
    /// <summary>
    /// Processes the provided raycasts using the configured collision strategy and filter.
    /// </summary>
    /// <param name="frameData">Physics data describing the current frame.</param>
    /// <param name="rayCasts">Array of raycast hits to process.</param>
    /// <returns>The number of valid raycasts processed by the collision strategy.</returns>
    private int ProcessRayCasts(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts)
    {
       return Strategy.ProcessRayCast(frameData, rayCasts, Filter);   
    }
    /// <summary>
    /// Resolves an overlap between the physics body and another collider.
    /// Calculates a correction movement based on the collider distance and
    /// applies a small separation offset.
    /// </summary>
    /// <param name="other">Collider overlapping the physics body.</param>
    /// <param name="move">Movement vector used to resolve the overlap.</param>
    private void ResolveOverlap(Collider2D other, ref Vector2 move)
    {
        ColliderDistance2D dist = Strategy.ProcessDistance(_physicData, other);
        if (dist.distance >= 0)
        {

            move = dist.normal * Mathf.Max(dist.distance - 0.015f, 0);
        }
        else
        {

            move = dist.normal * Mathf.Min(dist.distance +0.015f, 0);
        }
    }
    #endregion
}


