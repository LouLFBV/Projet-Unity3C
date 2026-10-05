using UnityEngine;

public class CameraCollisionCheck : CollisionCheck
{
    FramePhysicsData _data = new();
    [SerializeField] private PhysicBody _body;
    protected override void ExecuteChildCollision(RaycastHit2D[] hits)
    {
        if (!_body)
            return;
        _data.Pos = _body.Position;
        _data.Move = _body.Velocity * Time.fixedDeltaTime;

        int rayCount = Strategy.ProcessRayCast(_data, hits,Filter);

        if(rayCount == 0)
            return;

        float minDist = float.MaxValue;
        CameraChange camera = null;

        for(int i = 0; i < rayCount; ++i)
        {
            RaycastHit2D hit = hits[i];
            
            if (hit.distance < minDist )
            {
                CameraChange currentCamera = hit.collider.gameObject.GetComponent<CameraChange>();
                if (currentCamera == null || !currentCamera.IsReady())
                    continue;
                    
                minDist = hit.distance;
                camera = currentCamera;
            }
        }
        if (!camera)
            return;
        camera.ExecuteChange();
    }
}
