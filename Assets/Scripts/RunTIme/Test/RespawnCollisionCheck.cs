using UnityEngine;

public class RespawnCollisionCheck : CollisionCheck
{
    [SerializeField] private PhysicBody _body;
    FramePhysicsData _data = new();
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);

        if (rayCount == 0)
            return;

        CheckpointManager.Instance.RespawnPlayer(Vector2.zero);

    }
}
