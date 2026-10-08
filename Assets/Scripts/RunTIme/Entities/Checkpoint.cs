using UnityEngine;
/// <summary>
/// Detects when the player reaches a checkpoint and activates it.
/// Handles checkpoint registration, visual effects, audio feedback,
/// and player animation.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
    #region --- REFERENCES ---

    /// <summary>
    /// Collider defining the area used to detect the player.
    /// </summary>
    [SerializeField] private BoxCollider2D _collider;
    /// <summary>
    /// Animator controlling the checkpoint activation animation.
    /// </summary>
    [SerializeField] private Animator _animator;
    /// <summary>
    /// Layer mask used to detect the player.
    /// </summary>
    [SerializeField] private LayerMask _playerLayer;
    /// <summary>
    /// Particle system played when the checkpoint is activated.
    /// </summary>
    [SerializeField] private ParticleSystem _brekParticleSystem;
    /// <summary>
    /// Audio source used to play the checkpoint activation sound.
    /// </summary>
    private AudioSource _audioSource;
    #endregion

    #region --- STATE ---

    /// <summary>
    /// Indicates whether the checkpoint has already been activated.
    /// </summary>

    [SerializeField] private AudioClip[] _audioClip;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the checkpoint component references.
    /// </summary>
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


    #endregion

    #region --- CHECKPOINT ACTIVATION ---


    /// <summary>
    /// Activates the checkpoint and triggers its associated effects.
    /// Called by the new collision system when the player reaches the checkpoint.
    /// </summary>
    public void HandleCheckPoint()
    {
        _collider.enabled = false;
        GiveHisPosition(); 
        if (_brekParticleSystem != null)
        {
            _brekParticleSystem.Play();
        }
        PlaySoundEffect();
        _animator.SetTrigger("Break");
    }


    /// <summary>
    /// Registers this checkpoint's position as the player's current respawn point.
    /// </summary>
    public void GiveHisPosition()
    {
        CheckpointManager.Instance.SetCheckpoint(transform.position);
        Debug.Log("Checkpoint activated at position: " + transform.position);
    }

    #endregion

    #region --- AUDIO ---

    /// <summary>
    /// Plays a randomly selected checkpoint activation sound effect.
    /// </summary>
    public void PlaySoundEffect()
    {
        if (_audioSource != null && _audioClip.Length > 0)
        {
            int randomIndex = Random.Range(0, _audioClip.Length);
            AudioClip clipToPlay = _audioClip[randomIndex];
            _audioSource.PlayOneShot(clipToPlay);
        }
    }
    #endregion
}
