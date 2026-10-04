
using UnityEngine;

class JumpState : AirboneState
{
    public JumpState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();

        Character.jumpDir = (Character.HitNormal + Vector2.up * 2).normalized;
        Character.Body.AddForce(Character.jumpDir * Character.jumpForce,
        ForceType.Impulse);

        Character.TriggerJump();

        Character.lastJumpInputTime = float.MinValue;
        Character.canCoyoteJump = false;
    }

    public override void Update()
    {
        base.Update();

        if (Character.Body.Velocity.y < 0f)
        {
            SetNextState<FallState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() {}
}