using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region Champs/Attributs
    private PlayerStateMachine _playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;

    [Header("Physics")]
    [SerializeField] private PhysicBody _body;
    [SerializeField] private ColliderStrategy _strategy = null;
    [SerializeField] private FramePhysicsData _data = new();
    ContactFilter2D _filter = new ContactFilter2D();
    private RaycastHit2D[] _hits = new RaycastHit2D[10];
    //[HideInInspector] public Vector2 velocity;
    public PhysicBody Body => _body;

    [Header("Collision")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _skinWidth = 0.01f;
    private CollisionInfo _collisionInfo;
    public BoxCollider2D Collider => _collider;
    public LayerMask GroundLayer => _groundLayer;
    public float SkinWidth => _skinWidth;
    public CollisionInfo CollisionInfo => _collisionInfo;
    public Vector2 HitNormal { get; set; } = Vector2.zero;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _runSpeed = 20f;
    public float groundDeceleration = 100f;
    public float groundAcceleration = 10f;
    public float airAcceleration = 100f;
    public float airDeceleration = 100f;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public float acceleration;
    private float _moveSpeed;
    private float _maxMoveSpeed;
    private float _targetVelocityX;
    public float CurrentAcceleration { get; set; }
    public float CurrentDeceleration { get; set; }
    public float MaxMoveSpeed => _maxMoveSpeed;
    public float FacingDirection { get; private set; } = 1f;
    public bool IsSprinting { get; set; } = false;

    [Header("Gravity")]
    [SerializeField] private float _fallingGravity = 15f;
    [SerializeField] private float _risingingGravity = 25f;

    [Header("Jump")]
    public float jumpForce = 20f;
    public float jumpInputBuffer = 0.1f;
    public float coyoteTime = 0.065f;
    public Vector2 jumpDir = Vector2.zero;
    [HideInInspector] public float lastJumpInputTime = float.MinValue;
    [HideInInspector] public float lastGroundedTime = float.MinValue;
    [HideInInspector] public bool canCoyoteJump = false;

    [Header("Wall Jump")]
    [SerializeField] private float _castDistanceToWallJump = 0.3f;
    public float wallJumpGravity = 5f;
    public bool canWallJump = false;
    public float CastDistanceToWallJump => _castDistanceToWallJump;

    [Header("TP")]
    [SerializeField] private ManaSystem _manaSystem;
    public float costTP = 30f;
    public float distanceToTP = 2f;
    public ManaSystem ManaSystem => _manaSystem;

    [Header("Animator")]
    [SerializeField] private PlayerAnimator _animatorPlayer;
    public float TargetAnimSpeed { get; set; } = 0f;
    public PlayerAnimator AnimatorPlayerScript => _animatorPlayer;

    public event Action OnSprint;
    public event Action OnGround;
    public event Action OnJump;
    public event Action OnTP;
    public event Action OnHurt;
    public event Action OnAttack;
    #endregion

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

        _filter.useTriggers = false; 
        _filter.useLayerMask = true;
        _filter.layerMask = _groundLayer;
    }

    // Update is called once per frame
    void Update()
    {
        _playerStateMachine.Update();
        float targetForce = (IsSprinting && HitNormal != Vector2.zero) ? _runSpeed : _walkSpeed;
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
            TargetAnimSpeed = (IsSprinting && HitNormal != Vector2.zero) ? _runSpeed : _walkSpeed;
        }

        //_animatorPlayer.SetMoveAnimation(TargetAnimSpeed, _maxMoveSpeed, FacingDirection);
    }

    private void FixedUpdate()
    {

        _data.Move = Vector2.zero;
        _data.Pos = _body.Position;
        int rayCount = _strategy.ProcessRayCast(_data, _hits, _filter);

        HitNormal = Vector2.zero;
        for (int i = 0; i < rayCount; i++)
        {
            HitNormal += _hits[i].normal;
        }
        HitNormal.Normalize();

        Debug.Log($"HitNormal: {HitNormal}");
        if (moveInput.x != 0)
        {
            // On applique directement la vitesse souhaitée

            _body.AddForce(Vector2.right * moveInput.x * _moveSpeed, ForceType.Force);
        }

        _playerStateMachine.FixedUpdate();
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
