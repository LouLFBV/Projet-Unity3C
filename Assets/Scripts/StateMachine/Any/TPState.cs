using UnityEngine;

class TPState : PlayerState
{
    private float _tpAnimationDuration = 1.483f; 
    private float _tpTimer = float.MinValue;
    public TPState(PlayerCharacter character) : base(character) { }

    public override void Enter()
    {
        Debug.Log("<color=yellow>TPState Enter</color>");

        if (!Character.ManaSystem.HasEnoughMana(Character.CostTP))
        {
            SetPopState(1);
            return;
        }

        Time.timeScale = Character.TimeScaleInTP; // Ralentir le temps pour l'animation

        //ExecuteTP(); // Pour ne pas avoir l'animation

        _tpTimer = Time.unscaledTime;
        Character.TriggerStartTP();
    }

    public override void Update()
    {
        if (!Character.ManaSystem.HasEnoughMana(Character.CostTP))
        {
            SetPopState(1);
            return;
        }
        if (Character.IsCancelTP)
        {
            Character.TriggerTP();
            SetPopState(1);
            return;
        }

        if (Time.time - _tpTimer >= _tpAnimationDuration || !Character.IsInTP)
        {
            ExecuteTP();
        }

    }
    public override void FixedUpdate() { }

    public override void Exit()
    {
        Time.timeScale = 1f;
    }

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
        float tpDistance = Character.DistanceTP;
        Vector2 deltaPosition = new(tpDistance * Character.FacingDirection, 0);

        // Appliquer directement la nouvelle position sur le PhysicBody
        Character.Body.SetPosition(Character.Body.Position + deltaPosition);

        // Réinitialiser la vitesse
        Character.Body.SetVelocity(Vector2.zero);

        //    ProcessTeleportation(ref deltaPosition);
        //    _character.transform.Translate(deltaPosition);
        //    _character.AnimatorPlayerScript.isTPing = false;

        if (Character.ManaSystem != null)
        {
            Character.ManaSystem.ConsumeMana(Character.CostTP);
        }

        Character.TriggerTP();

        SetPopState(1);
        Time.timeScale = 1f; 
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