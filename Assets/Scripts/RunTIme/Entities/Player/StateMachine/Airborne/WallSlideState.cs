using UnityEngine;

/// <summary>
/// Represents the state in which the player slides along a wall while airborne.
/// Handles wall sliding, wall jumping, gravity, and transitions back to grounded
/// or falling states.
/// </summary>
class WallSlideState : AirboneState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WallSlideState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public WallSlideState(PlayerCharacter character) : base(character) { }

    /// <summary>
    /// Initializes the wall slide state by enabling wall jumping
    /// and triggering the grounded animation.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        Character.CanWallJump = true;
        Character.TriggerGround();
    }
    /// <summary>
    /// Updates the wall slide state by processing wall sliding and wall jumping,
    /// applying the appropriate gravity, and handling state transitions.
    /// </summary>
    public override void Update()
    {
        ProcessWallSlide();
        ProcessWallJump();

        Character.Body.SetGravity(true, Character.Body.Velocity.y <= 0 ? Character.WallJumpGravity : Character.FallingGravity);

        if (Character.HitNormal != Vector2.zero && Character.HitNormal.y > 0.5f)
        {
            SetNextState<WalkState>();
        }
        else if (!_canWallSlide)
        {
            SetNextState<FallState>();
        }
    }

    /// <summary>
    /// Disables wall jumping when exiting the wall slide state.
    /// </summary>
    public override void Exit()
    {
        Character.CanWallJump = false;
    }

    /// <summary>
    /// Processes a buffered wall jump input and transitions to the jump state
    /// when the player is in contact with a wall.
    /// </summary>
    private void ProcessWallJump()
    {
        bool isWallJumpBuffered = Time.time - Character.LastJumpInputTime <= Character.JumpInputBuffer;
        if (!isWallJumpBuffered || Character.HitNormal == Vector2.zero) return;
        Character.FacingDirection *= -1;
        SetNextState<JumpState>();
    }
}