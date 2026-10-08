using UnityEngine;
/// <summary>
/// Handles the player's Animator state and updates animations based on the player's actions and movement.
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    #region --- REFERENCES ---

    /// <summary>
    /// Reference to the player character controlled by this animator.
    /// </summary>
    [SerializeField] private PlayerCharacter _playerCharacter;
    /// <summary>
    /// Reference to the player's sprite renderer.
    /// </summary>
    [SerializeField] private SpriteRenderer _sprite;
    /// <summary>
    /// Reference to the player's Animator component.
    /// </summary>
    [SerializeField] private Animator _animator;
    #endregion

    #region --- SUBSCRIPTIONS ---

    /// <summary>
    /// Subscribes to the player character's events when this component is enabled.
    /// </summary>
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
    /// <summary>
    /// Unsubscribes from the player character's events when this component is disabled.
    /// </summary>
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

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the player animator and triggers the player spawn animation.
    /// </summary>
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
    /// <summary>
    /// Updates the player's movement and physics animations each frame.
    /// </summary>
    private void Update()
    {
        Debug.Log($"PlayerAnimator: Update called. PlayerCharacter is {(_playerCharacter == null ? "null" : "not null")}. CanMove is {(_playerCharacter != null ? _playerCharacter.CanMove.ToString() : "N/A")}.");
        if (_playerCharacter == null || !_playerCharacter.CanMove) return;

        Debug.Log("PlayerAnimator: Updating animations.");
        UpdateMovementAnimation();
        UpdatePhysicsAnimation();
    }
    #endregion

    #region --- ANIMATION UPDATES ---

    /// <summary>
    /// Updates the player's movement-related animation parameters.
    /// </summary>
    private void UpdateMovementAnimation()
    {
        float speedRatio = Mathf.InverseLerp(0f, _playerCharacter.MaxMoveSpeed, Mathf.Abs(_playerCharacter.TargetAnimSpeed));
        _animator.SetFloat(AnimatorHashes.Speed, speedRatio);

        if (_playerCharacter.FacingDirection != 0)
        {
            _sprite.flipX = _playerCharacter.FacingDirection < 0;
        }
    }
    /// <summary>
    /// Updates the player's physics-related animation parameters.
    /// </summary>
    private void UpdatePhysicsAnimation()
    {
        _animator.SetBool(AnimatorHashes.IsGrounded, _playerCharacter.HitNormal.y != Vector2.zero.y);

        _animator.SetFloat(AnimatorHashes.JumpVelocity, _playerCharacter.Body.Velocity.y);
    }
    #endregion

    #region --- EVENT HANDLERS ---

    /// <summary>
    /// Handles the player's hurt event and triggers the corresponding animation.
    /// </summary>
    private void HandleHurt()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Hurt);
    }
    /// <summary>
    /// Handles the player's jump event and triggers the corresponding animation.
    /// </summary>
    private void HandleJump()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Jump);
    }
    /// <summary>
    /// Handles the player's attack event and triggers the corresponding animation.
    /// </summary>
    private void HandleAttack()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.Attack);
    }

    /// <summary>
    /// Handles the player's teleport start event and triggers the corresponding animation.
    /// </summary>
    private void HandleStartTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.StartTP);
    }
    /// <summary>
    /// Handles the player's teleport event and triggers the corresponding animation.
    /// </summary>
    private void HandleTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.TP);
    }
    /// <summary>
    /// Handles the player's teleport cancellation event and triggers the corresponding animation.
    /// </summary>
    private void HandleCancelTP()
    {
        if (!_playerCharacter.CanMove)
        {
            return;
        }
        _animator.SetTrigger(AnimatorHashes.CancelTP);
    }

    #endregion

    #region --- ANIMATION EVENTS ---

    /// <summary>
    /// Enables player movement through an Animation Event.
    /// </summary>
    public void AE_ActiveCanMove() => _playerCharacter.CanMove = true;
#endregion
}
