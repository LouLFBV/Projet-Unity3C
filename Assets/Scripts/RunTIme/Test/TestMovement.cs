using UnityEngine;
using UnityEngine.InputSystem;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private PhysicBody _body;
    [SerializeField] private GroundCollisionInfo _infos;
    private Vector2 _input = Vector2.zero;
    [SerializeField] private float _moveForce = 10.0f;

    private float _jump = 0.0f;
    [SerializeField] private float _jumpForce = 10.0f;
    private Vector2 _jumpDir = Vector2.zero;

private float _jumpTime = float.MinValue;
    [SerializeField] private float _jumpInterval = 0.1f;
    private void FixedUpdate()
    {
        if (_input != Vector2.zero)
        {
            _body.AddForce(_infos.Right * _input.x * _moveForce, ForceType.Force);
        }
        if ((Time.time - _jumpTime) < _jumpInterval  && _infos.IsGrounded)
        { 
            _jumpDir = (_infos.Up + Vector2.up  ).normalized;
            _body.AddForce(_jumpDir * _jumpForce,
            ForceType.Impulse);
            _jumpTime = float.MinValue;
        }
    }

    public void Move(Vector2 input)
    {
        _input = input;
        _input.y = 0;
        if (_input.x != 0)
            _input.x = _input.x > 0 ? 1 : -1;
    }
    public void Jump( float jump)
    {
        if (jump != 0)
            _jumpTime = Time.time;
    }
}
