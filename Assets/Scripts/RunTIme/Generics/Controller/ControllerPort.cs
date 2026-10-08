using System;
using UnityEngine;

/// <summary>
/// Provides a port through which a <see cref="Controller{T}"/> can execute actions on a Unity component.
/// </summary>
/// <typeparam name="T">The type of <see cref="MonoBehaviour"/> managed by this port.</typeparam>
public class ControllerPort<T> : MonoBehaviour where T : MonoBehaviour
{
    #region --- MANAGED OBJECT ---

    /// <summary>
    /// The component managed by this port.
    /// </summary>
    protected T _object;
    /// <summary>
    /// Gets the component managed by this port.
    /// </summary>
    public T Object => _object;

    #endregion

    #region --- CONTROLLER SETTINGS ---

    /// <summary>
    /// The controller authorized to execute actions through this port.
    /// </summary>
    [Header("Properties")]
    [SerializeField] private Controller<T> _sender = null;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the managed component by retrieving it from the current GameObject
    /// when it has not already been assigned.
    /// </summary>
    virtual protected void Start()
    {
        if (!_object)
            _object = gameObject.GetComponent<T>();
       
        if (!_object)
            Debug.LogError("no object found please set it manualy");
    }
    #endregion

    #region --- PORT MANAGEMENT ---

    /// <summary>
    /// Assigns the controller authorized to use this port.
    /// </summary>
    /// <param name="newSender">The controller to associate with this port.</param>
    public void SetController(Controller<T> newSender)
    {
        _sender = newSender;
    }
    /// <summary>
    /// Assigns the component managed by this port.
    /// </summary>
    /// <param name="targetObject">The component to manage through this port.</param>
    public void SetObject(T targetObject)
    {
        _object = targetObject;
    }
    #endregion

    #region --- ACTION EXECUTION ---

    /// <summary>
    /// Executes an action on the managed component if the specified controller
    /// matches the controller assigned to this port.
    /// </summary>
    /// <param name="sender">The controller requesting the action.</param>
    /// <param name="action">The action to execute on the managed component.</param>
    virtual public void ExecuteAction(Controller<T> sender,Action<T> action)
   {
        if (!_object)
            return;

        if (_sender != sender)
            return;


        action(_object);
   }
    #endregion
}

