using UnityEngine;

/// <summary>
/// Base state for airborne player states.
/// Handles airborne movement parameters, wall slide detection,
/// and transitions to the wall slide state.
/// </summary>
class AirboneState : PlayerState
{
    /// <summary>
    /// Direction in which the player will jump when performing a wall jump.
    /// </summary>
    protected int _wallJumpDirection = 0;
    /// <summary>
    /// Indicates whether the player is currently able to enter a wall slide.
    /// </summary>
    protected bool _canWallSlide = false;
    /// <summary>
    /// Initializes a new instance of the <see cref="AirboneState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public AirboneState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Initializes the airborne state by applying the airborne acceleration
    /// and deceleration values.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        Character.CurrentDeceleration = Character.AirDeceleration;
        Character.CurrentAcceleration = Character.AirAcceleration;
    }
    /// <summary>
    /// Updates the airborne state and transitions to the wall slide state
    /// when the player is in contact with a suitable wall.
    /// </summary>
    public override void Update()
    {
        ProcessWallSlide();
        if (_canWallSlide)
        {
            SetNextState<WallSlideState>();
        }
    }
    /// <summary>
    /// Handles cleanup when exiting the airborne state by triggering
    /// the grounded animation or visual state.
    /// </summary>
    public override void Exit() 
    {
        Character.TriggerGround();
    }
    /// <summary>
    /// Checks for a wall in the player's facing direction and determines
    /// whether the player can enter a wall slide.
    /// </summary>
    protected void ProcessWallSlide()
    {
        float castDistance = Character.CastDistanceToWallJump + Character.SkinWidth;

        RaycastHit2D hitRight = Physics2D.BoxCast(
            Character.transform.position,
            Character.Collider.size,
            0,
            Vector2.right,
            castDistance,
            Character.GroundLayer
        );

        RaycastHit2D hitLeft = Physics2D.BoxCast(
            Character.transform.position,
            Character.Collider.size,
            0,
            Vector2.left,
            castDistance,
            Character.GroundLayer
        );

        if (hitRight && Mathf.Abs(hitRight.normal.y) < 0.1f)
        {
            _canWallSlide = true;
            Character.FacingDirection = 1;
            _wallJumpDirection = -1;       
        }
        else if (hitLeft && Mathf.Abs(hitLeft.normal.y) < 0.1f)
        {
            _canWallSlide = true;
            Character.FacingDirection = -1; 
            _wallJumpDirection = 1;         
        }
        else
        {
            _canWallSlide = false;
            _wallJumpDirection = 0;
        }
    }
}
