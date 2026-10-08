using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Manages the player's current checkpoint and handles player respawning.
/// </summary>
public class CheckpointManager : SingletonMonoObject<CheckpointManager>
{
    #region --- CHECKPOINT ---

    /// <summary>
    /// Current world position used as the player's respawn point.
    /// </summary>
    private Vector3 _currentSpawnPosition;
    #endregion

    #region --- REFERENCES ---

    /// <summary>
    /// Physics body associated with the player to be respawned.
    /// </summary>
    private PhysicBody _body;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the checkpoint manager and resets the current spawn position.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        _currentSpawnPosition = Vector3.zero;
    }
    #endregion

    #region --- CHECKPOINT MANAGEMENT ---

    /// <summary>
    /// Sets the current checkpoint position.
    /// </summary>
    /// <param name="newPosition">World position of the new checkpoint.</param>
    public void SetCheckpoint(Vector3 newPosition)
    {
      _currentSpawnPosition = newPosition;
    }
    /// <summary>
    /// Sets the physics body associated with the player.
    /// </summary>
    /// <param name="body">Physics body of the player.</param>
    public void SetBody(PhysicBody body)
    {
        _body = body;
    }

    #endregion

    #region --- PLAYER RESPAWN ---

    /// <summary>
    /// Respawns the player at the current checkpoint position.
    /// </summary>
    public void RespawnPlayer()
    {
        if(!_body)
            return;

        _body.Tp(_currentSpawnPosition);
    }
    /// <summary>
    /// Respawns the player at the current checkpoint position
    /// and restores the specified velocity.
    /// </summary>
    /// <param name="velocity">Velocity to apply after respawning.</param>
    public void RespawnPlayer(Vector2 velocity)
    {
        if (!_body)
            return;

        _body.Tp(_currentSpawnPosition);
        _body.SetVelocity(velocity);
    }
    #endregion
}
