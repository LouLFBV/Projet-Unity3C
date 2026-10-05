using UnityEngine;

public class MovingPlatformCollisionCheck : CollisionCheck
{
    FramePhysicsData _data = new();
    [SerializeField] private PhysicBody _body;
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;

        int rayCount = Strategy.ProcessRayCast(_data, hits, Filter);

        if (rayCount == 0)
            return;

        float minDist = float.MaxValue;
        PhysicBody body = null;
        Vector2 minNormal = new();
        for (int i = 0; i < rayCount; ++i)
        {
            RaycastHit2D hit = hits[i];
            if(hit.distance < minDist )
            {
                PhysicBody currentBody = hit.collider.gameObject.GetComponent<PhysicBody>();
                if (currentBody == null)
                    continue;
                minDist = hit.distance;
                body = currentBody;
                minNormal = hit.normal;
            }
           
        }

        if (!body)
            return;
       

      
    }

}
