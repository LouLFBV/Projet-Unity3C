class AirboneState : PlayerState
{
    public AirboneState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter() 
    {
        character.AnimatorPlayerScript.AnimatorPlayer.SetBool("IsGrounded", false);
    }
    public override void Update()
    {
        character.AnimatorPlayerScript.AnimatorPlayer.SetFloat("JumpVelocity", character.velocity.y);
        character.acceleration = character.moveInput.x != 0 ? character.airAcceleration : character.airDeceleration;
        if (character.IsGrounded)
        {
            stateMachine.ChangeState(PlayerStateType.Idle);
        }
        if (character.CollisionInfo._left || character.CollisionInfo._right)
        {
            stateMachine.ChangeState(PlayerStateType.WallJump);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
