using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FlagTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private BoxCollider2D _triggerCollider;
    [SerializeField] private AudioClip _flagSound;
    [SerializeField] private ParticleSystem _confetti;

    private bool _hasTriggered = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_triggerCollider == null)
            _triggerCollider = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_hasTriggered) return;

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + (Vector3)_triggerCollider.offset,
            _triggerCollider.size,
            0f,
            Vector2.zero,
            0f,
            _playerLayer
        );

        if (hit)
        {
            _hasTriggered = true;
            if (_audioSource != null && _flagSound != null)
            {
                _audioSource.PlayOneShot(_flagSound);
            }
            Debug.Log("Le joueur a franchi la ligne d'arrivée !");

            if(_confetti != null)
            {
                _confetti.Play();
            }

            if (TimerManager.Instance != null)
            {
                TimerManager.Instance.StopTimer();
            }
        }
    }
}