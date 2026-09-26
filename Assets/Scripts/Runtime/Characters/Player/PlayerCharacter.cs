using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;

    [Header("Animator")]
    [SerializeField] private PlayerAnimator _animatorPlayer;
    public PlayerAnimator AnimatorPlayerScript => _animatorPlayer;

    [Header("Collision")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _skinWidth = 0.01f;
    public BoxCollider2D Collider => _collider;
    public LayerMask GroundLayer => _groundLayer;
    public float SkinWidth => _skinWidth;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    public float maxMoveSpeed = 20f;
    public float groundAcceleration = 100f;
    public float groundDeceleration = 100f;
    public float airAcceleration = 100f;
    public float airDeceleration = 100f;
    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public float acceleration;

    [Header("Gravity")]
    [SerializeField] private float _fallingGravity = 15f;
    [SerializeField] private float _risingingGravity = 25f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float jumpInputBuffer = 0.1f;
    public float coyoteTime = 0.065f;

    [Header("Wall Jump")]   
    public bool canWallJump = false;
    public float wallJumpForce = 20f;
    public float wallJumpHorizontalForce = 10f;
    public float wallJumpGravity = 5f;
    public float lastWallJumpTime = float.MinValue;

    public bool IsGrounded => _collisionInfo._below;

    [Header("TP")]
    public float distanceToTP = 2f;
    public float FacingDirection { get; private set; } = 1f;



    [HideInInspector] public Vector2 velocity;
    private CollisionInfo _collisionInfo;
    public CollisionInfo CollisionInfo => _collisionInfo;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public float lastJumpInputTime = float.MinValue;
    [HideInInspector] public float lastGroundedTime = float.MinValue;
    [HideInInspector] public bool canCoyoteJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerStateMachine = new PlayerStateMachine();
        var states = new Dictionary<PlayerStateType, PlayerState>
        {
            { PlayerStateType.Idle, new IdleState(this, _playerStateMachine) },
            { PlayerStateType.Walk, new WalkState(this, _playerStateMachine) },
            { PlayerStateType.Run, new RunState(this, _playerStateMachine) },
            { PlayerStateType.Jump, new JumpState(this, _playerStateMachine) },
            { PlayerStateType.Fall, new FallState(this, _playerStateMachine) },
            { PlayerStateType.TP, new TPState(this, _playerStateMachine) },
            { PlayerStateType.WallJump, new WallJumpState(this, _playerStateMachine) },
            { PlayerStateType.VineSwing, new VineSwingState(this, _playerStateMachine) },
            { PlayerStateType.Death, new DeathState(this, _playerStateMachine) },
            { PlayerStateType.UI, new UIState(this, _playerStateMachine) }
        };
        _playerStateMachine.Initialized(states);
        _playerStateMachine.ChangeState(PlayerStateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        _playerStateMachine.Update();
        //if (_collisionInfo._left || _collisionInfo._right)
        //{
        //    _velocity.x = 0;
        //}

        // Si on touche le sol en descendant, ou le plafond en montant
        if ((_collisionInfo._below && velocity.y < 0) || (_collisionInfo._above && velocity.y > 0))
        {
            velocity.y = 0;
        }

        //ProcessJump();

        float gravity = velocity.y >= 0 ? _risingingGravity : canWallJump ? wallJumpGravity : _fallingGravity; // on choisit la gravité en fonction de la direction du mouvement
        velocity.y -= gravity * Time.deltaTime; // acc * delta = vitesse, Time.deltaTime pour l'accumulation

        float targetVelocityX = isSprinting && _collisionInfo._below ? moveInput.x * maxMoveSpeed : moveInput.x * _moveSpeed;
        //float acceleration;
        //if (_collisionInfo._below)
        //{
        //    acceleration = moveInput.x != 0 ? groundAcceleration : groundDeceleration;
        //}
        //else
        //{
        //    acceleration = moveInput.x != 0 ? airAcceleration : airDeceleration;

        //}
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, acceleration * Time.deltaTime);

        Vector2 deltaPosition = velocity * Time.deltaTime; // vitesse * delta = position

        _collisionInfo.Reset();
        ProcessMove(ref deltaPosition); // on modifie la position en fonction des collisions

        //if (_collisionInfo._below)
        //{
        //    lastGroundedTime = Time.time;
        //    canCoyoteJump = true;
        //    _animatorPlayer.AnimatorPlayer.SetBool("IsGrounded", true);
        //}// on met à jour le temps de la dernière fois que le personnage était au sol

        //if (!canWallJump)
        //{ 
        //    //transform.Translate(deltaPosition); // on donne la position au transform
        //}
        transform.Translate(deltaPosition);

        _animatorPlayer.SetMoveAnimation(velocity.x, maxMoveSpeed); // on met à jour l'animation en fonction de la vitesse

        if (moveInput.x > 0)
        {
            FacingDirection = 1f;
        }
        else if (moveInput.x < 0)
        {
            FacingDirection = -1f;
        }
    }

    private void FixedUpdate()
    {
        _playerStateMachine.FixedUpdate();
    }

    //private void ProcessJump()
    //{
    //    bool isJumpBuffered = Time.time - _lastJumpInputTime <= _jumpInputBuffer;
    //    if (!isJumpBuffered)
    //    {
    //        return;
    //    }
    //    if (_collisionInfo._below || (_canCoyoteJump && Time.time - _lastGroundedTime <= _coyoteTime))
    //    {
    //        _velocity.y = _jumpForce;
    //        _playerAnimator.AnimatorPlayer.SetTrigger("Jump");
    //        _lastJumpInputTime = float.MinValue; // Reset du jump input
    //        _canCoyoteJump = false;
    //    }
    //}

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
        this.moveInput = moveInput;
    }

    public void Jump()
    {
        lastJumpInputTime = Time.time;
    }

    public void Sprint(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    public void TP()
    {
        _playerStateMachine.PushState(PlayerStateType.TP);
    }
}
