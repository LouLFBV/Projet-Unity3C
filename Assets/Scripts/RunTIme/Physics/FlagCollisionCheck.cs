using UnityEngine;
/// <summary>
/// Handles collision detection for the finish flag.
/// Triggers the associated <see cref="FlagTrigger"/> when the player
/// collides with the flag.
/// </summary>
public class FlagCollisionCheck : CollisionCheck
{
    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the current collision frame.
    /// </summary>
    private FramePhysicsData _data = new();

    #endregion

    #region --- REFERENCES ---

    /// <summary>
    /// Physics body used to retrieve the player's position.
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
    /// <see cref="FlagTrigger"/> on each valid collision.
    /// </summary>
    /// <param name="hits">Array of raycast hits detected during the collision check.</param>
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body || !_player)
            return;
        _data.Pos = _body.Position;
        _data.Move = Vector2.zero;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);
        if (rayCount == 0)
            return;

        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit2D hit = hits[i];
            hit.collider.gameObject.GetComponent<FlagTrigger>()?.TrigerFlag();
        }
    }
    #endregion
}
