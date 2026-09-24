using UnityEngine;

class JumpState : AirboneState
{
    public JumpState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
        Debug.Log("JumpState Enter");
        character.velocity.y = character.jumpForce;
        character.AnimatorPlayer.AnimatorPlayer.SetTrigger("Jump");
        character.lastJumpInputTime = float.MinValue; // Reset du jump input
        character.canCoyoteJump = false;
    }
    public override void Update() 
    {
        Debug.Log($"<color=green>JumpState Update</color>");
        base.Update();
        if (character.velocity.y < 0.1f)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Fall);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}