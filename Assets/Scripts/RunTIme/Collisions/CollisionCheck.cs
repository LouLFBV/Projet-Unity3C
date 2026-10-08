using UnityEngine;


/// <summary>
/// Base class for collision checks that perform raycast-based collision detection
/// and delegate the collision resolution to derived classes.
/// </summary>
public abstract class CollisionCheck : MonoBehaviour
{
    #region --- PROPERTIES ---

    /// <summary>
    /// Number of rays used for the collision detection.
    /// </summary>
    [Header("Properties")]
    [SerializeField,Range(1,100)] private uint _rayCount = 10;
    /// <summary>
    /// Strategy used to perform the collision detection.
    /// </summary>
    [SerializeField] private ColliderStrategy _strategy = null;
    /// <summary>
    /// Layer mask used to filter the colliders detected during collision checks.
    /// </summary>
    [SerializeField] private LayerMask _layer;
    /// <summary>
    /// Contact filter configured for the collision detection.
    /// </summary>
    private ContactFilter2D _filter = new ContactFilter2D();
    /// <summary>
    /// Array used to store the raycast results.
    /// </summary>
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
    #endregion

    #region --- COLLISION EXECUTION ---

    /// <summary>
    /// Executes the collision check using the configured collision strategy.
    /// Initializes the raycast buffer when necessary, processes the collision
    /// and delegates the collision handling to the derived class.
    /// </summary>
    public void ExecuteCollision() 
    {
        
        if (_rayCasts == null)
            _rayCasts = new RaycastHit2D[_rayCount];
        if (!_strategy)
            Debug.LogError("No strategy Set");
        else
        {
           ExecuteChildCollision(_rayCasts);
        }
    }
    /// <summary>
    /// Handles the collision results produced by the raycast processing.
    /// Must be implemented by derived collision checks.
    /// </summary>
    /// <param name="rayCasts">
    /// Array containing the raycast results.
    /// </param>
    abstract protected void ExecuteChildCollision(RaycastHit2D[] rayCasts);
    #endregion
}
