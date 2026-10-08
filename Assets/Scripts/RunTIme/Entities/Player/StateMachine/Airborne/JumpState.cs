
using UnityEngine;

/// <summary>
/// Represents the state in which the player performs a jump.
/// Handles jump force, gravity, jump direction, and the transition to the falling state.
/// </summary>
class JumpState : AirboneState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JumpState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public JumpState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Initializes the jump by applying the rising gravity,
    /// resetting jump-related timers and states, calculating the jump direction,
    /// applying the jump force, and triggering the jump animation.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        Character.Body.SetGravity(true, Character.RisingingGravity);

        Character.LastJumpInputTime = float.MinValue;
        Character.CanCoyoteJump = false;

        Vector2 newVelocity = Character.Body.Velocity;
        newVelocity.y = 0f;
        Character.Body.SetVelocity(newVelocity);

        Character.JumpDir = (Character.HitNormal + Vector2.up * 2).normalized;
        Character.Body.AddForce(Character.JumpDir * Character.JumpForce,
        ForceType.Impulse);

        Character.TriggerJump();
    }
    /// <summary>
    /// Updates the jump state and transitions to the falling state
    /// once the player starts descending and can no longer perform a wall slide.
    /// </summary>

    public override void Update()
    {
        base.Update();

        if (Character.Body.Velocity.y < 0f && !_canWallSlide)
        {
            SetNextState<FallState>();
        }
    }
}