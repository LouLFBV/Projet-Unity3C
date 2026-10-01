using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    [SerializeField] private bool _enableSwing = false;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _swingForce = 0.5f;
    [SerializeField] private float _ropeLength = 10.0f;
    private void FixedUpdate()
    {
     
        if (_enableSwing)
        {
            Vector2 rope = _transform.position - (Vector3)_body.Position;
            Vector2 dir = rope.normalized;
            Vector2 tengant = new Vector2(dir.y, -dir.x);

            if (_input != Vector2.zero)
                _body.AddForce(_input.x * _swingForce * tengant, ForceType.Force);
            float distance = rope.magnitude;
            Debug.Log($"Distance{distance} , ropeLength {_ropeLength}");

            if (distance > _ropeLength)
            {
                float stretch = distance - _ropeLength;
    
                    _body.AddForce(dir * stretch, ForceType.Velocity);
            }


            //if (distance > _ropeLength)
            //{
            //    float stretch = distance - _ropeLength;

            //    float radialVelocity = Vector2.Dot(_body.Velocity, dir);

            //    float tension = stretch * 5000f - radialVelocity * 200f;

            //    if (tension > 0)
            //        _body.AddForce(dir * tension, ForceType.Force);
            //}



       

        }
        else if (_input != Vector2.zero )
        {
          
             _body.AddForce(_infos.Right * _input.x * _moveForce, ForceType.Force);
        }
        if ((Time.time - _jumpTime) < _jumpInterval  && _infos.IsGrounded)
        { 
            _jumpDir = (_infos.Up + Vector2.up * 2  ).normalized;
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
