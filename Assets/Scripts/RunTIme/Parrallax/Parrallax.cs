using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Réglages Parallax (0 = suit la caméra, 1 = fixe au monde)")]
    [SerializeField] private Vector2 _parallaxMultiplier = new Vector2(0.5f, 0.5f);

    [Header("Infini")]
    [SerializeField] private bool _isInfiniteHorizontal;
    [SerializeField] private bool _isInfiniteVertical;

    [Header("Références")]
    [SerializeField] private Camera _camera;

    private Vector3 _startPosition;
    private Vector3 _startCameraPosition;
    private Vector2 _textureSize;

    private void Start()
    {
        if (_camera == null) _camera = Camera.main;

        _startPosition = transform.position;
        _startCameraPosition = _camera.transform.position;

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            _textureSize = spriteRenderer.bounds.size;
        }
    }

    private void LateUpdate()
    {
        if (_camera == null) return;

        Vector3 cameraDelta = _camera.transform.position - _startCameraPosition;

        // Calcul du déplacement (on conserve le Z d'origine intact)
        Vector3 targetPosition = new Vector3(
            _startPosition.x + (cameraDelta.x * _parallaxMultiplier.x),
            _startPosition.y + (cameraDelta.y * _parallaxMultiplier.y),
            _startPosition.z
        );

        transform.position = targetPosition;

        // Repositionnement infini Horizontal
        if (_isInfiniteHorizontal && _textureSize.x > 0)
        {
            float relativeCamX = _camera.transform.position.x * (1f - _parallaxMultiplier.x);
            float diffX = relativeCamX - _startPosition.x;

            if (Mathf.Abs(diffX) >= _textureSize.x)
            {
                _startPosition.x += Mathf.Sign(diffX) * _textureSize.x;
            }
        }

        // Repositionnement infini Vertical
        if (_isInfiniteVertical && _textureSize.y > 0)
        {
            float relativeCamY = _camera.transform.position.y * (1f - _parallaxMultiplier.y);
            float diffY = relativeCamY - _startPosition.y;

            if (Mathf.Abs(diffY) >= _textureSize.y)
            {
                _startPosition.y += Mathf.Sign(diffY) * _textureSize.y;
            }
        }
    }
}