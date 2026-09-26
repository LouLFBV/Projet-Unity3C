using System.ComponentModel;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// Defines the different ways a force can be applied to a <see cref="PhysicBody"/>.
/// </summary>
public enum ForceType
{
    /// <summary>
    /// Applies an acceleration to the body by converting it to a force using the body's mass.
    /// </summary>
    Acceleration,
    /// <summary>
    /// Applies a force directly to the body.
    /// </summary>
    Force,
    /// <summary>
    /// Applies an instantaneous change to the body's velocity based on the applied force and mass.
    /// </summary>
    Impulse,
    /// <summary>
    /// Applies a direct change to the body's velocity.
    /// </summary>
    Velocity
}
/// <summary>
/// Represents a custom physics body that manages mass, velocity, position,
/// forces, gravity, friction, interpolation, and collision resolution.
/// </summary>

public class PhysicBody : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// Mass of the physics body in kilograms.
    /// A mass of zero makes the body static.
    /// </summary>
    [SerializeField, Range(0, float.MaxValue)] private float _massKg = 0.0f;
    /// <summary>
    /// Determines whether the body's rendered position is interpolated between physics updates.
    /// </summary>
    [SerializeField] private bool _enableInterpolation = true;

    /// <summary>
    /// Collision solver used to resolve the body's movement.
    /// </summary>
    [SerializeField] private CollisionSolver _solver;
    /// <summary>
    /// Determines whether collision solving is enabled.
    /// </summary>
    [SerializeField] private bool _enableSolver = true;
    /// <summary>
    /// Gravity object used to apply gravitational force to the body.
    /// </summary>
    [SerializeField] private GravityObject _gravity = null;
    /// <summary>
    /// Determines whether gravity is enabled.
    /// </summary>
    [SerializeField] private bool _enableGravity = true;
    /// <summary>
    /// Friction object used to apply frictional force to the body.
    /// </summary>
    [SerializeField] private FrictionObject _friction = null;

    /// <summary>
    /// Determines whether friction is enabled.
    /// </summary>
    [SerializeField] private bool _enableFriction = true;
    /// <summary>
    /// Sum of all forces currently accumulated on the body.
    /// </summary>

    private Vector2 _allForces = Vector2.zero;
    /// <summary>
    /// Current velocity of the body.
    /// </summary>
    private Vector2 _velocity = Vector2.zero;
    /// <summary>
    /// Position of the body during the previous physics update.
    /// </summary>
    private Vector2 _lastPosition = Vector2.zero;
    /// <summary>
    /// Current physics position of the body.
    /// </summary>
    private Vector2 _position = Vector2.zero;
    /// <summary>
    /// Cached inverse of the fixed physics timestep used for interpolation.
    /// </summary>

    private float _inverseFixedDeltaTime = 0.0f;



    /// <summary>
    /// Gets the mass of the body in kilograms.
    /// </summary>
    public float MassKg => _massKg;

    /// <summary>
    /// Gets whether position interpolation is enabled.
    /// </summary>
    public bool EnableInterpolation => _enableInterpolation;
    /// <summary>
    /// Gets whether collision solving is enabled.
    /// </summary>
    public bool EnableSolver => _enableSolver;
    /// <summary>
    /// Gets whether gravity is enabled.
    /// </summary>
    public bool EnableGravity => _enableGravity;
    /// <summary>
    /// Gets whether friction is enabled.
    /// </summary>
    public bool EnableFriction => _enableFriction;

    /// <summary>
    /// Gets the total force currently accumulated on the body.
    /// </summary>
    public Vector2 AllForces => _allForces;
    /// <summary>
    /// Gets the current velocity of the body.
    /// </summary>
    public Vector2 Velocity => _velocity;
    /// <summary>
    /// Gets the current physics position of the body.
    /// </summary>
    public Vector2 Position => _position;
    /// <summary>
    /// Gets the current physics position of the body.
    /// </summary>

    public bool IsStatic => _massKg == 0;
    /// <summary>
    /// Initializes the body's physics position and retrieves the required
    /// physics components when they are not explicitly assigned.
    /// </summary>

    private void Awake()
    {
        _position = transform.position;
        _lastPosition = transform.position;
        _inverseFixedDeltaTime = 1.0f / Time.fixedDeltaTime;
        if(_enableSolver && !_solver)
        {
            _solver = gameObject.GetComponent<CollisionSolver>();
            if (!_solver)
            { 
                Debug.LogError("no solver found set it manualy");
                _enableSolver = false;
            }

        }
        if (_enableGravity && !_gravity)
        {
            _gravity = gameObject.GetComponent<GravityObject>();
            if (!_gravity)
            {
                Debug.LogError("no solver found set it manualy");
                _enableGravity = false;
            }

        }
        if (_enableFriction && !_friction)
        {
            _friction = gameObject.GetComponent<FrictionObject>();
            if (!_friction)
            {
                Debug.LogError("no solver found set it manualy");
                _enableFriction = false;
            }
        }
    }
    /// <summary>
    /// Interpolates the rendered transform position between the previous
    /// and current physics positions when interpolation is enabled.
    /// </summary>
    private void Update()
    {
        if (!_enableInterpolation)
            return;

        float alpha = (Time.time - Time.deltaTime) * _inverseFixedDeltaTime;
        transform.position = (Vector3)Vector2.Lerp(_lastPosition, _position, alpha); 
    }
    /// <summary>
    /// Performs the physics update by applying gravity and friction,
    /// integrating forces into velocity, resolving collisions,
    /// and updating the body's physics position.
    /// </summary>
    private void FixedUpdate()
    {
        _lastPosition = _position;

        if (IsStatic)
            return;

        if (_gravity && _enableGravity)
        {
            this.AddForce(Vector2.down * _gravity.RealGravity, ForceType.Acceleration);
        }
        if (_friction && _enableFriction)
        {
            this.AddForce(_friction.DragCoefficient * Velocity.magnitude * Velocity.normalized, ForceType.Acceleration);
        }

        _velocity += (_allForces / _massKg) * Time.fixedDeltaTime;
        _allForces = Vector2.zero;
        Vector2 move = Vector2.zero;
        move = _velocity * Time.deltaTime;
        if (_enableSolver && _solver)
            _solver.Dispatch(ref move);
        _position += move;    
    }
    /// <summary>
    /// Sets the mass of the body.
    /// </summary>
    /// <param name="massKg">New mass in kilograms.</param>
    public void SetMass(float massKg)
    {
        _massKg = massKg;
    }
    /// <summary>
    /// Sets the current velocity of the body.
    /// </summary>
    /// <param name="velocity">New velocity vector.</param>
    public void SetVelocity( Vector2 velocity)
    {
        _velocity = velocity;
    }
    /// <summary>
    /// Sets the current physics position of the body.
    /// </summary>
    /// <param name="position">New physics position.</param>
    public void SetPosition(Vector2 position)
    {
        _position = position;
    }
    /// <summary>
    /// Enables or disables position interpolation.
    /// </summary>
    /// <param name="active">Whether interpolation should be enabled.</param>
    public void SetInterpolation(bool active)
    {
        _enableInterpolation = active;
    }
    /// <summary>
    /// Enables or disables collision solving and optionally assigns a collision solver.
    /// </summary>
    /// <param name="active">Whether collision solving should be enabled.</param>
    /// <param name="solver">Optional collision solver to assign.</param>
    public void SetSolver(bool active,CollisionSolver solver = null)
    {
        _enableSolver = active;
        if (solver)
            _solver = solver;
        else if (_enableSolver)
        {
            _solver = gameObject.GetComponent<CollisionSolver>();
            if (!_solver)
            {
                Debug.LogError("no solver found set it manualy");
                _enableSolver = false;
            }
        }

    }
    /// <summary>
    /// Enables or disables gravity and optionally assigns a gravity object.
    /// </summary>
    /// <param name="active">Whether gravity should be enabled.</param>
    /// <param name="gravity">Optional gravity object to assign.</param>
    public void SetGravity(bool active, GravityObject gravity = null)
    {
        _enableGravity = active;
        if (gravity)
            _gravity = gravity;
        else if (_enableGravity)
        {
            _gravity = gameObject.GetComponent<GravityObject>();
            if (!_gravity)
            {
                Debug.LogError("no solver found set it manualy");
                _enableGravity = false;
            }
        }

    }
    /// <summary>
    /// Enables or disables friction and optionally assigns a friction object.
    /// </summary>
    /// <param name="active">Whether friction should be enabled.</param>
    /// <param name="friction">Optional friction object to assign.</param>
    public void SetFriction(bool active, FrictionObject friction = null)
    {
        _enableFriction = active;
        if (friction)
            _friction = friction;
        else if (_enableFriction)
        {
            _friction = gameObject.GetComponent<FrictionObject>();
            if (!_gravity)
            {
                Debug.LogError("no solver found set it manualy");
                _enableFriction = false;
            }
        }

    }

    /// <summary>
    /// Clears all currently accumulated forces.
    /// </summary>
    public void ClearForces()
    {
        _allForces = Vector2.zero;
    }
    /// <summary>
    /// Applies a force to the body using the specified force type.
    /// </summary>
    /// <param name="force">Force, acceleration, impulse, or velocity to apply.</param>
    /// <param name="type">Type of force application to perform.</param>
    public void AddForce(Vector2 force,ForceType type)
    {
        switch (type)
        {
            case ForceType.Acceleration:
                _allForces += force * _massKg;
                break;
            case ForceType.Force:
                _allForces += force ;
                break;
            case ForceType.Impulse:
                _velocity += (force / _massKg);
                break;
            case ForceType.Velocity:
                _velocity += force;
                break;
        }
    }
}
