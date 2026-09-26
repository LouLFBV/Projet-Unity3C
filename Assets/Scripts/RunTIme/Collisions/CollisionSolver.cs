using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Contains the movement data used during a physics frame.
/// </summary>
public class FramePhysicsData
{
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
}
/// <summary>
/// Resolves collisions by executing a series of collision checks
/// on the current movement data.
/// </summary>
public class CollisionSolver : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// List of collision checks executed by the collision solver.
    /// </summary>
    [SerializeField] List<CollisionCheck> _checks = new(); 
    private FramePhysicsData _frameData = new FramePhysicsData();
    /// <summary>
    /// Processes the provided movement by applying the registered
    /// collision checks sequentially.
    /// </summary>
    /// <param name="move">
    /// Movement to process. The value is updated with the result
    /// of the collision resolution.
    /// </param>
    public void Dispatch(ref Vector2 move)
    {
        _frameData.Move = move;
        foreach (var check in _checks)
        {
            if (!check)
                continue;
            check.ExecuteCollision(ref _frameData);
        }
        move = _frameData.Move;
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
}
