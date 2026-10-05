using UnityEngine;

class FallState : AirboneState
{
    public FallState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
        Character.Body.SetGravity(true, Character.FallingGravity);
    }
    public override void Update()
    {
        base.Update();

        if (Character.HitNormal.y > 0.5f)
        {
            Debug.Log("<color=green>FallState</color> - Grounded");

            if (Character.GetPlayerDirection() != 0)
            {
                SetNextState<WalkState>();
            }
            else
            {
                SetNextState<IdleState>();
            }
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}