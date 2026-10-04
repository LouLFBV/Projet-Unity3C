using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _playerLayer;

    private bool _isAlreadyActivated = false;
    void Start()
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider2D>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAlreadyActivated)
        {
            CheckPlayer();
        }
    }

    private void CheckPlayer()
    {
        RaycastHit2D collider = Physics2D.BoxCast(
            transform.position + (Vector3)_collider.offset,
            _collider.size,
            0f,
            Vector2.zero,
            0f,
            _playerLayer
            );

        if (collider && collider.collider.CompareTag("Player"))
        {
            GiveHisPosition();
            _isAlreadyActivated = true;
            _animator.SetTrigger("Break");
            collider.collider.GetComponentInChildren<PlayerCharacter>().TriggerAttack();
        }
    }

    public void GiveHisPosition()
    {
        CheckpointManager.Instance.SetCheckpoint(transform.position);
        Debug.Log("Checkpoint activated at position: " + transform.position);
    }
}
