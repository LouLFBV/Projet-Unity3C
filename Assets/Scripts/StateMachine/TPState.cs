using UnityEngine;

class TPState : PlayerState
{
    private float _tpAnimationDuration = 1.483f; 
    private float _tpTimer = float.MinValue;
    public TPState(PlayerCharacter character, PlayerStateMachine stateMachine)
        : base(character, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("<color=yellow>TPState Enter</color>");
        //ExecuteTP(); // Pour ne pas avoir l'animation

        if (character.AnimatorPlayerScript.isTPing || !character.ManaSystem.HasEnoughMana(character.costTP))
        {
            stateMachine.PopState();
            return;
        }
        _tpTimer = Time.time;
       character.AnimatorPlayerScript.SetTPAnimation();
    }

    public override void Update() 
    {
        if (Time.time - _tpTimer >= _tpAnimationDuration)
        {
            ExecuteTP();
        }
    }
    public override void FixedUpdate() { }

    public override void Exit() { }

    private void ExecuteTP()
    {
        float tpDistance = character.distanceToTP;
        Vector2 deltaPosition = new Vector2(tpDistance * character.FacingDirection, 0);

        ProcessTeleportation(ref deltaPosition);

        character.transform.Translate(deltaPosition);

        character.AnimatorPlayerScript.isTPing = false;

        character.ManaSystem.ConsumeMana(character.costTP);

        stateMachine.PopState();
    }

    private void ProcessTeleportation(ref Vector2 deltaPosition)
    {
        if (deltaPosition.x != 0)
        {
            ProcessHorizontalCollisions(ref deltaPosition);
        }
    }

    private void ProcessHorizontalCollisions(ref Vector2 deltaPosition)
    {
        float directionX = Mathf.Sign(deltaPosition.x);

        RaycastHit2D hit = Physics2D.BoxCast(
            character.transform.position,
            character.Collider.size,
            0,
            Vector2.right * directionX,
            Mathf.Abs(deltaPosition.x) + character.SkinWidth,
            character.GroundLayer 
        );

        if (hit)
        {
            deltaPosition.x = Mathf.Max(0, hit.distance - character.SkinWidth) * directionX;
        }
    }
}