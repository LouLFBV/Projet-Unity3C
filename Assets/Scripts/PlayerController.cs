using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller<PlayerCharacter>
{
    [SerializeField] private PlayerInput _playerInput;

    public PlayerInput PlayerInput => _playerInput;

    private Vector2 _input;
    private bool _isSprinting;

    private void Awake()
    {
        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }
    protected override void Start()
    {
        _controllerPort.ExecuteAction(this, GivePlayerInput);
    }
    public void GivePlayerInput(PlayerCharacter character)
    {
        if (character != null)
        {
            character.InitializePlayerInputInPlayerUIManager(_playerInput);
        }
    }
    #region --- INPUT EVENTS ---
    private void OnEnable()
    {
        if (_playerInput == null) return;

        _playerInput.actions["Move"].performed += ReceiveMoveInput;
        _playerInput.actions["Move"].canceled += ReceiveMoveInput;

        _playerInput.actions["Jump"].performed += ReceiveJumpInput;

        _playerInput.actions["Sprint"].performed += ReceiveSprintInput;
        _playerInput.actions["Sprint"].canceled += ReceiveSprintInput;

        _playerInput.actions["TP"].performed += ReceiveTPInput;
        _playerInput.actions["TP"].canceled += ReceiveTPInput;

        _playerInput.actions["CancelTP"].performed += ReceiveCancelTP;

        _playerInput.actions["Menu"].performed += ReceiveMenuInput;
    }

    private void OnDisable()
    {
        if (_playerInput == null) return;

        _playerInput.actions["Move"].performed -= ReceiveMoveInput;
        _playerInput.actions["Move"].canceled -= ReceiveMoveInput;

        _playerInput.actions["Jump"].performed -= ReceiveJumpInput;

        _playerInput.actions["Sprint"].performed -= ReceiveSprintInput;
        _playerInput.actions["Sprint"].canceled -= ReceiveSprintInput;

        _playerInput.actions["TP"].performed -= ReceiveTPInput;
        _playerInput.actions["TP"].canceled -= ReceiveTPInput;

        _playerInput.actions["CancelTP"].performed -= ReceiveCancelTP;

        _playerInput.actions["Menu"].performed -= ReceiveMenuInput;
    }
    #endregion

    #region --- INPUT RECEIVERS ---
    public void ReceiveMoveInput(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>();
        _controllerPort.ExecuteAction(this, Move);
    }

    public void ReceiveJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, Jump);
        }
    }

    public void ReceiveSprintInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _isSprinting = true;
        else if (ctx.canceled) _isSprinting = false;

        _controllerPort.ExecuteAction(this, Sprint);
    }

    public void ReceiveTPInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, TPEnter);
        }
        else if (ctx.canceled)
        {
            _controllerPort.ExecuteAction(this, TPExit);
        }
    }

    public void ReceiveCancelTP(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, CancelTP);
        }
    }

    public void ReceiveMenuInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, Menu);
        }
    }
    #endregion

    #region --- ACTIONS DU PERSONNAGE ---

    private void Move(PlayerCharacter character) => character.Move(_input);
    private void Jump(PlayerCharacter character) => character.Jump();
    private void Sprint(PlayerCharacter character) => character.Sprint(_isSprinting);
    private void TPEnter(PlayerCharacter character) => character.TPEnter();
    private void TPExit(PlayerCharacter character) => character.TPExit();
    private void CancelTP(PlayerCharacter character) => character.CancelTP();
    private void Menu(PlayerCharacter character) => character.OpenCloseMenu();
    #endregion
}