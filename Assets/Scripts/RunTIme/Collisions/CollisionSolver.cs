using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Contains the movement data used during a physics frame.
/// </summary>
public class FramePhysicsData
{
    #region --- MOVEMENT DATA ---

    /// <summary>
    /// Current delta position between render and physics.
    /// </summary>
    public Vector2 Pos = Vector2.zero;
    /// <summary>
    /// Current movement vector.
    /// </summary>
    public Vector2 Move;
    /// <summary>
    /// Gets the normalized movement vector.
    /// </summary>
    public Vector2 MoveNormalized => Move.normalized;
    /// <summary>
    /// Gets the magnitude of the current movement vector.
    /// </summary>
    public float MoveMagnitude => Move.magnitude;
#endregion
}
/// <summary>
/// Resolves collisions by executing a series of collision checks
/// on the current movement data.
/// </summary>

public class CollisionSolver : MonoBehaviour
{
    #region --- PROPERTIES ---

    /// <summary>
    /// List of collision checks executed by the collision solver.
    /// </summary>
    [Header("Properties")]
    [SerializeField] List<CollisionCheck> _checks = new();
    /// <summary>
    /// Movement data used during the current physics frame.
    /// </summary>
    private FramePhysicsData _frameData = new FramePhysicsData();
    #endregion

    #region --- COLLISION MANAGEMENT ---

    /// <summary>
    /// Processes all registered collision checks sequentially.
    /// </summary>
    public void Dispatch()
    {

        foreach (var check in _checks)
        {
            if (!check)
                continue;
            check.ExecuteCollision();
        }
      
    }
    /// <summary>
    /// Adds a collision check to the list of checks executed during resolution.
    /// </summary>
    /// <param name="check">Collision check to add.</param>
    public void Add(CollisionCheck check)
    {
        _checks.Add(check);
    }
    /// <summary>
    /// Removes a collision check from the list of checks executed during resolution.
    /// </summary>
    /// <param name="check">Collision check to remove.</param>
    public void Remove(CollisionCheck check)
    {
        _checks.Remove(check);
    }
#endregion
}
