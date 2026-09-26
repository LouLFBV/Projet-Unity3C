using UnityEngine;

class JumpState : AirboneState
{
    public JumpState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter() 
    {
        base.Enter();
        character.velocity.y = character.jumpForce;
        character.AnimatorPlayerScript.AnimatorPlayer.SetTrigger("Jump");
        character.lastJumpInputTime = float.MinValue; // Reset du jump input
        character.canCoyoteJump = false;
    }
    public override void Update() 
    {
        base.Update();
        if (character.velocity.y < 0.1f)
        {
            stateMachine.ChangeState(PlayerStateType.Fall);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() 
    {
    }
}