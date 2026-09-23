using System;
using UnityEngine;
using UnityEngine.InputSystem;

//public class PlayerController : MonoBehaviour
//{
//    [SerializeField] private PlayerInput playerInput;
//    [SerializeField] private float moveSpeed;

//    private float _moveX;
//    private bool _isJumping;
//    void Start()
//    {
//        if(playerInput == null)
//        {
//            playerInput = GetComponent<PlayerInput>();
//        }
//    }

//    private void OnEnable()
//    {
//        playerInput.actions["Move"].performed += OnMove;
//        playerInput.actions["Move"].canceled += OnMove;


//        playerInput.actions["Jump"].performed += ctx => _isJumping = true;
//        playerInput.actions["Jump"].canceled += ctx => _isJumping = false;
//    }

//    private void OnDisable()
//    {
//        playerInput.actions["Move"].performed -= OnMove;
//        playerInput.actions["Move"].canceled -= OnMove;


//        playerInput.actions["Jump"].performed -= ctx => _isJumping = true;
//        playerInput.actions["Jump"].canceled -= ctx => _isJumping = false;
//    }

//    private void OnMove(InputAction.CallbackContext context)
//    {
//        Vector2 moveInput = context.ReadValue<Vector2>();
//        _moveX = moveInput.x * moveSpeed * Time.deltaTime;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    private void FixedUpdate()
//    {
//        if (_moveX != 0)
//        {
//            transform.Translate(_moveX, 0, 0);
//        }
//        if (_isJumping)
//        {
//            // Handle jump logic here
//            Debug.Log("Jumping...");
//        }
//    }
//}


#region VERSION PROF

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter _controlledCharacter;
    void Start()
    {

    }

    public void ReceiveMoveInput(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ReceiveMoveInput: {ctx.ReadValue<Vector2>()}");
        Vector2 moveInput = ctx.ReadValue<Vector2>();
        _controlledCharacter.Move(moveInput);
    }

    public void ReceiveJumpInput(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ReceiveJumpInput: {ctx.ReadValue<float>()}");
        // Handle jump input here

        if (ctx.started)
        {
            // Jump start logic here
            //Debug.Log("Jump started");
            _controlledCharacter.Jump();
        }

        if (ctx.canceled)
        {
            // Jump release logic here
            //Debug.Log("Jump canceled");
        }

        if (ctx.performed)
        {
            // Jump logic here
            //Debug.Log("Jump performed");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
    }

    public void SetPlayerCharacter(PlayerCharacter playerCharacter)
    {
        _controlledCharacter = playerCharacter;
    }
}

#endregion