using UnityEngine;

public class RespawnCollisionCheck : CollisionCheck
{
    [SerializeField] private PhysicBody _body;
    [SerializeField] private PlayerCharacter _player;
    FramePhysicsData _data = new();
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body ||!_player)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;
        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);

        if (rayCount == 0)
            return;
        if (_player.PlayerStateMachine.CurrentState is not DeathState)
        {
            _player.PlayerStateMachine.CurrentState.SetNextState<DeathState>(true);
        }


    }
}
