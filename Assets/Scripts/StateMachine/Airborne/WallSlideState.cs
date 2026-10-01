using UnityEngine;

class WallSlideState : AirboneState
{
    private int oppositeDirectionToWall = 1;
    public WallSlideState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
        Debug.Log("<color=yellow>WallJumpState Enter</color>");
        Character.lastWallJumpTime = Time.time;
        Character.canWallJump = true;
    }
    public override void Update() 
    {
        base.Update();
        //if (Time.time - character.lastWallJumpTime < character.timeToJumpOnWall)
        //{
        //    Debug.Log("<color=green>WallJumpState Update</color>");
        //    //stateMachine.ChangeState(PlayerStateType.Fall);
        //}

        //character.velocity.y -= character.wallJumpGravity * Time.deltaTime;
        //character.transform.Translate(velocity * Time.deltaTime);

        ProcessWallJump();

        if (!Character.CollisionInfo._left && !Character.CollisionInfo._right)
        {
            SetNextState<FallState>();
            //_stateMachine.ChangeState(PlayerStateType.Fall);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() 
    {
        Debug.Log("<color=yellow>WallJumpState Exit</color>");
        Character.canWallJump = false;
    }

    private void ProcessWallJump()
    {
        bool isWallJumpBuffered = Time.time - Character.lastJumpInputTime <= Character.jumpInputBuffer;
        if (!isWallJumpBuffered)
        {
            return;
        }

        //oppositeDirectionToWall = _character.CollisionInfo._left ? 1 : -1;

        //_character.velocity.y = _character.wallJumpForce;
        //_character.velocity.x = _character.wallJumpForce * oppositeDirectionToWall;

        Character.jumpDir = (Character.GroundInfos.Up + Vector2.up).normalized;
        Character.Body.AddForce(Character.jumpDir * Character.jumpForce,
        ForceType.Impulse);

        SetNextState<FallState>(); 
        //_stateMachine.ChangeState(PlayerStateType.Fall);
    }
}