using UnityEngine;

public class FlagCollisionCheck : CollisionCheck
{
    private FramePhysicsData _data = new();
    [SerializeField] private PhysicBody _body;
    [SerializeField] private PlayerCharacter _player;
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body || !_player)
            return;
        _data.Pos = _body.Position;
        _data.Move = Vector2.zero;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);
        if (rayCount == 0)
            return;

        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit2D hit = hits[i];
            hit.collider.gameObject.GetComponent<FlagTrigger>()?.TrigerFlag();
        }
    }
}
