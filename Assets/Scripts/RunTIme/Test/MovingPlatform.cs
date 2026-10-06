using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 _moveDirection = Vector2.right;
    [SerializeField] private PhysicBody _body;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _maxDist = 5f;
    private Vector2 _startPos;
    private float _lastMoveSpeed = 0.0f;

 
    private void Start()
    {
        _lastMoveSpeed = _moveSpeed;
        _startPos = _body.Position;
        _moveDirection.Normalize();
        _body.SetVelocity(_moveDirection * _moveSpeed);
    }
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

            if (distance > _maxDist)
            {
                float delta = distance - _maxDist;
                _body.SetPosition(_startPos + _moveDirection * _maxDist);
                _moveDirection = -_moveDirection;
                _body.SetVelocity(_moveDirection * _moveSpeed);
                _startPos = _startPos + (-_moveDirection) * _maxDist;

            }
        });
    }
}
