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
    private void FixedUpdate()
    {
        if (_input != Vector2.zero)
        {
            _body.AddForce(_infos.Right * _input.x * _moveForce, ForceType.Force);
        }
        if (_jump != 0 && _infos.IsGrounded)
        { 
            _jumpDir = (_infos.Up + Vector2.up  ).normalized;
            _body.AddForce(_jumpDir * _jumpForce,
            ForceType.Impulse);
        }
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>();
        _input.y = 0;
        if(_input.x != 0)
        _input.x = _input.x > 0 ? 1 : - 1;
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        _jump = ctx.ReadValue<float>();
    }
}
