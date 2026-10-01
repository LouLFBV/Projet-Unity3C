using Unity.Cinemachine;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [SerializeField] private GameObject _timerManager;

    [SerializeField] private GameObject _checkpointManager;


    [SerializeField] private GameObject _playerControllerPrefab;
    [SerializeField] private GameObject _playerCharacterPrefab;


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

        GameObject timerManagerObj = Instantiate(_timerManager);

        GameObject checkpointManagerObj = Instantiate(_checkpointManager);
        checkpointManagerObj.GetComponent<CheckpointManager>().SetCheckpoint(_spawnPoint.position);
    }

}
