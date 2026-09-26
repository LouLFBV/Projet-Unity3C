using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;
    public Animator AnimatorPlayer => _animator;

    [HideInInspector] public bool isTPing = false;

    public void SetMoveAnimation(float moveSpeed, float maxMoveSpeed)
    {
        _animator.SetFloat("Speed", Mathf.InverseLerp(0f, maxMoveSpeed, Mathf.Abs(moveSpeed)));
        if (Mathf.Abs(moveSpeed) > 0.01f)
        {
            _sprite.flipX = moveSpeed < 0;
        }
    }


    // Méthodes pour passer en état de téléportation et déclencher l'animation correspondante, mis en commentaire parce qu'en changeant d'état, TPState se faisait écrasze
    public void SetTPAnimation()
    {
        if (isTPing) return;
        isTPing = true;
        _animator.SetTrigger("TP");
    }
    //public void TPPlayer()
    //{
    //    _playerCharacter.PlayerStateMachine.PushState(PlayerStateType.TP);
    //}
}
