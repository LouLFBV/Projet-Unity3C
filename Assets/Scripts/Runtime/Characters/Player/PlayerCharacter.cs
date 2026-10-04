using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;

    [Header("Physics")]
    [SerializeField] private PhysicBody _body;
    [SerializeField] private GroundCollisionInfo _infos;

    public PhysicBody Body => _body;
    public GroundCollisionInfo GroundInfos => _infos;

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
    private float _moveSpeed ;
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _runSpeed = 20f;
    private float _maxMoveSpeed ;
    public float groundDeceleration = 100f; 
    public float groundAcceleration = 10f;
    public float airAcceleration = 100f;
    public float airDeceleration = 100f;

    public event Action OnSprint;
    public event Action OnGround;
    public bool IsSprinting { get; set; } = false;
    [HideInInspector] public float acceleration;
    public float CurrentAcceleration { get; set; }
    public float CurrentDeceleration { get; set; }
    public float MaxMoveSpeed => _maxMoveSpeed;


    [Header("Gravity")]
    [SerializeField] private float _fallingGravity = 15f;
    [SerializeField] private float _risingingGravity = 25f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float jumpInputBuffer = 0.1f;
    public float coyoteTime = 0.065f;
    public Vector2 jumpDir  = Vector2.zero;
    public event Action OnJump;

    [Header("Wall Jump")]   
    public bool canWallJump = false;
    public float wallJumpGravity = 5f;
    [SerializeField] private float _castDistanceToWallJump = 0.3f;
    public float CastDistanceToWallJump => _castDistanceToWallJump;


    [Header("TP")]
    [SerializeField] private ManaSystem _manaSystem;
    public ManaSystem ManaSystem => _manaSystem;
    public float costTP = 30f;
    public float distanceToTP = 2f;
    public event Action OnTP;
    public float FacingDirection { get; private set; } = 1f;


    private float _targetVelocityX;
    //[HideInInspector] public Vector2 velocity;
    private CollisionInfo _collisionInfo;
    public CollisionInfo CollisionInfo => _collisionInfo;

    public event Action OnHurt;
    public event Action OnAttack;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public float lastJumpInputTime = float.MinValue;
    [HideInInspector] public float lastGroundedTime = float.MinValue;
    [HideInInspector] public bool canCoyoteJump = false;

    public float TargetAnimSpeed { get; set; } = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerStateMachine = new PlayerStateMachine(this);

        _playerStateMachine.Register(new TPState(this));
        _playerStateMachine.Register(new IdleState(this), true);
        _playerStateMachine.Register(new WalkState(this));
        _playerStateMachine.Register(new RunState(this));
        _playerStateMachine.Register(new JumpState(this));
        _playerStateMachine.Register(new FallState(this));
        _playerStateMachine.Register(new WallSlideState(this));
        _playerStateMachine.Register(new VineSwingState(this));
        _playerStateMachine.Register(new DeathState(this));

        _moveSpeed = _walkSpeed;
        _maxMoveSpeed = _runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _playerStateMachine.Update();
        float targetForce = (IsSprinting && _infos.IsGrounded) ? _runSpeed : _walkSpeed;
        float currentAcc = moveInput.x != 0 ? CurrentAcceleration : CurrentDeceleration;
        _moveSpeed = Mathf.MoveTowards(_moveSpeed, targetForce, currentAcc * Time.deltaTime);

        if (moveInput.x > 0)
        {
            FacingDirection = 1f;
        }
        else if (moveInput.x < 0)
        {
            FacingDirection = -1f;
        }

        if (moveInput.x != 0) 
        {
            TargetAnimSpeed = (IsSprinting && _infos.IsGrounded) ? _runSpeed : _walkSpeed;
        }

        //_animatorPlayer.SetMoveAnimation(TargetAnimSpeed, _maxMoveSpeed, FacingDirection);
    }

    private void FixedUpdate()
    {
        _playerStateMachine.FixedUpdate();

        if (moveInput.x != 0)
        {
            // On applique directement la vitesse souhaitée

            _body.AddForce(_infos.Right * moveInput.x * _moveSpeed, ForceType.Force);
        }
    }

    public int GetPlayerDirection()
    {
        return  moveInput.x > 0 ? 1 : moveInput.x < 0 ? -1 : 0;
    }

    #region Input Methods
    public void Move(Vector2 moveInput) => this.moveInput = moveInput;
    public void Jump() => lastJumpInputTime = Time.time;
    public void Sprint(bool isSprinting) => IsSprinting = isSprinting;
    public void TP() => _playerStateMachine.CurrentState.SetPushState<TPState>();
    #endregion

    #region Event Methods
    public void TriggerSprint() => OnSprint?.Invoke();
    public void TriggerTP() => OnTP?.Invoke();
    public void TriggerHurt() => OnHurt?.Invoke();
    public void TriggerJump() => OnJump?.Invoke();
    public void TriggerGround() => OnGround?.Invoke();
    public void TriggerAttack() => OnAttack?.Invoke();
    #endregion
}
