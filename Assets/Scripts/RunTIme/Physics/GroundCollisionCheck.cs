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
    [SerializeField] private GroundCollisionInfo _info;
 
    Collider2D _collider = null;

    Vector2 _minNormal = new();
    Vector2 _acumulateNormal = Vector2.zero;
    FramePhysicsData _physicData = new();
    bool _bodyGrounded = false;

    [SerializeField, UnityEngine.Range(2, 10)] private int _maxIteration = 5;
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
       
        if(!_body)
        {
            Debug.LogError("no body set please set it manualy");
            return;
        }

       
        _acumulateNormal = Vector2.zero;
        _bodyGrounded = false;
        _physicData.DeltaPos = (Vector2)_body.transform.position - _body.Position;
        for (int i = 0; i < _maxIteration; i++)
        {
            if (!StepCollision2(hits))
                break;
        }
        if (!_info)
            return;
        
        _acumulateNormal.Normalize();
        if (_acumulateNormal != Vector2.zero)
        {
            float angle = Vector2.Angle(_acumulateNormal, Vector2.up);
                _bodyGrounded = angle > 90.0f ? false : true;
        }
        _info.IsGrounded = _bodyGrounded;
        _info.Right = Vector2.right;
        _info.Up = _acumulateNormal == Vector2.zero ? Vector2.up : _acumulateNormal;
    
    }

    private bool StepCollision2(RaycastHit2D[] rayCasts)
    {
        _physicData.Move = _body.Velocity * Time.fixedDeltaTime;
        int rayCount = this.ProcessRayCasts(ref _physicData, rayCasts);
        if (rayCount == 0)
            return false;

        float minDist = float.MaxValue;
         _minNormal = Vector2.zero;
        for(int i = 0; i < rayCount; ++i)
        {
            RaycastHit2D hit = rayCasts[i];
            if (hit.collider == Collider) continue;
            if (Vector2.Dot(_body.Velocity,hit.normal) >= 0f) continue;
            if(hit.distance < minDist)
            {
                minDist = hit.distance;
                _minNormal = hit.normal;
                _acumulateNormal += hit.normal;
            }
        }

        if (_minNormal == Vector2.zero)
            return false;
        _bodyGrounded = true;
        float dot = Vector2.Dot(_body.Velocity, _minNormal);
        if (dot < 0)
        {
            _body.SetVelocity(_body.Velocity - _minNormal * dot);
        }


        return true;
    }
    
    private int ProcessRayCasts(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts)
    {
       return Strategy.ProcessRayCast(frameData, rayCasts, Filter);   
    }

}   
