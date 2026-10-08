using UnityEngine;

/// <summary>
/// Controls the movement of a platform along a defined direction.
/// Handles movement speed, travel distance, and direction reversal.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    #region --- MOVEMENT SETTINGS ---

    /// <summary>
    /// Direction in which the platform initially moves.
    /// </summary>
    [SerializeField] private Vector2 _moveDirection = Vector2.right;
    /// <summary>
    /// Physics body used to move the platform.
    /// </summary>
    [SerializeField] private PhysicBody _body;
    /// <summary>
    /// Movement speed of the platform.
    /// </summary>
    [SerializeField] private float _moveSpeed = 5.0f;
    /// <summary>
    /// Maximum distance the platform can travel from its current movement origin.
    /// </summary>
    [SerializeField] private float _maxDist = 5f;
    #endregion

    #region --- STATE ---

    /// <summary>
    /// Position used as the starting point for the platform's current movement segment.
    /// </summary>
    private Vector2 _startPos;
    /// <summary>
    /// Movement speed used during the previous physics update.
    /// </summary>
    private float _lastMoveSpeed = 0.0f;


    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the platform's movement settings and starts its movement.
    /// </summary>
    private void Start()
    {
        _lastMoveSpeed = _moveSpeed;
        _startPos = _body.Position;
        _moveDirection.Normalize();
        _body.SetVelocity(_moveDirection * _moveSpeed);
    }
    /// <summary>
    /// Updates the platform's velocity when its movement speed changes
    /// and schedules its position and direction updates.
    /// </summary>
    private void FixedUpdate()
    {
        if(_lastMoveSpeed != _moveSpeed)
        {
            _lastMoveSpeed = _moveSpeed;
            _body.SetVelocity(_moveDirection * _moveSpeed);
        }

        _body.Actions.Add(() =>
        {
            Vector2 newPos = _body.Position + _body.Velocity * Time.fixedDeltaTime;
            Vector2 offset = newPos - _startPos;
            float distance = offset.magnitude;

            if (distance >= _maxDist)
            {
                float delta = distance - _maxDist;
                _body.SetPosition(_startPos + _moveDirection * _maxDist);
                _moveDirection = -_moveDirection;
                _body.SetVelocity(_moveDirection * _moveSpeed);
                _startPos = _startPos + (-_moveDirection) * _maxDist;

            }
        });
    }
    #endregion
}
