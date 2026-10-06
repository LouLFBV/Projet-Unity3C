using UnityEngine;

class GroundedState : PlayerState
{    public GroundedState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
        //Character.AnimatorPlayerScript.SetIsGrounded(true);
        Character.TriggerGround();

        Character.CurrentDeceleration = Character.GroundDeceleration;
        Character.CurrentAcceleration = Character.GroundAcceleration;

    }
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
    public override void FixedUpdate() { }
    public override void Exit() { }

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
