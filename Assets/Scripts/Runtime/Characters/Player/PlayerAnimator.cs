using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;
    public Animator AnimatorPlayer => _animator;
    
    public void SetMoveAnimation(float moveSpeed, float maxMoveSpeed)
    {
        _animator.SetFloat("Speed", Mathf.InverseLerp(0f, maxMoveSpeed, Mathf.Abs(moveSpeed)));
        if (Mathf.Abs(moveSpeed) > 0.01f)
        {
            _sprite.flipX = moveSpeed < 0;
        }
    }
}
