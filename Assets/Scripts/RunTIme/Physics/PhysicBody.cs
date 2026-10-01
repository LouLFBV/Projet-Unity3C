using System;
using System.Collections.Generic;
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
    /// The predefined celestial body used to determine the gravitational acceleration.
    /// </summary>
    [SerializeField] private GravityPreset _gravity = GravityPreset.Earth;
    /// <summary>
    /// A custom gravitational acceleration value.
    /// A value of <c>0.0f</c> causes the selected <see cref="GravityPreset"/> to be used instead.
    /// </summary>
    [SerializeField] private float _customGravity = 0.0f;
    /// <summary>
    /// Gets the effective gravitational acceleration.
    /// Uses the custom value when it is different from zero; otherwise, uses the selected preset.
    /// </summary>
    private float _realGravity => _customGravity == 0.0f ? GravityObject.GetGrav(_gravity) : _customGravity;
    /// <summary>
    /// Determines whether gravity is enabled.
    /// </summary>
    [SerializeField] private bool _enableGravity = true;
    /// <summary>
    /// The magnitude of the drag coefficient applied by this object.
    /// </summary>
    [SerializeField, Range(0, float.MaxValue)] private float _dragCoefficient = 10;

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
    [SerializeField]
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
    /// Initializes the body's physics state and retrieves the collision solver
    /// from the current GameObject when collision solving is enabled and no solver is assigned.
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
       
       
    }
    /// <summary>
    /// Interpolates the rendered transform position between the previous
    /// and current physics positions when interpolation is enabled.
    /// </summary>
    private void Update()
    {
        if (!_enableInterpolation)
        {
            transform.position = _position; 
        }
        else
        {
            float alpha = (Time.time - Time.fixedTime) * _inverseFixedDeltaTime;
            transform.position = (Vector3)Vector2.Lerp(_lastPosition, _position, alpha);
        }

        
    
    }
    /// <summary>
    /// Performs the physics update by applying gravity and friction as accelerations,
    /// integrating accumulated forces into velocity, resolving collisions,
    /// and updating the body's physics position.
    /// </summary>
    private void FixedUpdate()
    {
        _lastPosition = _position;

        if (IsStatic)
            return;

        if (_enableGravity)
        {
            this.AddForce(Vector2.down * _realGravity, ForceType.Acceleration);
        }
        if (_enableFriction)
        {
            this.AddForce(-_dragCoefficient * Velocity.magnitude * Velocity.normalized, ForceType.Acceleration);
        }
        _velocity += (_allForces / _massKg) * Time.fixedDeltaTime;
        _allForces = Vector2.zero;
        if (_enableSolver && _solver)
            _solver.Dispatch();
        
        _position += _velocity * Time.deltaTime;    
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
    /// Enables or disables gravity and sets the gravitational acceleration using a predefined preset.
    /// </summary>
    /// <param name="active">Whether gravity should be enabled.</param>
    /// <param name="presept">The predefined gravity preset to use.</param>
    public void SetGravity(bool active, GravityPreset presept)
    {
        _enableGravity = active;
        _gravity = presept;
    }
    /// <summary>   
    /// Enables or disables gravity and sets a custom gravitational acceleration.
    /// </summary>
    /// <param name="active">Whether gravity should be enabled.</param>
    /// <param name="customGravity">The custom gravitational acceleration to use.</param>   
    public void SetGravity(bool active, float customGravity)
    {
        _enableGravity = active;
        _customGravity = customGravity;
    }
    /// <summary>
    /// Enables or disables friction and sets the drag coefficient used to calculate friction.
    /// </summary>
    /// <param name="active">Whether friction should be enabled.</param>
    /// <param name="dragCoeficient">The drag coefficient used for friction calculations.</param>
    public void SetFriction(bool active, float dragCoeficient)
    {
        _enableFriction = active;
        _dragCoefficient = dragCoeficient;

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
