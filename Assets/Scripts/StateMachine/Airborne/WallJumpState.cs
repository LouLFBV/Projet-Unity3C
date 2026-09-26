using UnityEngine;

class WallJumpState : AirboneState
{
    private int oppositeDirectionToWall = 1;
    public WallJumpState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter() 
    {
        base.Enter();
        Debug.Log("<color=yellow>WallJumpState Enter</color>");
        character.lastWallJumpTime = Time.time;
        character.canWallJump = true;
        character.AnimatorPlayerScript.AnimatorPlayer.SetBool("IsGrounded", false);
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

        if (!character.CollisionInfo._left && !character.CollisionInfo._right)
        {
            stateMachine.ChangeState(PlayerStateType.Fall);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() 
    {
        Debug.Log("<color=yellow>WallJumpState Exit</color>");
        character.canWallJump = false;
    }

    private void ProcessWallJump()
    {
        bool isWallJumpBuffered = Time.time - character.lastJumpInputTime <= character.jumpInputBuffer;
        if (!isWallJumpBuffered)
        {
            return;
        }

        oppositeDirectionToWall = character.CollisionInfo._left ? 1 : -1;

        character.velocity.y = character.wallJumpForce;
        character.velocity.x = character.wallJumpForce * oppositeDirectionToWall;

        stateMachine.ChangeState(PlayerStateType.Fall);
    }


}