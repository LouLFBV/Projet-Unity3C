using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents the player character and manages its physics, movement, states, abilities and input.
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    #region --- PLAYER STATE ---

    /// <summary>
    /// Gets the state machine controlling the player's current state.
    /// </summary>
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    /// <summary>
    /// State machine used to manage the player's different states.
    /// </summary>
    private PlayerStateMachine _playerStateMachine;
    /// <summary>
    /// Reference to the player's UI manager.
    /// </summary>
    [SerializeField] private PlayerUIManager _playerUIManager;

    #endregion

    #region --- PHYSICS ---

    /// <summary>
    /// Reference to the physics body used by the player.
    /// </summary>
    [Header("Physics")]
    [SerializeField] private PhysicBody _body;
    /// <summary>
    /// Strategy used to process the player's physics raycasts.
    /// </summary>
    [SerializeField] private ColliderStrategy _strategy = null;
    /// <summary>
    /// Physics data shared with the collision processing system.
    /// </summary>
    [SerializeField] private FramePhysicsData _data = new();
    /// <summary>
    /// Gets the player's physics body.
    /// </summary>
    public PhysicBody Body => _body;
    /// <summary>
    /// Contact filter used for physics raycasts.
    /// </summary>
    private ContactFilter2D _filter = new ContactFilter2D();
    /// <summary>
    /// Raycast results buffer used for collision detection.
    /// </summary>
    private RaycastHit2D[] _hits = new RaycastHit2D[10];
    #endregion

    #region --- COLLISION ---

    /// <summary>
    /// Collider used to detect collisions around the player.
    /// </summary>
    [Header("Collision")]
    [SerializeField] private BoxCollider2D _collider;
    /// <summary>
    /// Layer mask defining which layers are considered as ground.
    /// </summary>
    [SerializeField] private LayerMask _groundLayer;
    /// <summary>
    /// Skin width used when processing player collisions.
    /// </summary>
    [SerializeField] private float _skinWidth = 0.01f;
    /// <summary>
    /// Gets the player's box collider.
    /// </summary>
    public BoxCollider2D Collider => _collider;
    /// <summary>
    /// Gets the layer mask used to identify ground collisions.
    /// </summary>
    public LayerMask GroundLayer => _groundLayer;
    /// <summary>
    /// Gets the collision skin width.
    /// </summary>
    public float SkinWidth => _skinWidth;
    /// <summary>
    /// Gets or sets the current collision normal detected by the player.
    /// </summary>
    public Vector2 HitNormal { get; set; } = Vector2.zero;
    #endregion

    #region --- MOVEMENT ---

    /// <summary>
    /// Walking speed of the player.
    /// </summary>
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 10f;
    /// <summary>
    /// Running speed of the player.
    /// </summary>
    [SerializeField] private float _runSpeed = 20f;
    /// <summary>
    /// Deceleration applied while the player is on the ground.
    /// </summary>
    [SerializeField] private float _groundDeceleration = 10f;
    /// <summary>
    /// Acceleration applied while the player is on the ground.
    /// </summary>
    [SerializeField] private float _groundAcceleration = 20f;
    /// <summary>
    /// Acceleration applied while the player is in the air.
    /// </summary>
    [SerializeField] private float _airAcceleration = 10f;
    /// <summary>
    /// Deceleration applied while the player is in the air.
    /// </summary>
    [SerializeField] private float _airDeceleration = 20f;
    /// <summary>
    /// Gets the ground deceleration value.
    /// </summary>
    public float GroundDeceleration => _groundDeceleration;
    /// <summary>
    /// Gets the ground acceleration value.
    /// </summary>
    public float GroundAcceleration => _groundAcceleration;
    /// <summary>
    /// Gets the air acceleration value.
    /// </summary>
    public float AirAcceleration => _airAcceleration;
    /// <summary>
    /// Gets the air deceleration value.
    /// </summary>
    public float AirDeceleration => _airDeceleration;
    /// <summary>
    /// Gets or sets the current movement acceleration.
    /// </summary>
    public float CurrentAcceleration { get; set; }
    /// <summary>
    /// Gets or sets the current movement deceleration.
    /// </summary>
    public float CurrentDeceleration { get; set; }
    /// <summary>
    /// Gets the maximum movement speed.
    /// </summary>
    public float MaxMoveSpeed => _maxMoveSpeed;
    /// <summary>
    /// Gets or sets whether the player is allowed to move.
    /// </summary>
    public bool CanMove { get; set; } = false;
    /// <summary>
    /// Gets or sets the direction the player is facing.
    /// </summary>
    public float FacingDirection { get; set; } = 1f;
    /// <summary>
    /// Gets or sets whether the player is currently sprinting.
    /// </summary>
    public bool IsSprinting { get; set; } = false;
    /// <summary>
    /// Stores the current movement input.
    /// </summary>
    private Vector2 _moveInput;

    /// <summary>
    /// Stores the current movement speed.
    /// </summary>
    private float _moveSpeed;
    /// <summary>
    /// Stores the current maximum movement speed.
    /// </summary>
    private float _maxMoveSpeed;

    #endregion

    #region --- GRAVITY ---

    /// <summary>
    /// Gravity applied while the player is falling.
    /// </summary>
    [Header("Gravity")]

    [SerializeField] private float _fallingGravity = 15f;
    /// <summary>
    /// Gravity applied while the player is rising.
    /// </summary>
    [SerializeField] private float _risingingGravity = 25f;
    /// <summary>
    /// Gets the falling gravity value.
    /// </summary>
    public float FallingGravity => _fallingGravity;
    /// <summary>
    /// Gets the rising gravity value.
    /// </summary>
    public float RisingingGravity => _risingingGravity;
    #endregion

    #region --- JUMP ---

    /// <summary>
    /// Force applied when the player jumps.
    /// </summary>
    [Header("Jump")]
    [SerializeField] private float _jumpForce  = 20f;
    /// <summary>
    /// Duration for which a jump input is buffered.
    /// </summary>
    [SerializeField] private float _jumpInputBuffer = 0.1f;
    /// <summary>
    /// Duration during which the player can still perform a jump after leaving the ground.
    /// </summary>
    [SerializeField] private float _coyoteTime  = 0.065f;
    /// <summary>
    /// Gets the jump force.
    /// </summary>
    public float JumpForce => _jumpForce;
    /// <summary>
    /// Gets the jump input buffer duration.
    /// </summary>
    public float JumpInputBuffer => _jumpInputBuffer;
    /// <summary>
    /// Gets the coyote time duration.
    /// </summary>
    public float CoyoteTime => _coyoteTime;
    /// <summary>
    /// Gets or sets the direction of the jump.
    /// </summary>
    public Vector2 JumpDir { get; set; } = Vector2.zero;
    /// <summary>
    /// Gets or sets the time when the last jump input was received.
    /// </summary>
    public float LastJumpInputTime { get; set; } = float.MinValue;
    /// <summary>
    /// Gets or sets the time when the player was last grounded.
    /// </summary>
    public float LastGroundedTime { get; set; } = float.MinValue;
    /// <summary>
    /// Gets or sets whether the player can perform a coyote jump.
    /// </summary>
    public bool CanCoyoteJump { get; set; } = false;
    #endregion

    #region --- WALL JUMP ---

    /// <summary>
    /// Maximum distance used to detect a wall for wall jumping.
    /// </summary>
    [Header("Wall Jump")]
    [SerializeField] private float _castDistanceToWallJump = 0.3f;
    /// <summary>
    /// Gravity multiplier applied during a wall jump.
    /// </summary>
    [SerializeField] private float _wallJumpGravity = 0.3f;
    /// <summary>
    /// Gets the gravity multiplier used during a wall jump.
    /// </summary>
    public float WallJumpGravity => _wallJumpGravity;
    /// <summary>
    /// Gets or sets whether the player can perform a wall jump.
    /// </summary>
    public bool CanWallJump { get; set; } = false;
    /// <summary>
    /// Gets the wall detection distance used for wall jumping.
    /// </summary>
    public float CastDistanceToWallJump => _castDistanceToWallJump;
    #endregion

    #region --- TELEPORT ---

    /// <summary>
    /// Reference to the mana system used to pay for teleportation.
    /// </summary>
    [Header("TP")]
    [SerializeField] private ManaSystem _manaSystem;
    /// <summary>
    /// Mana cost required to teleport.
    /// </summary>
    [SerializeField] private float _costTP = 30f;
    /// <summary>
    /// Maximum teleport distance.
    /// </summary>
    [SerializeField] private float _distanceTP = 2f;
    /// <summary>
    /// Time scale applied while teleporting.
    /// </summary>
    [SerializeField] private float _timeScaleInTP = 0.1f;
    /// <summary>
    /// Game object representing the teleportation zone.
    /// </summary>
    [SerializeField] private GameObject _TPZone;
    /// <summary>
    /// Game object representing the teleportation zone timer.
    /// </summary>
    [SerializeField] private GameObject _TPZoneTimer;
    /// <summary>
    /// Ghost sprite displayed during teleportation.
    /// </summary>
    [SerializeField] private GameObject _spriteGhost;
    /// <summary>
    /// Gets the mana cost required for teleportation.
    /// </summary>
    public float CostTP => _costTP;
    /// <summary>
    /// Gets the maximum teleport distance.
    /// </summary>
    public float DistanceTP => _distanceTP;
    /// <summary>
    /// Gets the mana system used by the player.
    /// </summary>
    public ManaSystem ManaSystem => _manaSystem;
    /// <summary>
    /// Gets or sets whether the player is currently teleporting.
    /// </summary>
    public bool IsInTP { get; set; } = false;
    /// <summary>
    /// Gets or sets whether the current teleport is being cancelled.
    /// </summary>
    public bool IsCancelTP { get; set; } = false;
    /// <summary>
    /// Gets the time scale applied during teleportation.
    /// </summary>
    public float TimeScaleInTP => _timeScaleInTP;
    /// <summary>
    /// Gets the teleportation zone object.
    /// </summary>
    public GameObject TPZone => _TPZone;
    /// <summary>
    /// Gets the teleportation zone timer object.
    /// </summary>
    public GameObject TPZoneTimer => _TPZoneTimer;
    /// <summary>
    /// Gets the ghost sprite object used during teleportation.
    /// </summary>
    public GameObject SpriteGhost => _spriteGhost;
    #endregion

    #region --- ANIMATOR ---

    /// <summary>
    /// Reference to the player's animator controller component.
    /// </summary>
    [Header("Animator")]

    [SerializeField] private PlayerAnimator _animatorPlayer;
    /// <summary>
    /// Gets the player's animator script.
    /// </summary>
    public PlayerAnimator AnimatorPlayerScript => _animatorPlayer;
    /// <summary>
    /// Gets the target movement speed used by the player's animations.
    /// </summary>
    public float TargetAnimSpeed { get; private set; } = 0f;
    #endregion

    #region --- EVENTS ---

    /// <summary>
    /// Invoked when the player starts sprinting.
    /// </summary>
    public event Action OnSprint;
    /// <summary>
    /// Invoked when the player reaches the ground.
    /// </summary>
    public event Action OnGround;
    /// <summary>
    /// Invoked when the player jumps.
    /// </summary>
    public event Action OnJump;
    /// <summary>
    /// Invoked when the player teleports.
    /// </summary>
    public event Action OnTP;
    /// <summary>
    /// Invoked when the player starts teleporting.
    /// </summary>
    public event Action OnStartTP;
    /// <summary>
    /// Invoked when the player cancels a teleport.
    /// </summary>
    public event Action OnCancelTP;
    /// <summary>
    /// Invoked when the player is hurt.
    /// </summary>
    public event Action OnHurt;
    /// <summary>
    /// Invoked when the player attacks.
    /// </summary>
    public event Action OnAttack;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the player's state machine, physics configuration and teleportation objects.
    /// </summary>

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
    /// <summary>
    /// Updates the player's state machine, movement speed, facing direction and animation speed.
    /// </summary>
    void Update()
    {
        if (!CanMove)
        {
            return;
        }
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
    /// <summary>
    /// Processes physics movement and collision detection during the fixed update loop.
    /// </summary>
    private void FixedUpdate()
    {
        if (!CanMove)
        {
            return;
        }
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
    #endregion

    #region --- PLAYER MANAGEMENT ---

    /// <summary>
    /// Gets the player's current movement direction.
    /// </summary>
    /// <returns><c>1</c> when moving right, <c>-1</c> when moving left, or <c>0</c> when not moving.</returns>
    public int GetPlayerDirection()
    {
        return _moveInput.x > 0 ? 1 : _moveInput.x < 0 ? -1 : 0;
    }
    /// <summary>
    /// Initializes the teleportation zone and configures its visual scale.
    /// </summary>
    private void InitializeTPZone()
    {
        if (_TPZone != null)
        {
            _TPZone.SetActive(false);
        }
        if (_TPZoneTimer != null)
        {
            _TPZoneTimer.SetActive(false);
        }
        _TPZone.transform.localScale = new Vector3((_distanceTP - 1) * 0.5f, (_distanceTP - 1) * 0.5f, _TPZone.transform.localScale.z);
    }
    /// <summary>
    /// Initializes the player input reference used by the player UI manager.
    /// </summary>
    /// <param name="playerInput">Input component associated with the player.</param>
    public void InitializePlayerInputInPlayerUIManager(PlayerInput playerInput)
    {
        if (_playerUIManager != null)
        {
            _playerUIManager.SetPlayerInput(playerInput);
        }
    }
    #endregion

    #region --- INPUT METHODS ---

    /// <summary>
    /// Updates the player's movement input.
    /// </summary>
    /// <param name="moveInput">Movement input vector.</param>
    public void Move(Vector2 moveInput) => _moveInput = moveInput;
    /// <summary>
    /// Records the time when the player requests a jump.
    /// </summary>
    public void Jump() => LastJumpInputTime = Time.time;
    /// <summary>
    /// Updates the player's sprinting state.
    /// </summary>
    /// <param name="isSprinting">Whether the player is sprinting.</param>
    public void Sprint(bool isSprinting) => IsSprinting = isSprinting;
    /// <summary>
    /// Starts the teleportation process if enough mana is available.
    /// </summary>
    public void TPEnter()
    {
        if(!_manaSystem.HasEnoughMana(_costTP))
        {
            return;
        }
        IsInTP = true;
        _TPZone.SetActive(true);
        _TPZoneTimer.SetActive(true);
        _playerStateMachine.CurrentState.SetPushState<TPState>();
    }
    /// <summary>
    /// Exits the current teleportation state and resets teleportation flags.
    /// </summary>
    public void TPExit()
    {
        IsInTP = false;
        _TPZone.SetActive(false);
        _TPZoneTimer.SetActive(false);
        IsCancelTP = false;
    }
    /// <summary>
    /// Cancels the current teleportation process.
    /// </summary>
    public void CancelTP()
    {
        _TPZone.SetActive(false);
        _TPZoneTimer.SetActive(false);
        IsCancelTP = true;
    }
    /// <summary>
    /// Toggles the player's menu through the player UI manager.
    /// </summary>
    public void OpenCloseMenu()
    {
        if(_playerUIManager != null)
        {
            _playerUIManager.ToggleMenu();
        }
    }
    #endregion

    #region --- EVENT METHODS ---

    /// <summary>
    /// Invokes the sprint event.
    /// </summary>
    public void TriggerSprint() => OnSprint?.Invoke();
    /// <summary>
    /// Invokes the teleport event.
    /// </summary>
    public void TriggerTP() => OnTP?.Invoke();
    /// <summary>
    /// Invokes the teleport cancellation event.
    /// </summary>
    public void TriggerCancelTP() => OnCancelTP?.Invoke();
    /// <summary>
    /// Invokes the teleport start event.
    /// </summary>
    public void TriggerStartTP() => OnStartTP?.Invoke();
    /// <summary>
    /// Invokes the hurt event.
    /// </summary>
    public void TriggerHurt() => OnHurt?.Invoke();
    /// <summary>
    /// Invokes the jump event.
    /// </summary>
    public void TriggerJump() => OnJump?.Invoke();

    /// <summary>
    /// Invokes the ground event.
    /// </summary>
    public void TriggerGround() => OnGround?.Invoke();
    /// <summary>
    /// Invokes the attack event.
    /// </summary>
    public void TriggerAttack() => OnAttack?.Invoke();
    #endregion
}
