using UnityEngine;

/// <summary>
/// Handles collision detection for the player's respawn and death conditions.
/// Triggers the <see cref="DeathState"/> when a valid collision is detected.
/// </summary>
public class RespawnCollisionCheck : CollisionCheck
{
    #region --- REFERENCES ---

    /// <summary>
    /// Physics body used to retrieve the player's position and velocity.
    /// </summary>
    [SerializeField] private PhysicBody _body;
    /// <summary>
    /// Player character associated with this collision check.
    /// </summary>
    [SerializeField] private PlayerCharacter _player;
    #endregion

    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the player's collision rays.
    /// </summary>
    FramePhysicsData _data = new();
    #endregion

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes detected collisions and triggers the player's death state
    /// when a valid respawn collision is detected.
    /// </summary>
    /// <param name="hits">Array of raycast hits detected during the collision check.</param>
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body ||!_player)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);

        if (rayCount == 0)
            return;
        if (_player.PlayerStateMachine.CurrentState is not DeathState)
        {
            _player.PlayerStateMachine.CurrentState.SetNextState<DeathState>(true);
        }


    }
    #endregion
}
