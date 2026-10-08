using UnityEngine;
/// <summary>
/// Provides cached Animator parameter hashes used throughout the project.
/// </summary>
public static class AnimatorHashes
{
    #region --- ACTION HASHES ---

    /// <summary>
    /// Hash of the <c>Jump</c> Animator parameter.
    /// </summary>
    public static readonly int Jump = Animator.StringToHash("Jump");
    /// <summary>
    /// Hash of the <c>JumpVelocity</c> Animator parameter.
    /// </summary>
    public static readonly int JumpVelocity = Animator.StringToHash("JumpVelocity");
    /// <summary>
    /// Hash of the <c>Attack</c> Animator parameter.
    /// </summary>
    public static readonly int Attack = Animator.StringToHash("Attack");
    /// <summary>
    /// Hash of the <c>StartTP</c> Animator parameter.
    /// </summary>
    public static readonly int StartTP = Animator.StringToHash("StartTP");
    /// <summary>
    /// Hash of the <c>TP</c> Animator parameter.
    /// </summary>
    public static readonly int TP = Animator.StringToHash("TP");
    /// <summary>
    /// Hash of the <c>CancelTP</c> Animator parameter.
    /// </summary>
    public static readonly int CancelTP = Animator.StringToHash("CancelTP");
    /// <summary>
    /// Hash of the <c>Hurt</c> Animator parameter.
    /// </summary>
    public static readonly int Hurt = Animator.StringToHash("Hurt");
    /// <summary>
    /// Hash of the <c>Spawn</c> Animator parameter.
    /// </summary>
    public static readonly int Spawn = Animator.StringToHash("Spawn");
    #endregion

    #region --- STATE HASHES ---

    /// <summary>
    /// Hash of the <c>Speed</c> Animator parameter.
    /// </summary>
    public static readonly int Speed = Animator.StringToHash("Speed");
    /// <summary>
    /// Hash of the <c>IsGrounded</c> Animator parameter.
    /// </summary>
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    #endregion
}