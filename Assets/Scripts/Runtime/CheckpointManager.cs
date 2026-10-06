using UnityEngine;
using UnityEngine.TextCore.Text;

public class CheckpointManager : SingletonMonoObject<CheckpointManager>
{
    private Vector3 _currentSpawnPosition;
    private PhysicBody _body;
    private void Awake()
    {
        base.Awake();
        _currentSpawnPosition = Vector3.zero;
    }
    public void SetCheckpoint(Vector3 newPosition)
    {
      _currentSpawnPosition = newPosition;
    }
    public void SetBody(PhysicBody body)
    {
        _body = body;
    }

    public void RespawnPlayer()
    {
        if(!_body)
            return;

        _body.Tp(_currentSpawnPosition);
        //body.SetVelocity(Vector2.zero);
    }
    public void RespawnPlayer(Vector2 velocity)
    {
        if (!_body)
            return;

        _body.Tp(_currentSpawnPosition);
        _body.SetVelocity(velocity);
    }
}
