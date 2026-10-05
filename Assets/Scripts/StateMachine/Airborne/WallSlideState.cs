using UnityEngine;

class WallSlideState : AirboneState
{
    public WallSlideState(PlayerCharacter character) : base(character) { }

    public override void Enter()
    {
        base.Enter();
        Character.CanWallJump = true;
    }
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

    public override void FixedUpdate() { }

    public override void Exit()
    {
        Character.CanWallJump = false;
    }

    private void ProcessWallJump()
    {
        bool isWallJumpBuffered = Time.time - Character.LastJumpInputTime <= Character.JumpInputBuffer;
        if (!isWallJumpBuffered || Character.HitNormal == Vector2.zero) return;
        Character.FacingDirection *= -1;
        SetNextState<JumpState>();
    }
}