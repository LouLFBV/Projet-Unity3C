class RunState : GroundedState
{
    public RunState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
        Character.TriggerSprint();
    }
    public override void Update()
    {
        base.Update();
        if (!Character.IsSprinting)
        {
            SetNextState<WalkState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}