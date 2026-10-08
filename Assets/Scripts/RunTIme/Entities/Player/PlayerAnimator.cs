using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;

    #region Subscriptions

    private void OnEnable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt += HandleHurt;
        _playerCharacter.OnJump += HandleJump;
        _playerCharacter.OnStartTP += HandleStartTP;
        _playerCharacter.OnTP += HandleTP;
        _playerCharacter.OnCancelTP += HandleCancelTP;
        _playerCharacter.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt -= HandleHurt;
        _playerCharacter.OnJump -= HandleJump;
        _playerCharacter.OnStartTP -= HandleStartTP;
        _playerCharacter.OnTP -= HandleTP;
        _playerCharacter.OnCancelTP -= HandleCancelTP;
        _playerCharacter.OnAttack -= HandleAttack;
    }
    #endregion

    private void Start()
    {
        if (_playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter reference is not assigned in the inspector.");
            return;
        }
        Debug.Log("PlayerAnimator: PlayerCharacter reference is assigned.");
        _playerCharacter.CanMove = false;
        _animator.SetTrigger(AnimatorHashes.Spawn);
    }

    private void Update()
    {
        Debug.Log($"PlayerAnimator: Update called. PlayerCharacter is {(_playerCharacter == null ? "null" : "not null")}. CanMove is {(_playerCharacter != null ? _playerCharacter.CanMove.ToString() : "N/A")}.");
        if (_playerCharacter == null || !_playerCharacter.CanMove) return;

        Debug.Log("PlayerAnimator: Updating animations.");
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
    private void HandleHurt()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Hurt);
    }
    private void HandleJump()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Jump);
    }
    private void HandleAttack()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Attack);
    }
    private void HandleStartTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.StartTP);
    }
    private void HandleTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.TP);
    }
    private void HandleCancelTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.CancelTP);
    }
    #endregion

    public void AE_ActiveCanMove() => _playerCharacter.CanMove = true;
}
