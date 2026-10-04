using Unity.Cinemachine;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [SerializeField] private GameObject _timerManagerPrefab;

    [SerializeField] private GameObject _checkpointManagerPrefab;

    [SerializeField] private GameObject _audioManagerPrefab;


    [SerializeField] private GameObject _playerControllerPrefab;
    [SerializeField] private GameObject _playerCharacterPrefab;

    [SerializeField] private AudioClip _musicClip;


    [SerializeField] private Transform _spawnPoint;
    private void Awake()
    {
        GameObject playerControllerObj = Instantiate(_playerControllerPrefab, _spawnPoint.position, Quaternion.identity);

        PlayerController playerController = playerControllerObj.GetComponent<PlayerController>();
        PlayerControllerPort playerControllerPort = playerControllerObj.GetComponent<PlayerControllerPort>();

        playerControllerPort.SetController(playerController);


        GameObject playerCharacterObj = Instantiate(_playerCharacterPrefab, _spawnPoint.position, Quaternion.identity);
        PlayerCharacter playerCharacter = playerCharacterObj.GetComponent<PlayerCharacter>();

        playerControllerPort.SetObject(playerCharacter);
        playerController.SetPlayerCharacter(playerCharacter);



        _cinemachineCamera.Follow = playerCharacter.transform;



        GameObject timerManagerObj = Instantiate(_timerManagerPrefab);

        GameObject checkpointManagerObj = Instantiate(_checkpointManagerPrefab);
        checkpointManagerObj.GetComponent<CheckpointManager>().SetCheckpoint(_spawnPoint.position);




        GameObject audioManagerObj = Instantiate(_audioManagerPrefab);
        AudioManager audioManager = audioManagerObj.GetComponent<AudioManager>();
        audioManager.SetPlayerCharacter(playerCharacter);
        audioManager.SetMusicClip(_musicClip);
    }

}
