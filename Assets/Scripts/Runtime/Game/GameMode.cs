using Unity.Cinemachine;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;


    [SerializeField] private GameObject _playerControllerPrefab;
    [SerializeField] private GameObject _playerCharacterPrefab;

    [SerializeField] private Transform _spawnPoint;
    private void Start()
    {
        GameObject playerControllerObj = Instantiate(_playerControllerPrefab, _spawnPoint.position, Quaternion.identity);
        PlayerController playerController = playerControllerObj.GetComponent<PlayerController>();

        GameObject playerCharacterObj = Instantiate(_playerCharacterPrefab, _spawnPoint.position, Quaternion.identity);
        PlayerCharacter playerCharacter = playerCharacterObj.GetComponent<PlayerCharacter>();

        playerController.SetPlayerCharacter(playerCharacter);

        _cinemachineCamera.Follow = playerCharacter.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
