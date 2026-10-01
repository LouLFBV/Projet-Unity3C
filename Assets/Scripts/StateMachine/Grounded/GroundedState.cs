using UnityEngine;
using UnityEngine.TextCore.Text;

class GroundedState : PlayerState
{    public GroundedState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
        Character.AnimatorPlayerScript.SetIsGrounded(true);
    }
    public override void Update() 
    {
        ProcessJump();
        //character.acceleration = character.moveInput.x != 0 ? character.groundAcceleration : character.groundDeceleration;

        if (!Character.GroundInfos.IsGrounded)
        {
            if (Character.Body.Velocity.y < 0.1f)
            {
                SetNextState<FallState>();
                //_stateMachine.ChangeState(PlayerStateType.Fall);
            }
        }
        Character.lastGroundedTime = Time.time;
        Character.canCoyoteJump = true;
    }
    public override void FixedUpdate() { }
    public override void Exit() { }

    private void ProcessJump()
    {
        bool isJumpBuffered = Time.time - Character.lastJumpInputTime <= Character.jumpInputBuffer;
        if (!isJumpBuffered)
        {
            return;
        }
        if (Character.GroundInfos.IsGrounded || (Character.canCoyoteJump && Time.time - Character.lastGroundedTime <= Character.coyoteTime))
        {
            //Debug.Log("Jumping");
            SetNextState<JumpState>();
            //_stateMachine.ChangeState(PlayerStateType.Jump);
        }
    }
}
