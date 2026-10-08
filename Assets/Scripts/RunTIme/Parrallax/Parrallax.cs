using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxMultiplier = new Vector2(0.5f, 0.5f);

    [Header("Références")]
    [SerializeField] private Camera _camera;

    private Vector3 _startPosition;
    private Vector3 _startCameraPosition;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        _startPosition = transform.position;
        _startCameraPosition = _camera.transform.position;
    }

    private void LateUpdate()
    {
        if (_camera == null) return;

        Vector3 cameraDelta = _camera.transform.position - _startCameraPosition;


        Vector3 targetPosition = new Vector3(
            _startPosition.x + (cameraDelta.x * _parallaxMultiplier.x),
            _startPosition.y + (cameraDelta.y * _parallaxMultiplier.y),
            _startPosition.z
        );

        transform.position = targetPosition;
    }
}