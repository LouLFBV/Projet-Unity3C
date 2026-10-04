using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;

    //public bool IsTPing { get; set; } = false;

    #region Subscriptions

    private void OnEnable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt += HandleHurt;
        _playerCharacter.OnJump += HandleJump;
        _playerCharacter.OnTP += HandleTP;
        _playerCharacter.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt -= HandleHurt;
        _playerCharacter.OnJump -= HandleJump;
        _playerCharacter.OnTP -= HandleTP;
        _playerCharacter.OnAttack -= HandleAttack;
    }
    #endregion

    private void Update()
    {
        if (_playerCharacter == null) return;

        UpdateMovementAnimation();
        UpdatePhysicsAnimation();
    }

    private void UpdateMovementAnimation()
    {
        float speedRatio = Mathf.InverseLerp(0f, _playerCharacter.MaxMoveSpeed, Mathf.Abs(_playerCharacter.TargetAnimSpeed));
        _animator.SetFloat(AnimatorHashes.Speed, speedRatio);

        if (_playerCharacter.FacingDirection != 0)
        {
            _sprite.flipX = _playerCharacter.FacingDirection < 0;
        }
    }
    private void UpdatePhysicsAnimation()
    {
        _animator.SetBool(AnimatorHashes.IsGrounded, _playerCharacter.HitNormal.y != Vector2.zero.y);

        _animator.SetFloat(AnimatorHashes.JumpVelocity, _playerCharacter.Body.Velocity.y);
    }

    #region Handlers Events
    private void HandleHurt() => _animator.SetTrigger(AnimatorHashes.Hurt);
    private void HandleJump() => _animator.SetTrigger(AnimatorHashes.Jump);
    private void HandleAttack() => _animator.SetTrigger(AnimatorHashes.Attack);
    private void HandleTP()
    {
        //if (IsTPing) return;
        //IsTPing = true;
        //_animator.SetTrigger(AnimatorHashes.TP);
    }
    #endregion
}
