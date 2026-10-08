using UnityEngine;
/// <summary>
/// Collision check responsible for detecting camera change triggers
/// and executing the closest valid camera transition.
/// </summary>
public class CameraCollisionCheck : CollisionCheck
{
    #region --- PHYSICS DATA ---

    /// <summary>
    /// Physics data used to process the camera body's current position and movement.
    /// </summary>
    FramePhysicsData _data = new();
    /// <summary>
    /// Physics body used as the source of the camera collision movement data.
    /// </summary>
    [SerializeField] private PhysicBody _body;

    #endregion

    #region --- COLLISION PROCESSING ---

    /// <summary>
    /// Processes the raycast results and executes the closest valid camera change.
    /// </summary>
    /// <param name="hits">
    /// Array containing the raycast results produced by the collision strategy.
    /// </param>
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
    #endregion
}
