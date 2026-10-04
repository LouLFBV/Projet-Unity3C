using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _playerLayer;

    private bool _isAlreadyActivated = false;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _audioClip;
    private void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider2D>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_isAlreadyActivated)
        {
            CheckPlayer();
        }
    }

    private void CheckPlayer()
    {
        RaycastHit2D collider = Physics2D.BoxCast(
            transform.position + (Vector3)_collider.offset,
            _collider.size,
            0f,
            Vector2.zero,
            0f,
            _playerLayer
            );

        if (collider && collider.collider.CompareTag("Player"))
        {
            GiveHisPosition();
            _isAlreadyActivated = true;
            PlaySoundEffect();
            _animator.SetTrigger("Break");
            collider.collider.GetComponentInChildren<PlayerCharacter>().TriggerAttack();
        }
    }

    public void GiveHisPosition()
    {
        CheckpointManager.Instance.SetCheckpoint(transform.position);
        Debug.Log("Checkpoint activated at position: " + transform.position);
    }

    public void PlaySoundEffect()
    {
        if (_audioSource != null && _audioClip.Length > 0)
        {
            int randomIndex = Random.Range(0, _audioClip.Length);
            AudioClip clipToPlay = _audioClip[randomIndex];
            _audioSource.PlayOneShot(clipToPlay);
        }
    }
}
