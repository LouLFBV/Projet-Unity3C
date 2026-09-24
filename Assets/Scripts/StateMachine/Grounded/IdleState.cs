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
        if (character.velocity.x > 0.1f)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Walk);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}