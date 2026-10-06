using UnityEngine;
class DeathState : PlayerState
{
    public DeathState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        Debug.Log("<color=red>Le joueur est mort, réapparition au dernier checkpoint !</color>");
        Character.TriggerHurt();
        CheckpointManager.Instance.RespawnPlayer();
        Character.ManaSystem.ResetMana();
        SetNextState<IdleState>();
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}