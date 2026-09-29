using UnityEngine;


/// <summary>
/// Base class for collision checks that perform raycast-based collision detection
/// and delegate the collision resolution to derived classes.
/// </summary>
public abstract class CollisionCheck : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// Number of rays used for the collision detection.
    /// </summary>
    [SerializeField,Range(1,100)] private uint _rayCount = 10;
    /// <summary>
    /// Strategy used to perform the collision detection.
    /// </summary>
    [SerializeField] private ColliderStrategy _strategy = null;
    /// <summary>
    /// Layer mask used to filter the colliders detected during collision checks.
    /// </summary>

    [SerializeField] private LayerMask _layer;
    private ContactFilter2D _filter = new ContactFilter2D();
    
    private RaycastHit2D[] _rayCasts = null;
    /// <summary>
    /// Gets the collider strategy used to process the raycasts.
    /// </summary>
    protected ColliderStrategy Strategy => _strategy;
    /// <summary>
    /// Gets the contact filter configured with the selected layer mask.
    /// </summary>
    protected ContactFilter2D Filter
    {
        get
        {
            _filter.useLayerMask = true;
            _filter.layerMask = _layer;
            return _filter;
        }
    }
    /// <summary>
    /// Gets the collider associated with the current collision strategy.
    /// </summary>
    protected Collider2D Collider => _strategy.Collider;
    /// <summary>
    /// Executes the collision check using the provided frame physics data.
    /// Initializes the raycast buffer when necessary, processes the raycasts
    /// through the configured strategy, and delegates the collision handling
    /// to the derived class.
    /// </summary>
    /// <param name="frameData">
    /// Physics data for the current frame. The data may be modified by the collision check.
    /// </param>
    public void ExecuteCollision(ref FramePhysicsData frameData) 
    {
        
        if (_rayCasts == null)
            _rayCasts = new RaycastHit2D[_rayCount];
        if (!_strategy)
            Debug.LogError("No strategy Set");
        else
        {
           int count = _strategy.ProcessRayCast(frameData, _rayCasts, Filter);
           ExecuteChildCollision(ref frameData, _rayCasts,count);
        }
    }
    /// <summary>
    /// Handles the collision results produced by the raycast processing.
    /// Must be implemented by derived collision checks.
    /// </summary>
    /// <param name="move">
    /// Physics data for the current frame. The data can be modified to resolve the collision.
    /// </param>
    /// <param name="rayCasts">
    /// Array containing the raycast results.
    /// </param>
    /// <param name="rayCount">
    /// Number of valid raycast results contained in the array.
    /// </param>
    abstract protected void ExecuteChildCollision(ref FramePhysicsData move, RaycastHit2D[] rayCasts,int rayCount);
}
