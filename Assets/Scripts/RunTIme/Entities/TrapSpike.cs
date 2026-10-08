using UnityEngine;
/// <summary>
/// Controls a spike trap that can automatically or proximity-triggered attack the player.
/// Handles attack detection, animation events, and attack audio playback.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TrapSpike : MonoBehaviour
{
    /// <summary>
    /// Layer mask used to detect valid player targets.
    /// </summary>
    [SerializeField] private LayerMask _playerLayer;
    /// <summary>
    /// Determines whether the trap attacks automatically through its animation
    /// or only when a player enters the detection area.
    /// </summary>
    [SerializeField] private bool _isAutomatic = true;
    /// <summary>
    /// Collider defining the area affected by the active spike attack.
    /// </summary>  
    [SerializeField] private BoxCollider2D _triggerCollider;
    /// <summary>
    /// Collider defining the area used to detect nearby players.
    /// </summary>
    [SerializeField] private CircleCollider2D _detectionCollider;
    /// <summary>
    /// Animator controlling the spike trap animations.
    /// </summary>
    [SerializeField] private Animator _animator;
    /// <summary>
    /// Audio clip played when the spike attack is triggered.
    /// </summary>
    [SerializeField] private AudioClip _audioclip;

    /// <summary>
    /// Indicates whether the trap's attack hitbox is currently active.
    /// </summary>
    private bool _isAttacking = false;
    /// <summary>
    /// Indicates whether the trap is currently playing an attack animation.
    /// </summary>
    private bool _isInAnimation = false;
    /// <summary>
    /// Audio source used to play the trap's attack sound.
    /// </summary>
    private AudioSource _audioSource;
    /// <summary>
    /// Initializes the trap's component references.
    /// </summary>

    private void Awake()
    {
        if (_triggerCollider == null) _triggerCollider = GetComponent<BoxCollider2D>();
        if (_detectionCollider == null) _detectionCollider = GetComponent<CircleCollider2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Initializes the trap animator with its configured activation mode.
    /// </summary>
    private void Start()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsAutomatic", _isAutomatic);
        }
    }

    /// <summary>
    /// Updates the trap attack state and checks for nearby players when
    /// the trap is configured to operate manually or through proximity detection.
    /// </summary>
    private void Update()
    {
        if (_isAttacking)
            _triggerCollider.enabled = true;
        else
            _triggerCollider.enabled = false;


        if (!_isAutomatic)
        {
            float radius = _detectionCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);

            Collider2D detectedPlayer = Physics2D.OverlapCircle(
                _detectionCollider.bounds.center,
                radius,
                _playerLayer
            );

            if (detectedPlayer != null && !_isAttacking && !_isInAnimation)
            {
                _animator.SetTrigger("Attack");
            }
        }
    }

    #region --- ANIMATOR EVENTS ---

    /// <summary>
    /// Activates the spike attack hitbox.
    /// Called by an animation event when the attack becomes active.
    /// </summary>
    public void AE_ActiveAttack() => _isAttacking = true;

    /// <summary>
    /// Deactivates the spike attack hitbox.
    /// Called by an animation event when the attack ends.
    /// </summary>
    public void AE_DesactiveAttack() => _isAttacking = false;
    /// <summary>
    /// Marks the trap as being inside its attack animation.
    /// Called by an animation event.
    /// </summary>
    public void AE_ActiveIsInAnimation() => _isInAnimation = true;
    /// <summary>
    /// Marks the trap as no longer being inside its attack animation.
    /// Called by an animation event.
    /// </summary>
    public void AE_DesactiveIsInAnimation() => _isInAnimation = false;
    /// <summary>
    /// Plays the configured attack audio clip.
    /// Called by an animation event.
    /// </summary>
    public void AE_PlayAudioClip()
    {
        if (_audioSource != null && _audioclip != null)
        {
            _audioSource.PlayOneShot(_audioclip);
        }
    }
    #endregion 
}