using UnityEngine;

class FallState : AirboneState
{
    public FallState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
    }
    public override void Update() 
    {
        base.Update();
        if (Character.GroundInfos.IsGrounded && Character.Body.Velocity.y <= 0.1f && _wallJumpDirection == 0)
        {
            Debug.Log("<color=green>FallState</color> - Grounded");
            SetNextState<IdleState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}