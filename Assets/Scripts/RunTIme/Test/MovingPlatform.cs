using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 _moveDirection = Vector2.right;
    [SerializeField] private PhysicBody _body;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _maxDist = 5f;
    private Vector2 _startPos;
    private void Start()
    {
        _startPos = _body.Position;
        _body.AddForce(_moveDirection * _moveSpeed, ForceType.Acceleration);

    }
    private void FixedUpdate()
    {

        Debug.Log($"Velocity: {_body.Velocity}");
        _body.Actions.Add(() =>
        {
            Vector2 newPos = _body.Velocity * Time.fixedDeltaTime + _body.Position;
            float distance = Vector2.Distance(_startPos, newPos);
            if (distance > _maxDist)
            {
                float delta = distance - _maxDist;

                _body.SetVelocity(-_body.Velocity.normalized * (delta / Time.fixedDeltaTime));
                _moveDirection = -_moveDirection;
                _body.AddForce(_moveDirection * _moveSpeed, ForceType.Acceleration);

            }
        });
    }
}
