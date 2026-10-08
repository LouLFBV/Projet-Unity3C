using UnityEngine;

/// <summary>
/// Manages the player's audio feedback and background music.
/// Handles player event subscriptions and playback of associated sound effects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region --- AUDIO SOURCES ---
    [Header("Audio Sources")]

    /// <summary>
    /// Audio source used to play background music.
    /// </summary>
    [SerializeField] private AudioSource _audioSourceMusic;
    /// <summary>
    /// Audio source used to play sound effects.
    /// </summary>
    [SerializeField] private AudioSource _audioSourceClip;

    #endregion

    #region --- AUDIO CLIPS ---
    [Header("Audio Clips")]

    /// <summary>
    /// Sound effect played when the player jumps.
    /// </summary>
    [SerializeField] private AudioClip _jumpClip;
    /// <summary>
    /// Sound effect played when the player takes damage.
    /// </summary>
    [SerializeField] private AudioClip _hurtClip;
    /// <summary>
    /// Sound effect played when the player starts sprinting.
    /// </summary>
    [SerializeField] private AudioClip _sprintClip;
    /// <summary>
    /// Sound effect played when the player teleports.
    /// </summary>
    [SerializeField] private AudioClip _tpClip;
    /// <summary>
    /// Sound effect played when the player lands on the ground.
    /// </summary>
    [SerializeField] private AudioClip _groundClip;
    /// <summary>
    /// Sound effect played when the player attacks.
    /// </summary>
    [SerializeField] private AudioClip _attackClip;
    #endregion

    #region --- REFERENCES ---

    /// <summary>
    /// Player character whose events are used to trigger audio feedback.
    /// </summary>

    private PlayerCharacter _playerCharacter;

    #endregion

    #region --- SUBSCRIPTIONS ---

    /// <summary>
    /// Subscribes to player events used to trigger sound effects.
    /// </summary>
    private void OnEnable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnJump += PlayJumpSound;
        _playerCharacter.OnHurt += PlayHurtSound;
        _playerCharacter.OnSprint += PlaySprintSound;
        _playerCharacter.OnTP += PlayTPSound;
        _playerCharacter.OnGround += PlayGroundSound;
        _playerCharacter.OnAttack += PlayAttackSound;
    }

    /// <summary>
    /// Unsubscribes from player events to prevent callbacks after the manager is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnJump -= PlayJumpSound;
        _playerCharacter.OnHurt -= PlayHurtSound;
        _playerCharacter.OnSprint -= PlaySprintSound;
        _playerCharacter.OnTP -= PlayTPSound;
        _playerCharacter.OnGround -= PlayGroundSound;
        _playerCharacter.OnAttack -= PlayAttackSound;
    }

    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Validates that the required audio sources are assigned.
    /// </summary>
    public void Start()
    {
        if (_audioSourceMusic == null)
        {
            Debug.LogWarning("AudioManager: _audioSourceMusic is not assigned.");
        }
        if (_audioSourceClip == null)
        {
            Debug.LogWarning("AudioManager: _audioSourceClip is not assigned.");
        }
    }

    #endregion

    #region --- SET METHODS ---

    /// <summary>
    /// Sets and starts the specified music clip in a looping mode.
    /// </summary>
    /// <param name="clip">Music clip to play.</param>
    public void SetMusicClip(AudioClip clip)
    {
        if (_audioSourceMusic != null)
        {
            _audioSourceMusic.clip = clip;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
    }
    /// <summary>
    /// Sets the player character used to receive audio-related events
    /// and registers the corresponding event callbacks.
    /// </summary>
    /// <param name="character">Player character to associate with the audio manager.</param>
    public void SetPlayerCharacter(PlayerCharacter character)
    {
        _playerCharacter = character ;
        if (character != null)
        {
            OnEnable();
        }
    }
    #endregion

    #region --- PLAY SOUND METHODS ---

    /// <summary>
    /// Plays the jump sound effect.
    /// </summary>
    private void PlayJumpSound()
    {
        if (_jumpClip == null)
        {
            return;
        }
        PlayClip(_jumpClip);
    }
    /// <summary>
    /// Plays the hurt sound effect.
    /// </summary>
    private void PlayHurtSound()
    {
        if (_hurtClip == null)
        {
            return;
        }
        PlayClip(_hurtClip);
    }
    /// <summary>
    /// Plays the sprint sound effect.
    /// </summary>
    private void PlaySprintSound()
    {
        if (_sprintClip == null)
        {
            return;
        }
        PlayClip(_sprintClip);
    }
    /// <summary>
    /// Plays the teleport sound effect.
    /// </summary>
    private void PlayTPSound()
    {
        if (_tpClip == null)
        {
            return;
        }
        PlayClip(_tpClip);
    }
    /// <summary>
    /// Plays the ground landing sound effect.
    /// </summary>
    private void PlayGroundSound()
    {
        if (_groundClip == null)
        {
            return;
        }
        PlayClip(_groundClip);
    }
    /// <summary>
    /// Plays the attack sound effect.
    /// </summary>
    private void PlayAttackSound()
    {
        if (_attackClip == null)
        {
            return;
        }
        PlayClip(_attackClip);
    }
    /// <summary>
    /// Plays the specified audio clip through the sound effect audio source.
    /// </summary>
    /// <param name="clip">Audio clip to play.</param>
    private void PlayClip(AudioClip clip)
    {
        Debug.Log($"AudioManager: Attempting to play clip: {clip?.name ?? "null"}");
        if (clip != null && _audioSourceClip != null)
        {
            _audioSourceClip.PlayOneShot(clip);
        }
    }
    #endregion
}