using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;

    //public bool IsTPing { get; set; } = false;

    #region Subscriptions

    private void OnEnable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt += HandleHurt;
        _playerCharacter.OnJump += HandleJump;
        _playerCharacter.OnTP += HandleTP;
        _playerCharacter.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        if (_playerCharacter == null) return;

        _playerCharacter.OnHurt -= HandleHurt;
        _playerCharacter.OnJump -= HandleJump;
        _playerCharacter.OnTP -= HandleTP;
        _playerCharacter.OnAttack -= HandleAttack;
    }
    #endregion

    private void Update()
    {
        if (_playerCharacter == null) return;

        UpdateMovementAnimation();
        UpdatePhysicsAnimation();
    }


    //public void SetMoveAnimation(float moveSpeedX, float maxMoveSpeed, float facingDirection)
    //{
    //    float speedRatio = Mathf.InverseLerp(            0f,            maxMoveSpeed,            Mathf.Abs(moveSpeedX)        );

    //    _animator.SetFloat(
    //        AnimatorHashes.Speed,
    //        speedRatio
    //    );

    //    if (facingDirection != 0)
    //    {
    //        _sprite.flipX = facingDirection < 0;
    //    }
    //}

    //public void SetJumpAnimation(float velocity)
    //{
    //    _animator.SetFloat(AnimatorHashes.JumpVelocity, velocity);
    //}
    private void UpdateMovementAnimation()
    {

        float speedRatio = Mathf.InverseLerp(0f, _playerCharacter.MaxMoveSpeed, Mathf.Abs(_playerCharacter.TargetAnimSpeed));
        _animator.SetFloat(AnimatorHashes.Speed, speedRatio);

        // 2. Orientation du Sprite
        if (_playerCharacter.FacingDirection != 0)
        {
            _sprite.flipX = _playerCharacter.FacingDirection < 0;
        }
    }

    //public void SetIsGrounded(bool isGrounded)
    //{
    //   _animator.SetBool(AnimatorHashes.IsGrounded, isGrounded);
    //}


    private void UpdatePhysicsAnimation()
    {
        _animator.SetBool(AnimatorHashes.IsGrounded, _playerCharacter.HitNormal.y != Vector2.zero.y);

        _animator.SetFloat(AnimatorHashes.JumpVelocity, _playerCharacter.Body.Velocity.y);
    }

    #region Handlers Events
    private void HandleHurt() => _animator.SetTrigger(AnimatorHashes.Hurt);
    private void HandleJump() => _animator.SetTrigger(AnimatorHashes.Jump);
    private void HandleAttack() => _animator.SetTrigger(AnimatorHashes.Attack);
    private void HandleTP()
    {
        //if (IsTPing) return;
        //IsTPing = true;
        //_animator.SetTrigger(AnimatorHashes.TP);
    }
    #endregion
}



//using UnityEngine;



//public class PlayerAnimator : MonoBehaviour

//{

//    [SerializeField] private PlayerCharacter _playerCharacter;



//    [SerializeField] private SpriteRenderer _sprite;

//    [SerializeField] private Animator _animator;



//    public bool IsTPing { get; set; } = false;



//    private void OnEnable()

//    {

//        _playerCharacter.OnHurt += () => SetTriggerPlayer(AnimatorHashes.Hurt);

//        _playerCharacter.OnJump += () => SetTriggerPlayer(AnimatorHashes.Jump);

//        _playerCharacter.OnTP += () => SetTPAnimation();

//    }



//    private void OnDisable()

//    {

//        _playerCharacter.OnHurt -= () => SetTriggerPlayer(AnimatorHashes.Hurt);

//        _playerCharacter.OnJump -= () => SetTriggerPlayer(AnimatorHashes.Jump);

//        _playerCharacter.OnTP -= () => SetTPAnimation();

//    }





//    public void SetMoveAnimation(float moveSpeedX, float maxMoveSpeed, float facingDirection)

//    {

//        float speedRatio = Mathf.InverseLerp(

//            0f,

//            maxMoveSpeed,

//            Mathf.Abs(moveSpeedX)

//        );



//        _animator.SetFloat(

//            AnimatorHashes.Speed,

//            speedRatio

//        );



//        if (facingDirection != 0)

//        {

//            _sprite.flipX = facingDirection < 0;

//        }

//    }



//    public void SetJumpAnimation(float velocity)

//    {

//        _animator.SetFloat(AnimatorHashes.JumpVelocity, velocity);

//    }



//    public void SetTriggerPlayer(int triggerName)

//    {

//        _animator.SetTrigger(triggerName);

//    }



//    public void SetIsGrounded(bool isGrounded)

//    {

//        _animator.SetBool(AnimatorHashes.IsGrounded, isGrounded);

//    }





//    // Méthodes pour passer en état de téléportation et déclencher l'animation correspondante, mis en commentaire parce qu'en changeant d'état, TPState se faisait écrasze

//    public void SetTPAnimation()

//    {

//        if (IsTPing) return;

//        IsTPing = true;

//        _animator.SetTrigger(AnimatorHashes.TP);

//    }

//    //public void TPPlayer()

//    //{

//    //    _playerCharacter.PlayerStateMachine.PushState(PlayerStateType.TP);

//    //}

//}

