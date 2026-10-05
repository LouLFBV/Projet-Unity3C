using Unity.Cinemachine;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _brain;
    [SerializeField] private CinemachineCamera _current; 
    [SerializeField] private CinemachineCamera _target;

    public void ExecuteChange()
    {
       if(!IsReady())
            return;

        _current.Priority.Value = 0;
         _target.Priority.Value = 1;
    }
    public bool IsReady()
    {
        if (_brain == null || _current == null || _target == null)
            return false;
        CinemachineCamera activeCamera = _brain.ActiveVirtualCamera as CinemachineCamera;
        if (activeCamera != _current)
            return false;
        return true;
    }

}
