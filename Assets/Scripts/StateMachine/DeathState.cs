class DeathState : PlayerState
{
    public DeathState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter() { }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}