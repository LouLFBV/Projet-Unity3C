using UnityEngine;

class TPState : PlayerState
{
    private float _tpAnimationDuration = 1.483f; 
    private float _tpTimer = float.MinValue;
    public TPState(PlayerCharacter character) : base(character) { }

    public override void Enter()
    {
        Debug.Log("<color=yellow>TPState Enter</color>");

        if (Character.AnimatorPlayerScript.isTPing || !Character.ManaSystem.HasEnoughMana(Character.costTP))
        {
            SetPopState(1);
            //_stateMachine.PopState();
            return;
        }

       ExecuteTP(); // Pour ne pas avoir l'animation

       // _tpTimer = Time.time;
       //character.AnimatorPlayerScript.SetTPAnimation();
    }

    public override void Update() 
    {
        //if (Time.time - _tpTimer >= _tpAnimationDuration)
        //{
        //    ExecuteTP();
        //}
    }
    public override void FixedUpdate() { }

    public override void Exit() { }

    //private void ExecuteTP()
    //{
    //    float tpDistance = _character.distanceToTP;
    //    Vector2 deltaPosition = new Vector2(tpDistance * _character.FacingDirection, 0);

    //    ProcessTeleportation(ref deltaPosition);

    //    _character.transform.Translate(deltaPosition);

    //    _character.AnimatorPlayerScript.isTPing = false;

    //    _character.ManaSystem.ConsumeMana(_character.costTP);

    //    _stateMachine.PopState();
    //}

    private void ExecuteTP()
    {
        float tpDistance = Character.distanceToTP;
        Vector2 deltaPosition = new Vector2(tpDistance * Character.FacingDirection, 0);

        // Appliquer directement la nouvelle position sur le PhysicBody
        Character.Body.SetPosition(Character.Body.Position + deltaPosition);

        // Réinitialiser la vitesse
        Character.Body.SetVelocity(Vector2.zero);

        //    ProcessTeleportation(ref deltaPosition);
        //    _character.transform.Translate(deltaPosition);
        //    _character.AnimatorPlayerScript.isTPing = false;

        if (Character.ManaSystem != null)
        {
            Character.ManaSystem.ConsumeMana(Character.costTP);
        }

        SetPopState(1);
        //_stateMachine.PopState();
    }

    //private void ProcessTeleportation(ref Vector2 deltaPosition)
    //{
    //    if (deltaPosition.x != 0)
    //    {
    //        ProcessHorizontalCollisions(ref deltaPosition);
    //    }
    //}

    //private void ProcessHorizontalCollisions(ref Vector2 deltaPosition)
    //{
    //    float directionX = Mathf.Sign(deltaPosition.x);

    //    RaycastHit2D hit = Physics2D.BoxCast(
    //        _character.transform.position,
    //        _character.Collider.size,
    //        0,
    //        Vector2.right * directionX,
    //        Mathf.Abs(deltaPosition.x) + _character.SkinWidth,
    //        _character.GroundLayer 
    //    );

    //    if (hit)
    //    {
    //        deltaPosition.x = Mathf.Max(0, hit.distance - _character.SkinWidth) * directionX;
    //    }
    //}
}