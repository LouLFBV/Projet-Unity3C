using Unity.VisualScripting;
using UnityEngine;
class AirboneState : PlayerState
{
    protected int _wallJumpDirection = 0;
    private bool _canWallSlide = false;
    public AirboneState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
        Character.CurrentDeceleration = Character.AirDeceleration;
        Character.CurrentAcceleration = Character.AirAcceleration;
    }
    public override void Update()
    {
        ProcessWallSlide();
        if (_canWallSlide)
        {
            SetNextState<WallSlideState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }

    private void ProcessWallSlide()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            Character.transform.position,
            Character.Collider.size,
            0,
            Vector2.right * Character.GetPlayerDirection(),
            Character.CastDistanceToWallJump + Character.SkinWidth,
            Character.GroundLayer
            ); ;

        if (hit)
        {
            _canWallSlide = true;
            _wallJumpDirection = -Character.GetPlayerDirection();
        }
        else
        {
            _canWallSlide = false;
            _wallJumpDirection = 0;

        }
    }
}
