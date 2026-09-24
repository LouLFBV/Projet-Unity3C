class AirboneState : PlayerState
{
    public AirboneState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        character.AnimatorPlayer.AnimatorPlayer.SetBool("IsGrounded", false);
    }
    public override void Update()
    {
        character.AnimatorPlayer.AnimatorPlayer.SetFloat("JumpVelocity", character.velocity.y);
        character.acceleration = character.moveInput.x != 0 ? character.airAcceleration : character.airDeceleration;
        if (character.IsGrounded)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Idle);
        }
        if (character.CollisionInfo._left || character.CollisionInfo._right)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.WallJump);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
