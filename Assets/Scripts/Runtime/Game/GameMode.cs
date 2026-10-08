using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// Initializes and configures the main gameplay systems,
/// including the player, cameras, checkpoints, timer, audio, and camera shake systems.
/// </summary>
public class GameMode : MonoBehaviour
{
    #region --- CAMERA SETTINGS ---
    /// <summary>
    /// List of Cinemachine cameras used by the game mode.
    /// Each camera is configured to follow the spawned player character.
    /// </summary>
    [Header("Cinemachine Cameras")]
    [SerializeField] private List<CinemachineCamera> _cinemachineCameras;
    /// <summary>
    /// Prefab used to create the camera shake manager.
    /// </summary>
    [SerializeField] private GameObject _cameraShakeManagerPrefab;
    #endregion

    #region --- MANAGER PREFABS ---
    /// <summary>
    /// Prefab used to create the timer manager.
    /// </summary>
    [Header("Prefabs")]
    [SerializeField] private GameObject _timerManagerPrefab;
    /// <summary>
    /// Prefab used to create the checkpoint manager.
    /// </summary>
    [SerializeField] private GameObject _checkpointManagerPrefab;
    /// <summary>
    /// Prefab used to create the audio manager.
    /// </summary>
    [SerializeField] private GameObject _audioManagerPrefab;
    #endregion

    #region --- PLAYER SETTINGS ---
    /// <summary>
    /// Prefab used to create the player controller.
    /// </summary>
    [Header("Player Prefabs")]
    [SerializeField] private GameObject _playerControllerPrefab;
    /// <summary>
    /// Prefab used to create the player character.
    /// </summary>
    [SerializeField] private GameObject _playerCharacterPrefab;
    /// <summary>
    /// Transform defining the initial spawn position of the player.
    /// </summary>
    [SerializeField] private Transform _spawnPoint;
    #endregion

    #region --- AUDIO SETTINGS ---
    /// <summary>
    /// Music clip assigned to the audio manager when the game mode is initialized.
    /// </summary>
    [Header("Music")]
    [SerializeField] private AudioClip _musicClip;

    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the gameplay systems and creates the required player,
    /// camera shake manager, checkpoint manager, timer manager, and audio manager.
    /// Also configures the Cinemachine cameras to follow the spawned player character.
    /// </summary>
    private void Awake()
    {
        if (!_playerControllerPrefab)
            Debug.LogError("PlayerControllerPrefab is not assigned in GameMode");
        if (!_playerCharacterPrefab)
            Debug.LogError("PlayerCharacterPrefab is not assigned in GameMode");
        GameObject playerControllerObj = Instantiate(_playerControllerPrefab, _spawnPoint.position, Quaternion.identity);
        GameObject playerCharacterObj = null;
        if (_playerCharacterPrefab == _playerControllerPrefab)
            playerCharacterObj = playerControllerObj;
        else
            playerCharacterObj = Instantiate(_playerCharacterPrefab, _spawnPoint.position, Quaternion.identity);
        PlayerController playerController = playerControllerObj.GetComponent<PlayerController>();
        PlayerControllerPort playerControllerPort = playerCharacterObj.GetComponent<PlayerControllerPort>();
        PlayerCharacter playerCharacter = playerCharacterObj.GetComponent<PlayerCharacter>();

        GameObject cameraShakeManagerObj = Instantiate(_cameraShakeManagerPrefab);
        CameraShakeManager _cameraShakeManager = cameraShakeManagerObj.GetComponent<CameraShakeManager>();
        _cameraShakeManager.Initialize(playerCharacter);
        Transform transform = null;
        if (playerController)
        {
            playerControllerPort.SetController(playerController);
            playerController.SetController(playerControllerPort);
            playerControllerPort.SetObject(playerCharacter);
            transform = playerCharacter.transform;

        }
        foreach (var cam in _cinemachineCameras)
            cam.Follow = transform;
        if (_checkpointManagerPrefab)
        {
            GameObject checkpointManagerObj = Instantiate(_checkpointManagerPrefab);
            CheckpointManager manager = checkpointManagerObj.GetComponent<CheckpointManager>();
            manager.SetCheckpoint(_spawnPoint.position);

            manager.SetBody(playerCharacterObj.GetComponent<PhysicBody>());
            manager.RespawnPlayer();
        }
        if (_timerManagerPrefab)
            Instantiate(_timerManagerPrefab);
        if (_audioManagerPrefab)
        {
            GameObject audioManagerObj = Instantiate(_audioManagerPrefab);
            AudioManager audioManager = audioManagerObj.GetComponent<AudioManager>();
            audioManager.SetPlayerCharacter(playerCharacter);
            if (_musicClip)
            {
                audioManager.SetMusicClip(_musicClip);
            }
        }
    }
    #endregion

}
