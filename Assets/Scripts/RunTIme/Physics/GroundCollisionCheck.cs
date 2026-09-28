using Unity.VisualScripting;
using UnityEngine;

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
    /// <summary>
    /// Maximum angle, in degrees, at which a surface is considered ground.
    /// </summary>
    [SerializeField,Range(1,90)] private float _maxAngle = 45;
    /// <summary>
    /// Distance tolerance used when resolving collider overlaps.
    /// </summary>
    private float _distanceSlop = 0.05f;
    /// <summary>
    /// Direction along the ground surface.
    /// </summary>
    private Vector2 _right = Vector2.right;
    /// <summary>
    /// Current averaged ground normal.
    /// </summary>
    private Vector2 _normal = Vector2.zero;
    /// <summary>
    /// Indicates whether a valid ground collision was detected during the current check.
    /// </summary>
    private bool _isGrounded = false;
    /// <summary>
    /// Movement required to bring the body to the detected contact surface.
    /// </summary>
    private Vector2 _moveToContact = Vector2.zero;
    /// <summary>
    /// Processes the raycast results and resolves ground collisions.
    /// Updates the body's velocity and movement according to the detected surfaces,
    /// and stores the resulting ground information in the configured collision info.
    /// </summary>
    /// <param name="frameData">Physics data for the current frame.</param>
    /// <param name="rayCasts">Array containing the raycast results.</param>
    /// <param name="rayCount">Number of valid raycast results in the array.</param>
    override protected void ExecuteChildCollision(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts,int rayCount)
    {
        _moveToContact = Vector2.zero;
        _right = Vector2.right;
        _normal = Vector2.zero;
        _isGrounded = false;
        if (!_body)
        {
            Debug.LogError("no body set please set it manualy");
            return;
        }
        
        if (rayCount != 0)
        {
            for (int i = 0; i < rayCount; ++i)
            {
                RaycastHit2D hit = rayCasts[i];
                if (hit.collider == Collider)
                {
                    Debug.LogError("shound not be colliding with himself change layer");
                    continue;
                }

                float hitDistance = hit.distance;
                Vector2 hitNormal = hit.normal;
                _normal += hit.normal;

                float traveled = Mathf.Max(0, hitDistance);
                if (Mathf.Approximately(traveled, 0))
                    this.ResolveOverlap(hit.collider);

                _moveToContact += frameData.MoveNormalized * traveled;




                float dot = Vector2.Dot(_body.Velocity, hitNormal);
                if (dot < 0f)
                {
                        Vector2 vTangent = _body.Velocity - dot * hitNormal;
                        _body.SetVelocity(vTangent);
                    
                }

                frameData.Move = _moveToContact + _body.Velocity * Time.deltaTime;
            }
            _normal = _normal.normalized;
            float angle = Vector2.Angle(_normal, Vector2.up);

            if ( angle >_maxAngle)
            {
                _right = Vector2.right;
            }
            else
            {          
                _right.x = _normal.y;
                _right.y = -_normal.x;
            }
            _isGrounded = angle > 90.0f ? false : true;


        }

        if (_info)
        {
            _info.IsGrounded = _isGrounded;
            _info.Right = _right;
            _info.Up = _normal;
        }
    }
    private void ResolveOverlap(Collider2D other)
    {
        ColliderDistance2D dist = Strategy.ProcessDistance(other);
        if (dist.distance >= 0)
        {

            _moveToContact += dist.normal * Mathf.Max(dist.distance - _distanceSlop, 0);
        }
        else
        {

            _moveToContact += dist.normal * Mathf.Min(dist.distance + _distanceSlop, 0);
        }

    }
}
