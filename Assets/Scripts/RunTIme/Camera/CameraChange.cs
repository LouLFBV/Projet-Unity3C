using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// Manages transitions between Cinemachine cameras by changing their priorities.
/// </summary>
public class CameraChange : MonoBehaviour
{
    #region --- CAMERA REFERENCES ---

    /// <summary>
    /// Cinemachine brain responsible for managing the active virtual camera.
    /// </summary>
    [SerializeField] private CinemachineBrain _brain;
    /// <summary>
    /// Currently active Cinemachine camera that can be replaced by the target camera.
    /// </summary>
    [SerializeField] private CinemachineCamera _current;
    /// <summary>
    /// Target Cinemachine camera that will become active after the transition.
    /// </summary>
    [SerializeField] private CinemachineCamera _target;
    #endregion

    #region --- CAMERA MANAGEMENT ---

    /// <summary>
    /// Executes the camera change by lowering the current camera priority
    /// and increasing the target camera priority.
    /// </summary>

    public void ExecuteChange()
    {
       if(!IsReady())
            return;

       _current.Priority.Value = 0;
       _target.Priority.Value = 1;
    }
    /// <summary>
    /// Determines whether the camera change can be executed.
    /// Checks that all required camera references are assigned and that
    /// the expected current camera is the active virtual camera.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the camera change is ready to be executed; otherwise, <c>false</c>.
    /// </returns>
    public bool IsReady()
    {
        if (_brain == null || _current == null || _target == null)
            return false;
        CinemachineCamera activeCamera = _brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCamera != _current)
            return false;
        return true;
    }
    #endregion
}
