
using UnityEngine;

class JumpState : AirboneState
{
    public JumpState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();

        Character.JumpDir = (Character.HitNormal + Vector2.up * 2).normalized;
        Character.Body.AddForce(Character.JumpDir * Character.JumpForce,
        ForceType.Impulse);

        Character.TriggerJump();

        Character.LastJumpInputTime = float.MinValue;
        Character.CanCoyoteJump = false;
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