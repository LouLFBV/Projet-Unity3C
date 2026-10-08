using System;
using System.Xml.Linq;
using UnityEngine;
/// <summary>
/// Defines a generic controller associated with a <see cref="MonoBehaviour"/> type.
/// </summary>
/// <typeparam name="T">The type of <see cref="MonoBehaviour"/> controlled by this controller.</typeparam>
public class Controller<T> : MonoBehaviour where T : MonoBehaviour
{
    #region --- CONTROLLER SETTINGS ---


    /// <summary>
    /// The controller port used to communicate with the controlled object.
    /// </summary>
    [Header("Properties")]
    [SerializeField] protected ControllerPort<T> _controllerPort;

    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the controller port by retrieving it from the current GameObject
    /// when it has not already been assigned.
    /// </summary>
    virtual protected void Start()
    {
        if (!_controllerPort)
            _controllerPort = gameObject.GetComponent<ControllerPort<T>>();
        if (!_controllerPort)
            Debug.LogError("no controllerPort<T> found please set it manualy");
    }
    #endregion

    #region --- CONTROLLER MANAGEMENT ---

    /// <summary>
    /// Assigns the controller port used by this controller to communicate
    /// with the controlled object.
    /// </summary>
    /// <param name="controllerPort">The controller port to associate with this controller.</param>
    public void SetController(ControllerPort<T> controllerPort)
    {
        _controllerPort = controllerPort;
    }
    #endregion
}
