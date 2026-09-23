using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private PlayerAnimator _playerAnimator;

    [Header("Collision")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _skinWidth = 0.01f;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _maxMoveSpeed = 20f;
    [SerializeField] private float _groundAcceleration = 100f;
    [SerializeField] private float _groundDeceleration = 100f;
    [SerializeField] private float _airAcceleration = 100f;
    [SerializeField] private float _airDeceleration = 100f;

    [Header("Gravity")]
    [SerializeField] private float _fallingGravity = 15f;
    [SerializeField] private float _risingingGravity = 25f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 20f;
    [SerializeField] private float _jumpInputBuffer = 0.1f;
    [SerializeField] private float _coyoteTime = 0.065f;



    private Vector2 _velocity;
    private CollisionInfo _collisionInfo;

    private Vector2 _moveInput;
    private float _lastJumpInputTime = float.MinValue;
    private float _lastGroundedTime = float.MinValue;
    private bool _canCoyoteJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (_collisionInfo._left || _collisionInfo._right)
        //{
        //    _velocity.x = 0;
        //}

        if (_collisionInfo._below || _collisionInfo._above)
        {
            _velocity.y = 0;
        }

        ProcessJump();

        float gravity = _velocity.y >= 0 ? _risingingGravity : _fallingGravity; // on choisit la gravité en fonction de la direction du mouvement
        _velocity.y -= gravity * Time.deltaTime; // acc * delta = vitesse, Time.deltaTime pour l'accumulation

        float targetVelocityX = _moveInput.x * _moveSpeed;
        float acceleration;
        if (_collisionInfo._below)
        {
            acceleration = _moveInput.x != 0 ? _groundAcceleration : _groundDeceleration;
        }
        else
        {
            acceleration = _moveInput.x != 0 ? _airAcceleration : _airDeceleration;
            _playerAnimator.AnimatorPlayer.SetFloat("JumpVelocity", _velocity.y);
            _playerAnimator.AnimatorPlayer.SetBool("IsGrounded", false);
        }
        _velocity.x = Mathf.MoveTowards(_velocity.x, targetVelocityX, acceleration * Time.deltaTime);

        Vector2 deltaPosition = _velocity * Time.deltaTime; // vitesse * delta = position

        _collisionInfo.Reset();
        ProcessMove(ref deltaPosition); // on modifie la position en fonction des collisions

        if (_collisionInfo._below)
        {
            _lastGroundedTime = Time.time;
            _canCoyoteJump = true;
            _playerAnimator.AnimatorPlayer.SetBool("IsGrounded", true);
        }// on met à jour le temps de la dernière fois que le personnage était au sol

        transform.Translate(deltaPosition); // on donne la position au transform
        _playerAnimator.SetMoveAnimation(_velocity.x, _maxMoveSpeed); // on met à jour l'animation en fonction de la vitesse
    }

    private void ProcessJump()
    {
        bool isJumpBuffered = Time.time - _lastJumpInputTime <= _jumpInputBuffer;
        if (!isJumpBuffered)
        {
            return;
        }
        if (_collisionInfo._below || (_canCoyoteJump && Time.time - _lastGroundedTime <= _coyoteTime))
        {
            _velocity.y = _jumpForce;
            _playerAnimator.AnimatorPlayer.SetTrigger("Jump");
            _lastJumpInputTime = float.MinValue; // Reset du jump input
            _canCoyoteJump = false;
        }
    }

    private void ProcessMove(ref Vector2 deltaPosition)
    {

        if (deltaPosition.x != 0)
        {
            ProcessHorizontalCollisions(ref deltaPosition);
        }

        if (deltaPosition.y != 0)
        {
            ProcessVerticalCollisions(ref deltaPosition);
        }
    }

    private void ProcessVerticalCollisions(ref Vector2 deltaPosition)
    {
        float directionY = Mathf.Sign(deltaPosition.y);
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + new Vector3(deltaPosition.x, 0),
            _collider.size,
            0,
            Vector2.up * directionY,
            Mathf.Abs(deltaPosition.y) + _skinWidth,
            _groundLayer
            );

        if (hit)
        {
            deltaPosition.y = Mathf.Max(0, hit.distance - _skinWidth) * directionY;
            _collisionInfo._below = directionY < 0;
            _collisionInfo._above = directionY > 0;

        }
    }

    private void ProcessHorizontalCollisions(ref Vector2 deltaPosition)
    {
        float directionX = Mathf.Sign(deltaPosition.x);
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            _collider.size,
            0,
            Vector2.right * directionX,
            Mathf.Abs(deltaPosition.x) + _skinWidth,
            _groundLayer
            );

        if (hit)
        {
            deltaPosition.x = Mathf.Max(0, hit.distance - _skinWidth) * directionX;
            _collisionInfo._left = directionX < 0;
            _collisionInfo._right = directionX > 0;
        }
    }

    public void Move(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    public void Jump()
    {
        _lastJumpInputTime = Time.time;
    }
}
