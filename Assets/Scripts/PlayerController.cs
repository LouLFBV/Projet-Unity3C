using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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

public class PlayerController : Controller<PlayerCharacter>
{
    //private PlayerCharacter _controlledCharacter;

    private Vector2 _input;
    private float _jump;
    public void ReceiveMoveInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log($"ReceiveMoveInput: {ctx.ReadValue<Vector2>()}");
         _input = ctx.ReadValue<Vector2>();
       


        _controllerPort.ExecuteAction(this, Move);
    }

    public void ReceiveJumpInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log($"ReceiveJumpInput: {ctx.ReadValue<float>()}");
        // Handle jump input here

        if (ctx.started)
        {
            // Jump start logic here
            //Debug.Log("Jump started");
            _controllerPort.ExecuteAction(this, Jump);


            _jump = ctx.ReadValue<float>();
            //_controllerPort.ExecuteAction(this, Jump);
        }
    }
    private void Jump(PlayerCharacter character)
    {
        character.Jump();
    }   
    bool _isSprinting = false;
    public void ReceiveSprintInput(InputAction.CallbackContext ctx)
    {
        // Handle sprint input here
        Debug.Log("Sprint input received");
        if (ctx.started)
        {
            _isSprinting = true;
            _controllerPort.ExecuteAction(this, Sprint);

        }

        if (ctx.canceled)
        {
            _isSprinting = false;
            _controllerPort.ExecuteAction(this, Sprint);

        }
    }
    private void Sprint(PlayerCharacter character)
    {
        character.Sprint(_isSprinting);
    }

    public void ReceiveMenuInput(InputAction.CallbackContext ctx)
    {
        // Handle menu input here
        Debug.Log("Menu input received");
        if (ctx.started)
        {
            // Open menu logic here
            Debug.Log("Menu opened");
        }
    }

    public void ReceiveTPInput(InputAction.CallbackContext ctx)
    {
        // Handle teleport input here
        Debug.Log("Teleport input received");
        if (ctx.started)
        {
            // Teleport logic here
            Debug.Log("Teleporting...");
            _controllerPort.ExecuteAction(this, TPEnter);

        }
        if (ctx.canceled)
        {
            _controllerPort.ExecuteAction(this, TPExit);
        }
    }
    private void TPEnter(PlayerCharacter character)
    {
        character.TPEnter();
    }
    private void TPExit(PlayerCharacter character)
    {
        character.TPExit();
    }
    public void ReceiveCancelTP(InputAction.CallbackContext ctx)
    {
        // Handle cancel teleport input here
        Debug.Log("Cancel Teleport input received");
        if (ctx.started)
        {
            // Cancel teleport logic here
            Debug.Log("Canceling Teleport...");
            _controllerPort.ExecuteAction(this, CancelTP);

        }
    }
    private void CancelTP(PlayerCharacter character)
    {
        character.CancelTP();
    }

    private void Move(PlayerCharacter movement)
    {
        movement.Move(_input);
    }
}

#endregion