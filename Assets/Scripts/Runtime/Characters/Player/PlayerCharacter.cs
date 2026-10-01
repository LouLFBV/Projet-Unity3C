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
    [SerializeField] private float _moveSpeed = 10f;
    public float maxMoveSpeed = 20f;
    public float groundAcceleration = 100f;
    public float groundDeceleration = 100f; 
    public float sprintAcceleration = 10f;
    public float sprintDeceleration = 10f;
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
    public Vector2 jumpDir = Vector2.zero;

    [Header("Wall Jump")]   
    public bool canWallJump = false;
    public float wallJumpForce = 20f;
    public float wallJumpHorizontalForce = 10f;
    public float wallJumpGravity = 5f;
    public float lastWallJumpTime = float.MinValue;


    [Header("TP")]
    [SerializeField] private ManaSystem _manaSystem;
    public ManaSystem ManaSystem => _manaSystem;
    public float costTP = 30f;
    public float distanceToTP = 2f;
    public float FacingDirection { get; private set; } = 1f;



    //[HideInInspector] public Vector2 velocity;
    private CollisionInfo _collisionInfo;
    public CollisionInfo CollisionInfo => _collisionInfo;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public float lastJumpInputTime = float.MinValue;
    [HideInInspector] public float lastGroundedTime = float.MinValue;
    [HideInInspector] public bool canCoyoteJump = false;

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
        //if ((_collisionInfo._below && velocity.y < 0) || (_collisionInfo._above && velocity.y > 0))
        //{
        //    velocity.y = 0;
        //}


        //float gravity = velocity.y >= 0 ? _risingingGravity : canWallJump ? wallJumpGravity : _fallingGravity; // on choisit la gravité en fonction de la direction du mouvement
        //velocity.y -= gravity * Time.deltaTime; // acc * delta = vitesse, Time.deltaTime pour l'accumulation

        //float targetVelocityX = isSprinting && _collisionInfo._below ? moveInput.x * maxMoveSpeed : moveInput.x * _moveSpeed;
        
        //velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, acceleration * Time.deltaTime);

        //Vector2 deltaPosition = velocity * Time.deltaTime; // vitesse * delta = position

        //_collisionInfo.Reset();
        //ProcessMove(ref deltaPosition); // on modifie la position en fonction des collisions

        //transform.Translate(deltaPosition);

        _animatorPlayer.SetMoveAnimation(_body.Velocity.x, maxMoveSpeed); // on met à jour l'animation en fonction de la vitesse

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

        if (moveInput.x != 0)
        {
            // On applique directement la vitesse souhaitée
            Vector2 targetVelocity = new Vector2(moveInput.x * _moveSpeed, _body.Velocity.y);
            _body.SetVelocity(targetVelocity);
        }
        else if (_infos.IsGrounded)
        {
            // On stoppe le mouvement horizontal au sol si pas d'input
            _body.SetVelocity(new Vector2(0, _body.Velocity.y));
        }
    }

    //private void ProcessMove(ref Vector2 deltaPosition)
    //{

    //    if (deltaPosition.x != 0)
    //    {
    //        ProcessHorizontalCollisions(ref deltaPosition);
    //    }

    //    if (deltaPosition.y != 0)
    //    {
    //        ProcessVerticalCollisions(ref deltaPosition);
    //    }
    //}

    //private void ProcessVerticalCollisions(ref Vector2 deltaPosition)
    //{
    //    float directionY = Mathf.Sign(deltaPosition.y);
    //    RaycastHit2D hit = Physics2D.BoxCast(
    //        transform.position + new Vector3(deltaPosition.x, 0),
    //        _collider.size,
    //        0,
    //        Vector2.up * directionY,
    //        Mathf.Abs(deltaPosition.y) + _skinWidth,
    //        _groundLayer
    //        );

    //    if (hit)
    //    {
    //        deltaPosition.y = Mathf.Max(0, hit.distance - _skinWidth) * directionY;
    //        _collisionInfo._below = directionY < 0;
    //        _collisionInfo._above = directionY > 0;

    //    }
    //}

    //private void ProcessHorizontalCollisions(ref Vector2 deltaPosition)
    //{
    //    float directionX = Mathf.Sign(deltaPosition.x);
    //    RaycastHit2D hit = Physics2D.BoxCast(
    //        transform.position,
    //        _collider.size,
    //        0,
    //        Vector2.right * directionX,
    //        Mathf.Abs(deltaPosition.x) + _skinWidth,
    //        _groundLayer
    //        );

    //    if (hit)
    //    {
    //        deltaPosition.x = Mathf.Max(0, hit.distance - _skinWidth) * directionX;
    //        _collisionInfo._left = directionX < 0;
    //        _collisionInfo._right = directionX > 0;
    //    }
    //}

    public void Move(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    public void Jump()
    {
        lastJumpInputTime = Time.time;
    }

    public void Jump(float jump)
    {

    }

    public void Sprint(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    public void TP()
    {
        _playerStateMachine.CurrentState.SetPushState<TPState>();
        //_playerStateMachine.PushState(PlayerStateType.TP);
    }
}
