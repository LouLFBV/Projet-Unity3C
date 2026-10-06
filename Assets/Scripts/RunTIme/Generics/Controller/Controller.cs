using System;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// Defines a generic controller associated with a <see cref="MonoBehaviour"/> type.
/// </summary>
/// <typeparam name="T">The type of <see cref="MonoBehaviour"/> controlled by this controller.</typeparam>
public class Controller<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// The controller port used to communicate with the controlled object.
    /// </summary>
    [SerializeField] protected ControllerPort<T> _controllerPort;

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
    public void SetController(ControllerPort<T> controllerPort)
    {
        _controllerPort = controllerPort;
    }
}
