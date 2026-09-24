using UnityEngine;

class GroundedState : PlayerState
{    public GroundedState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        character.AnimatorPlayer.AnimatorPlayer.SetBool("IsGrounded", true);
    }
    public override void Update() 
    {
        ProcessJump();
        character.acceleration = character.moveInput.x != 0 ? character.groundAcceleration : character.groundDeceleration;

        if (!character.IsGrounded)
        {
            if (character.velocity.y < 0.1f)
            {
                Debug.Log("Falling");
                character.PlayerStateMachine.ChangeState(PlayerStateType.Fall);
            }
        }
        character.lastGroundedTime = Time.time;
        character.canCoyoteJump = true;
    }
    public override void FixedUpdate() { }
    public override void Exit() { }

    private void ProcessJump()
    {
        bool isJumpBuffered = Time.time - character.lastJumpInputTime <= character.jumpInputBuffer;
        if (!isJumpBuffered)
        {
            return;
        }
        if (character.CollisionInfo._below || (character.canCoyoteJump && Time.time - character.lastGroundedTime <= character.coyoteTime))
        {
            Debug.Log("Jumping");
            character.PlayerStateMachine.ChangeState(PlayerStateType.Jump);
        }
    }
}
