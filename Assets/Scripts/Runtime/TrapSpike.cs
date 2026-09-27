using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private bool _isAutomatic = true;
    [SerializeField] private BoxCollider2D _triggerCollider;
    [SerializeField] private CircleCollider2D _detectionCollider;
    [SerializeField] private Animator _animator;

    private bool _isAttacking = false;
    private bool _isInAnimation = false;

    private void Awake()
    {
        if (_triggerCollider == null)
        {
            _triggerCollider = GetComponent<BoxCollider2D>();
        }
        if (_detectionCollider == null)
        {
            _detectionCollider = GetComponent<CircleCollider2D>();
        }
    }

    private void Start()
    {
        _animator.SetBool("IsAutomatic", _isAutomatic);
    }


    private void Update()
    {
        if (_isAttacking)
        {
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
                hit.collider.GetComponent<PlayerCharacter>().PlayerStateMachine.ChangeState(PlayerStateType.Death);
            }
        }

        if (!_isAutomatic)
        {
            // Calcul du vrai rayon (en prenant en compte le scaling X ou Y du GameObject)
            float currentScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            float realRadius = _detectionCollider.radius * currentScale;


            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position + (Vector3)_detectionCollider.offset,
                realRadius,
                _playerLayer
            );

            if (colliders.Length > 0 && !_isAttacking && !_isInAnimation)
            {
                _animator.SetTrigger("Attack");
            }
        }        
    }

    public void ActiveAttack()
    {
        _isAttacking = true;
    }

    public void DesactiveAttack()
    {
        _isAttacking = false;
    }

    public void ActiveIsInAnimation()
    {
        _isInAnimation = true;
    }

    public void DesactiveIsInAnimation()
    {
        _isInAnimation = false;
    }
}
