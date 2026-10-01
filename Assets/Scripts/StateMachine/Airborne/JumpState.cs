
using UnityEngine;

class JumpState : AirboneState
{
    public JumpState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();

        // On applique directement une vitesse verticale propre (ex: jumpForce = 15)
        //_character.Body.AddForce(Vector2.up * _character.jumpForce, ForceType.Velocity);
        Character.jumpDir = (Character.GroundInfos.Up + Vector2.up).normalized;
        Character.Body.AddForce(Character.jumpDir * Character.jumpForce,
        ForceType.Impulse);

        Character.AnimatorPlayerScript.AnimatorPlayer.SetTrigger("Jump");
        Character.lastJumpInputTime = float.MinValue;
        Character.canCoyoteJump = false;
    }

    public override void Update()
    {
        base.Update();

        // Utiliser la vitesse du PhysicBody
        if (Character.Body.Velocity.y < 0f)
        {
            SetNextState<FallState>();
            //_stateMachine.ChangeState(PlayerStateType.Fall);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() 
    {
    }
}