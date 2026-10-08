using UnityEngine;

/// <summary>
/// Base state for player states where the character is grounded.
/// Handles grounded movement parameters, jump processing, and transitions
/// to the falling state when the character loses contact with the ground.
/// </summary>
class GroundedState : PlayerState
{
    /// <summary>
    /// Base state for player states where the character is grounded.
    /// Handles grounded movement parameters, jump processing, and transitions
    /// to the falling state when the character loses contact with the ground.
    /// </summary>
    public GroundedState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Initializes the character's acceleration and deceleration values
    /// using the grounded movement parameters.
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        Character.CurrentDeceleration = Character.GroundDeceleration;
        Character.CurrentAcceleration = Character.GroundAcceleration;

    }
    /// <summary>
    /// Updates the grounded state, checks whether the character has started falling,
    /// processes buffered jump input, and updates the coyote jump state.
    /// </summary>
    public override void Update() 
    {
        if (Character.HitNormal == Vector2.zero && Character.Body.Velocity.y != 0f)
        {
            SetNextState<FallState>();
        }

        ProcessJump();

        Character.LastGroundedTime = Time.time;
        Character.CanCoyoteJump = true;
    }
    /// <summary>
    /// Processes buffered jump input and transitions to the jump state when
    /// the character is grounded or still within the coyote time window.
    /// </summary>
    private void ProcessJump()
    {
        bool isJumpBuffered = Time.time - Character.LastJumpInputTime <= Character.JumpInputBuffer;
        if (!isJumpBuffered)
        {
            return;
        }
        if (Character.HitNormal != Vector2.zero || (Character.CanCoyoteJump && Time.time - Character.LastGroundedTime <= Character.CoyoteTime))
        {
            SetNextState<JumpState>();
        }
    }
}
