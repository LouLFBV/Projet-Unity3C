using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private Vector3 _currentSpawnPosition;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetCheckpoint(Vector3 newPosition)
    {
        _currentSpawnPosition = newPosition;
    }

    public void RespawnPlayer(PlayerCharacter player)
    {
        player.transform.position = _currentSpawnPosition;
    }
}
