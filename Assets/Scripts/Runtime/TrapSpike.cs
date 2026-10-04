using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TrapSpike : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private bool _isAutomatic = true;
    [SerializeField] private BoxCollider2D _triggerCollider;
    [SerializeField] private CircleCollider2D _detectionCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _audioclip;

    private bool _isAttacking = false;
    private bool _isInAnimation = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_triggerCollider == null) _triggerCollider = GetComponent<BoxCollider2D>();
        if (_detectionCollider == null) _detectionCollider = GetComponent<CircleCollider2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsAutomatic", _isAutomatic);
        }
    }

    private void Update()
    {
        if (_isAttacking)
        {
            Collider2D hit = Physics2D.OverlapBox(
                _triggerCollider.bounds.center,
                _triggerCollider.bounds.size,
                transform.eulerAngles.z,
                _playerLayer
            );

            if (hit != null && hit.TryGetComponent<PlayerCharacter>(out var playerCharacter))
            {
                if (playerCharacter.PlayerStateMachine.CurrentState is not DeathState)
                {
                    playerCharacter.PlayerStateMachine.CurrentState.SetNextState<DeathState>();
                    _isAttacking = false;
                }
            }
        }

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

    #region Animator Events
    public void AE_ActiveAttack() => _isAttacking = true;
    public void AE_DesactiveAttack() => _isAttacking = false;
    public void AE_ActiveIsInAnimation() => _isInAnimation = true;
    public void AE_DesactiveIsInAnimation() => _isInAnimation = false;
    public void AE_PlayAudioClip()
    {
        if (_audioSource != null && _audioclip != null)
        {
            _audioSource.PlayOneShot(_audioclip);
        }
    }
    #endregion 
}