using UnityEngine;
/// <summary>
/// Handles collision detection for moving platforms.
/// </summary>
public class MovingPlatformCollisionCheck : CollisionCheck
{
    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the moving platform's collision information.
    /// </summary>
    FramePhysicsData _data = new();
    #endregion

    #region --- REFERENCES ---

    /// <summary>
    /// Physics body associated with the moving platform.
    /// </summary>
    [SerializeField] private PhysicBody _body;
    #endregion

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes collisions detected for the moving platform.
    /// </summary>
    /// <param name="hits">Array of raycast hits detected during the collision check.</param>
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
    }
    #endregion
}
