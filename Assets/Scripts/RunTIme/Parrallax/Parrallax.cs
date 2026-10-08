using UnityEngine;
/// <summary>
/// Controls the parallax effect of a GameObject based on the movement of a camera.
/// </summary>
public class Parallax : MonoBehaviour
{
    #region --- PARALLAX SETTINGS ---

    /// <summary>
    /// Multiplier applied to the camera movement to determine the parallax movement.
    /// A value of <c>1.0</c> makes the object follow the camera movement,
    /// while a value of <c>0.0</c> keeps the object stationary relative to its starting position.
    /// </summary>
    [SerializeField] private Vector2 _parallaxMultiplier = new Vector2(0.5f, 0.5f);
    #endregion

    #region --- REFERENCES ---

    /// <summary>
    /// Camera used as the reference for calculating the parallax movement.
    /// If no camera is assigned, the main camera is automatically retrieved.
    /// </summary>
    [Header("Références")]
    [SerializeField] private Camera _camera;
    #endregion

    #region --- INITIAL STATE ---

    /// <summary>
    /// World position of the object when the parallax effect is initialized.
    /// </summary>
    private Vector3 _startPosition;
    /// <summary>
    /// World position of the camera when the parallax effect is initialized.
    /// </summary>
    private Vector3 _startCameraPosition;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the parallax effect by retrieving the main camera when necessary
    /// and storing the initial positions of the object and camera.
    /// </summary>
    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        _startPosition = transform.position;
        _startCameraPosition = _camera.transform.position;
    }
    /// <summary>
    /// Updates the object's position according to the camera movement
    /// and the configured parallax multipliers.
    /// </summary>
    private void LateUpdate()
    {
        if (!_camera) return;

        Vector3 cameraDelta = _camera.transform.position - _startCameraPosition;


        Vector3 targetPosition = new Vector3(
            _startPosition.x + (cameraDelta.x * _parallaxMultiplier.x),
            _startPosition.y + (cameraDelta.y * _parallaxMultiplier.y),
            _startPosition.z
        );

        transform.position = targetPosition;
    }
    #endregion
}