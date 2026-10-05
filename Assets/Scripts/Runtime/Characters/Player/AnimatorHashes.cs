using UnityEngine;

public static class AnimatorHashes
{
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int JumpVelocity = Animator.StringToHash("JumpVelocity");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int StartTP = Animator.StringToHash("StartTP");
    public static readonly int TP = Animator.StringToHash("TP");
    public static readonly int Hurt = Animator.StringToHash("Hurt");

    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
}