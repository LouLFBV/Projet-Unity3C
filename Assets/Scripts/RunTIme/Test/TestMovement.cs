using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private PhysicBody _body;
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
    [SerializeField] private ColliderStrategy _strategy = null;
    private RaycastHit2D[] _hits = new RaycastHit2D[10];
    ContactFilter2D _filter = new ContactFilter2D();
    [SerializeField] private FramePhysicsData _data = new();
    private void Awake()
    {
        _filter.useLayerMask = true;
        _filter.layerMask = LayerMask.GetMask("Ground");
    }
    private void FixedUpdate()
    {
        _data.Move = Vector2.zero;
        _data.Pos = _body.Position;
        int rayCount = _strategy.ProcessRayCast(_data,_hits,_filter);   

        Vector2 hitNormal = Vector2.zero;
        for(int i = 0; i < rayCount; i++)
        {
            hitNormal += _hits[i].normal;
        }
        hitNormal.Normalize();
       


        if (_enableSwing)
        {
            Vector2 rope = _transform.position - (Vector3)_body.Position;
            Vector2 dir = rope.normalized;
            Vector2 tengant = new Vector2(dir.y, -dir.x);
            if (_input != Vector2.zero)
               { 
                 _body.AddForce(_input.x * _swingForce * tengant, ForceType.Force);
               }
           
            _body.Actions.Add(()=> {
                 Vector2 newBodyPos = _body.Position + _body.Velocity * Time.fixedDeltaTime;
                Vector2 newRope = _transform.position - (Vector3)newBodyPos;
                if(newRope.magnitude > _ropeLength)
                {
                    Vector2 romeDir = newRope.normalized;
                    float dot = Vector2.Dot(_body.Velocity, romeDir);
                   
                        Vector2 radialVelocity = romeDir * dot;
                        _body.SetVelocity(_body.Velocity - radialVelocity);
                    
                }
       
            });


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
     
             _body.AddForce(Vector2.right* _input.x * _moveForce, ForceType.Force);
        }
        {
            Vector2 vel = _body.Velocity;
            vel.y = 0;
            //_body.AddForce(-0.75 * vel.magnitude * Velocity.normalized, ForceType.Acceleration);
        }

        if ((Time.time - _jumpTime) < _jumpInterval  && hitNormal != Vector2.zero)
        {
            Vector2 vel = _body.Velocity;
            vel.y = 0;
            _body.SetVelocity(vel);
            _jumpDir = (hitNormal + Vector2.up * 2  ).normalized;
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
