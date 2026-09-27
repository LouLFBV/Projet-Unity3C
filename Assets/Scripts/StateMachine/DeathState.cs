using UnityEngine;
class DeathState : PlayerState
{
    public DeathState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter() 
    {
        Debug.Log("<color=red>Le joueur est mort, réapparition au dernier checkpoint !</color>");
        // TP le joueur au dernier checkpoint
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}