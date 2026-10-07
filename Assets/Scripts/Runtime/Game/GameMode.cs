using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private List<CinemachineCamera> _cinemachineCameras;
    [SerializeField] private GameObject _cameraShakeManagerPrefab;

    [Header("Prefabs")]

    [SerializeField] private GameObject _timerManagerPrefab;
    [SerializeField] private GameObject _checkpointManagerPrefab;
    [SerializeField] private GameObject _audioManagerPrefab;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject _playerControllerPrefab;
    [SerializeField] private GameObject _playerCharacterPrefab;
    [SerializeField] private Transform _spawnPoint;

    [Header("Music")]
    [SerializeField] private AudioClip _musicClip;


    private void Awake()
    {
        // controller
        // 

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
        TestController testController = playerControllerObj.GetComponent<TestController>();


        PlayerControllerPort playerControllerPort = playerCharacterObj.GetComponent<PlayerControllerPort>();
        TestControllerPort testControllerPort = playerCharacterObj.GetComponent<TestControllerPort>();

        PlayerCharacter playerCharacter = playerCharacterObj.GetComponent<PlayerCharacter>();
        TestMovement testMovement = playerCharacterObj.GetComponent<TestMovement>();

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
        else
        {
            testControllerPort.SetController(testController);
            testController.SetController(testControllerPort);
            testControllerPort.SetObject(testMovement);
            transform = testMovement.transform;



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

}
