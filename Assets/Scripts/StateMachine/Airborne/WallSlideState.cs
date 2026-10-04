using UnityEngine;

class WallSlideState : AirboneState
{
    public WallSlideState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();

        Character.canWallJump = true;
    }
    public override void Update() 
    {
        base.Update();

        ProcessWallJump();

        if (Character.HitNormal == Vector2.zero)
        {
            SetNextState<FallState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() 
    {
        Character.canWallJump = false;
    }

    private void ProcessWallJump()
    {
        bool isWallJumpBuffered = Time.time - Character.lastJumpInputTime <= Character.jumpInputBuffer;
        if (!isWallJumpBuffered) return;

        SetNextState<JumpState>(); 
    }
}