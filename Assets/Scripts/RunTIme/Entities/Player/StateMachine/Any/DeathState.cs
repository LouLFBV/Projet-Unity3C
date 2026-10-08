using UnityEngine;

/// <summary>
/// Represents the state in which the player is dead and respawns
/// at the last available checkpoint.
/// </summary>
class DeathState : PlayerState
{

    /// <summary>
    /// Initializes a new instance of the <see cref="DeathState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public DeathState(PlayerCharacter character) : base(character) { }

    /// <summary>
    /// Handles the player's death by triggering the hurt animation,
    /// respawning at the last checkpoint, resetting mana, and transitioning
    /// back to the idle state.
    /// </summary>
    public override void Enter() 
    {
        Debug.Log("<color=red>Le joueur est mort, réapparition au dernier checkpoint !</color>");
        Character.TriggerHurt();
        CheckpointManager.Instance.RespawnPlayer();
        Character.ManaSystem.ResetMana();
        SetNextState<IdleState>();
    }
}