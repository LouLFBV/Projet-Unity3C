using UnityEngine;
class AirboneState : PlayerState
{
    public AirboneState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
        Character.AnimatorPlayerScript.SetIsGrounded(false);
    }
    public override void Update()
    {
        Character.AnimatorPlayerScript.AnimatorPlayer.SetFloat("JumpVelocity", Character.Body.Velocity.y);
        Character.acceleration = Character.moveInput.x != 0 ? Character.airAcceleration : Character.airDeceleration;
        if (Character.GroundInfos.IsGrounded && Character.PlayerStateMachine.CurrentState is not JumpState)
        {
            SetNextState<IdleState>();
        }
        //if (Character.GroundInfos.Right == Vector2.right || Character.GroundInfos.Right ==  Vector2.left)
        if (Character.CollisionInfo._left || Character.CollisionInfo._right)
        {
            SetNextState<WallSlideState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
