class RunState : GroundedState
{
    public RunState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (!character.isSprinting)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Walk);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}