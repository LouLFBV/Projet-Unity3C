using UnityEngine;
/// <summary>
/// Handles collision detection for checkpoints.
/// Triggers the checkpoint activation and the player's attack
/// when a valid collision with a checkpoint is detected.
/// </summary>
public class CheckPointCollisionCheck : CollisionCheck
{
    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the current collision frame.
    /// </summary>
    private FramePhysicsData _data = new();
    #endregion

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

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes detected collisions and triggers the associated
    /// <see cref="Checkpoint"/> when a valid collision is detected.
    /// </summary>
    /// <param name="hits">Array of raycast hits detected during the collision check.</param>
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body || !_player)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);

        if (rayCount == 0)
            return;
        
        for(int i = 0; i < rayCount; i++)
        {
            RaycastHit2D hit = hits[i];
            hit.collider.gameObject.GetComponent<Checkpoint>()?.HandleCheckPoint();
            _player.TriggerAttack();
        }
    }   
    #endregion
}   