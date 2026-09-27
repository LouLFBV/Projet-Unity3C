using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private BoxCollider2D _triggerCollider;

    private bool _hasTriggered = false;

    private void Awake()
    {
        if (_triggerCollider == null)
            _triggerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_hasTriggered) return;

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + (Vector3)_triggerCollider.offset,
            _triggerCollider.size,
            0f,
            Vector2.zero,
            0f,
            _playerLayer
        );

        if (hit)
        {
            _hasTriggered = true;
            Debug.Log("Le joueur a franchi la ligne d'arrivée !");

            if (TimerManager.Instance != null)
            {
                TimerManager.Instance.StopTimer();
            }
        }
    }
}