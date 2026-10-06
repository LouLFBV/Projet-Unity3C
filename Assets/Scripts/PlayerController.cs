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
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null || _playerInput.actions == null) return;

        ToggleSubscriptions(subscribe: true);
    }

    private void OnDisable()
    {
        if (_playerInput == null || _playerInput.actions == null) return;

        ToggleSubscriptions(subscribe: false);
    }

    private void ToggleSubscriptions(bool subscribe)
    {
        BindAction("Move", ReceiveMoveInput, subscribe, includeCanceled: true);
        BindAction("Jump", ReceiveJumpInput, subscribe);
        BindAction("Sprint", ReceiveSprintInput, subscribe, includeCanceled: true);
        BindAction("TP", ReceiveTPInput, subscribe, includeCanceled: true);
        BindAction("CancelTP", ReceiveCancelTP, subscribe);

        // Actions Menu dans les 2 maps
        BindAction("Player/Menu", ReceiveMenuInput, subscribe);
        BindAction("UI/Menu", ReceiveMenuInput, subscribe);
    }

    private void BindAction(string actionNameOrPath, System.Action<InputAction.CallbackContext> callback, bool subscribe, bool includeCanceled = false)
    {
        InputAction action = _playerInput.actions.FindAction(actionNameOrPath);
        if (action == null) return;

        if (subscribe)
        {
            action.performed += callback;
            if (includeCanceled) action.canceled += callback;
        }
        else
        {
            action.performed -= callback;
            if (includeCanceled) action.canceled -= callback;
        }
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