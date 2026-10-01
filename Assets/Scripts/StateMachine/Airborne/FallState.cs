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
        if (Character.GroundInfos.IsGrounded)
        {
            SetNextState<IdleState>();
            //StateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}