using UnityEngine;

/// <summary>
/// Handles the finish line activation and triggers the associated
/// audio, particle, transition, and timer effects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FlagTrigger : MonoBehaviour
{
    #region --- DETECTION ---

    /// <summary>
    /// Collider defining the area associated with the finish line.
    /// Collision detection is handled by the new collision system.
    /// </summary>
    [SerializeField] private BoxCollider2D _triggerCollider;
    #endregion

    #region --- FEEDBACK ---

    /// <summary>
    /// Audio clip played when the player crosses the finish line.
    /// </summary>
    [SerializeField] private AudioClip _flagSound;
    /// <summary>
    /// Particle system played when the player crosses the finish line.
    /// </summary>
    [SerializeField] private ParticleSystem _confetti;
    /// <summary>
    /// Animator used to trigger the end-of-level transition.
    /// </summary>

    [SerializeField] private Animator _transitionAnimator;
    #endregion

    #region --- STATE ---

 
    /// <summary>
    /// Audio source used to play the finish line sound.
    /// </summary>
    private AudioSource _audioSource;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the trigger collider and audio source references.
    /// </summary>
    private void Awake()
    {
        if (_triggerCollider == null)
            _triggerCollider = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Activates the finish line and triggers the associated
    /// audio, particle, transition, and timer effects.
    /// Called by the new collision system when the player reaches the finish line.
    /// </summary>
    public void TrigerFlag()
    {
        _triggerCollider.enabled = false;
        if (_audioSource != null && _flagSound != null)
        {
            _audioSource.PlayOneShot(_flagSound);
        }
        Debug.Log("Le joueur a franchi la ligne d'arrivée !");

        if (_confetti != null)
        {
            _confetti.Play();
        }

        if (_transitionAnimator != null)
        {
            _transitionAnimator.SetTrigger("StartTransition");
        }

        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.StopTimer();
        }
    }
    #endregion
}