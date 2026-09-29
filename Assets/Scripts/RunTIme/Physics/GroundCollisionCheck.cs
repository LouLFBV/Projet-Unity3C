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
    /// Movement vector calculated during the ground collision resolution. 
    /// </summary>
    private Vector2 _move = Vector2.zero;
    /// <summary> /
    /// Minimum distance to the closest detected collision.
    /// </summary>
    float _minDist = 0.0f;
    /// <summary> 
    /// Current velocity used while resolving ground collisions. 
    /// </summary>
    private Vector2 _veclocity = Vector2.zero;
    /// <summary> 
    /// Collider associated with the closest detected ground collision. 
    /// </summary>
    Collider2D _collider = null;
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
        if (!_body)
        {
            Debug.LogError("no body set please set it manualy");
            return;
        }
        _right = Vector2.right;
        _normal = Vector2.zero;
        _isGrounded = false;

        if (rayCount != 0)
        {
            this.RealCollisionCheck(ref frameData, rayCasts, rayCount);
        }

        if (_info)
        {
            _info.IsGrounded = _isGrounded;
            _info.Right = _right;
            _info.Up = _normal;
        }
    }


    //private void Test(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts, int rayCount)
    //{
    //    uint _maxIteration = 5;
    //    FramePhysicsData _currentFrame = frameData;

    //    _currentFrame.DeltaPos += _move;
    //    _currentFrame.Move = _move;
    //    for(int i = 0; i < _maxIteration; ++i)
    //    {
    //       rayCount = Strategy.ProcessRayCast(_currentFrame, rayCasts, Filter);
    //    }
    //    frameData.Move = _move + _body.Velocity * Time.fixedDeltaTime;


    //}
    /// <summary>
    /// Processes the detected ground collisions and calculates the resulting movement,
    /// velocity, and ground normal.
    /// </summary>
    /// <param name="frameData">Physics data for the current frame.</param>
    /// <param name="rayCasts">Array containing the raycast results.</param>
    /// <param name="rayCount">Number of valid raycast results in the array.</param>
    private void RealCollisionCheck(ref FramePhysicsData frameData, RaycastHit2D[] rayCasts, int rayCount)
    {
        _minDist = float.MaxValue;
        _veclocity = _body.Velocity;
        _move = Vector2.zero;
        for (int i = 0; i < rayCount; ++i)
        {
            RaycastHit2D hit = rayCasts[i];
            if (hit.collider == Collider)
            {
                Debug.LogError("shound not be colliding with himself change layer");
                continue;
            }
            if (hit.distance < _minDist)
            {
                _minDist = hit.distance;
                _collider = hit.collider;
            }
            float dot = Vector2.Dot(_veclocity, hit.normal);
            if (dot < 0f)
            {
                _veclocity -= dot * hit.normal;
            }
            _normal += hit.normal;

        }

        if (Mathf.Approximately(_minDist, 0))
            _move = Vector2.zero;
        else
            _move = frameData.MoveNormalized * _minDist;

        _collider = null;
        _normal.Normalize();
        _body.SetVelocity(_veclocity);
        frameData.Move = _move + _body.Velocity * Time.fixedDeltaTime;
        float angle = Vector2.Angle(_normal, Vector2.up);
        Debug.Log($"normal : {_normal},Up : {Vector2.up}, Angle : {angle}");
        _isGrounded = angle > 90.0f ? false : true;
    }
    //private void ResolveOverlap(Collider2D other,ref Vector2 move)
    //{
    //    ColliderDistance2D dist = Strategy.ProcessDistance(other);
    //    if (dist.distance >= 0)
    //    {

    //        move = dist.normal * Mathf.Max(dist.distance - _distanceSlop, 0);
    //    }
    //    else
    //    {

    //        move = dist.normal * Mathf.Min(dist.distance + _distanceSlop, 0);
    //    }

    //}
}   
