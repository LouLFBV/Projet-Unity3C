using UnityEngine;
class IdleState : GroundedState
{
    public IdleState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
    }
    public override void Update() 
    {
        base.Update();
        if (Mathf.Abs(Character.Body.Velocity.x) > 0.1f)
        {
            SetNextState<WalkState>();
            //_stateMachine.ChangeState(PlayerStateType.Walk);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}