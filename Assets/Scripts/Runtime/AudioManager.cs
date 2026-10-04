using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceClip;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _sprintClip;
    [SerializeField] private AudioClip _tpClip;
    [SerializeField] private AudioClip _groundClip;
    [SerializeField] private AudioClip _attackClip;

    private PlayerCharacter _playerCharacter;

    #region Subscriptions
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

    #region Set Methods
    public void SetMusicClip(AudioClip clip)
    {
        if (_audioSourceMusic != null)
        {
            _audioSourceMusic.clip = clip;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
    }
    public void SetPlayerCharacter(PlayerCharacter character)
    {
        _playerCharacter = character ;
        if (character != null)
        {
            OnEnable();
        }
    }
    #endregion

    #region Play Sound Methods
    private void PlayJumpSound()
    {
        if (_jumpClip == null)
        {
            return;
        }
        PlayClip(_jumpClip);
    }
    private void PlayHurtSound()
    {
        if (_hurtClip == null)
        {
            return;
        }
        PlayClip(_hurtClip);
    }
    private void PlaySprintSound()
    {
        if (_sprintClip == null)
        {
            return;
        }
        PlayClip(_sprintClip);
    }
    private void PlayTPSound()
    {
        if (_tpClip == null)
        {
            return;
        }
        PlayClip(_tpClip);
    }
    private void PlayGroundSound()
    {
        if (_groundClip == null)
        {
            return;
        }
        PlayClip(_groundClip);
    }
    private void PlayAttackSound()
    {
        if (_attackClip == null)
        {
            return;
        }
        PlayClip(_attackClip);
    }

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