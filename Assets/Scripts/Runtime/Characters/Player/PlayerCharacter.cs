using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    #region Champs/Attributs
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    private PlayerStateMachine _playerStateMachine;
    [SerializeField] private PlayerUIManager _playerUIManager;

    #region Physics 
    [Header("Physics")]
    [SerializeField] private PhysicBody _body;
    [SerializeField] private ColliderStrategy _strategy = null;
    [SerializeField] private FramePhysicsData _data = new();

    public PhysicBody Body => _body;

    private ContactFilter2D _filter = new ContactFilter2D();
    private RaycastHit2D[] _hits = new RaycastHit2D[10];
    #endregion

    #region Collision 
    [Header("Collision")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _skinWidth = 0.01f;

    public BoxCollider2D Collider => _collider;
    public LayerMask GroundLayer => _groundLayer;
    public float SkinWidth => _skinWidth;
    public Vector2 HitNormal { get; set; } = Vector2.zero;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _runSpeed = 20f;
    [SerializeField] private float _groundDeceleration = 10f;
    [SerializeField] private float _groundAcceleration = 20f;
    [SerializeField] private float _airAcceleration = 10f;
    [SerializeField] private float _airDeceleration = 20f;

    public float GroundDeceleration => _groundDeceleration;
    public float GroundAcceleration => _groundAcceleration;
    public float AirAcceleration => _airAcceleration;
    public float AirDeceleration => _airDeceleration;
    public float CurrentAcceleration { get; set; }
    public float CurrentDeceleration { get; set; }
    public float MaxMoveSpeed => _maxMoveSpeed;
    public float FacingDirection { get; set; } = 1f;
    public bool IsSprinting { get; set; } = false;

    private Vector2 _moveInput;
    private float _moveSpeed;
    private float _maxMoveSpeed;
    #endregion

    #region Gravity 
    [Header("Gravity")]

    [SerializeField] private float _fallingGravity = 15f;
    [SerializeField] private float _risingingGravity = 25f;

    public float FallingGravity => _fallingGravity;
    public float RisingingGravity => _risingingGravity;
    #endregion

    #region Jump 
    [Header("Jump")]
    [SerializeField] private float _jumpForce  = 20f;
    [SerializeField] private float _jumpInputBuffer = 0.1f;
    [SerializeField] private float _coyoteTime  = 0.065f;
    public float JumpForce => _jumpForce;
    public float JumpInputBuffer => _jumpInputBuffer;
    public float CoyoteTime => _coyoteTime;
    public Vector2 JumpDir { get; set; } = Vector2.zero;
    public float LastJumpInputTime { get; set; } = float.MinValue;
    public float LastGroundedTime { get; set; } = float.MinValue;
    public bool CanCoyoteJump { get; set; } = false;
    #endregion

    #region Wall Jump
    [Header("Wall Jump")]
    [SerializeField] private float _castDistanceToWallJump = 0.3f;
    [SerializeField] private float _wallJumpGravity = 0.3f;

    public float WallJumpGravity => _wallJumpGravity;
    public bool CanWallJump { get; set; } = false;
    public float CastDistanceToWallJump => _castDistanceToWallJump;
    #endregion

    #region TP
    [Header("TP")]
    [SerializeField] private ManaSystem _manaSystem;
    [SerializeField] private float _costTP = 30f;
    [SerializeField] private float _distanceTP = 2f;
    [SerializeField] private float _timeScaleInTP = 0.1f;
    [SerializeField] private GameObject _TPZone;
    [SerializeField] private GameObject _spriteGhost;
    public float CostTP => _costTP;
    public float DistanceTP => _distanceTP;
    public ManaSystem ManaSystem => _manaSystem;
    public bool IsInTP { get; set; } = false;
    public bool IsCancelTP { get; set; } = false;
    public float TimeScaleInTP => _timeScaleInTP;
    public GameObject TPZone => _TPZone;
    public GameObject SpriteGhost => _spriteGhost;
    #endregion

    #region Animator
    [Header("Animator")]

    [SerializeField] private PlayerAnimator _animatorPlayer;
    public PlayerAnimator AnimatorPlayerScript => _animatorPlayer;
    public float TargetAnimSpeed { get; private set; } = 0f;
    #endregion

    #region Events
    public event Action OnSprint;
    public event Action OnGround;
    public event Action OnJump;
    public event Action OnTP;
    public event Action OnStartTP;
    public event Action OnHurt;
    public event Action OnAttack;
    #endregion

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

        if (_TPZone == null)
        {
            Debug.LogWarning("TPZone is not assigned in the inspector.");
        }
        else
        {
            InitializeTPZone();
        }

        if (_spriteGhost == null)
        {
            Debug.LogWarning("SpriteGhost is not assigned in the inspector.");
        }
        else
        {
            _spriteGhost.SetActive(false);
        }

        if (_playerUIManager == null)
        {
            _playerUIManager = GetComponent<PlayerUIManager>();
        }
    }

    void Update()
    {
        _playerStateMachine.Update();
        float targetForce = (IsSprinting && HitNormal != Vector2.zero) ? _runSpeed : _walkSpeed;
        float currentAcc = _moveInput.x != 0 ? CurrentAcceleration : CurrentDeceleration;
        _moveSpeed = Mathf.MoveTowards(_moveSpeed, targetForce, currentAcc * Time.deltaTime);

        if (_moveInput.x > 0)
        {
            FacingDirection = 1f;
        }
        else if (_moveInput.x < 0)
        {
            FacingDirection = -1f;
        }

        if (_moveInput.x != 0) 
        {
            TargetAnimSpeed = (IsSprinting && HitNormal != Vector2.zero) ? _runSpeed : _walkSpeed;
        }
        else
        {
            TargetAnimSpeed = 0f;
        }
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
        if (_moveInput.x != 0)
        {
            _body.AddForce(_moveInput.x * _moveSpeed * Vector2.right, ForceType.Force);
        }

        _playerStateMachine.FixedUpdate();
    }

    public int GetPlayerDirection()
    {
        return _moveInput.x > 0 ? 1 : _moveInput.x < 0 ? -1 : 0;
    }

    private void InitializeTPZone()
    {
        if (_TPZone != null)
        {
            _TPZone.SetActive(false);
        }
        _TPZone.transform.localScale = new Vector3((_distanceTP - 1) * 0.5f, (_distanceTP - 1) * 0.5f, _TPZone.transform.localScale.z);
    }
    public void InitializePlayerInputInPlayerUIManager(PlayerInput playerInput)
    {
        if (_playerUIManager != null)
        {
            _playerUIManager.SetPlayerInput(playerInput);
        }
    }

    #region Input Methods
    public void Move(Vector2 moveInput) => _moveInput = moveInput;
    public void Jump() => LastJumpInputTime = Time.time;
    public void Sprint(bool isSprinting) => IsSprinting = isSprinting;
    public void TPEnter()
    {
        if(!_manaSystem.HasEnoughMana(_costTP))
        {
            return;
        }
        IsInTP = true;
        _TPZone.SetActive(true);
        _playerStateMachine.CurrentState.SetPushState<TPState>();
    }
    public void TPExit()
    {
        IsInTP = false;
        _TPZone.SetActive(false);
        IsCancelTP = false;
    }
    public void CancelTP()
    {
        _TPZone.SetActive(false);
        IsCancelTP = true;
    }

    public void OpenCloseMenu()
    {
        if(_playerUIManager != null)
        {
            _playerUIManager.ToggleMenu();
        }
    }
    #endregion

    #region Event Methods
    public void TriggerSprint() => OnSprint?.Invoke();
    public void TriggerTP() => OnTP?.Invoke();
    public void TriggerStartTP() => OnStartTP?.Invoke();
    public void TriggerHurt() => OnHurt?.Invoke();
    public void TriggerJump() => OnJump?.Invoke();
    public void TriggerGround() => OnGround?.Invoke();
    public void TriggerAttack() => OnAttack?.Invoke();
    #endregion
}
