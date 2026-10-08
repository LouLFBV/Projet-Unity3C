using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Handles player input and forwards input-driven actions to the associated
/// <see cref="PlayerCharacter"/>.
/// </summary>
public class PlayerController : Controller<PlayerCharacter>
{
    /// <summary>
    /// Input component used to retrieve and subscribe to the player's input actions.
    /// </summary>
    [SerializeField] private PlayerInput _playerInput;
    /// <summary>
    /// Gets the <see cref="PlayerInput"/> component used by this controller.
    /// </summary>
    public PlayerInput PlayerInput => _playerInput;
    /// <summary>
    /// Current movement input received from the player.
    /// </summary>
    private Vector2 _input;
    /// <summary>
    /// Indicates whether the player is currently sprinting.
    /// </summary>
    private bool _isSprinting;
    /// <summary>
    /// Initializes the player input component reference.
    /// If no reference has been assigned in the Inspector, the component is retrieved
    /// from the same GameObject.
    /// </summary>
    private void Awake()
    {
        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }
    /// <summary>
    /// Initializes the controller and provides the player's input component
    /// to the associated <see cref="PlayerCharacter"/>.
    /// </summary>
    protected override void Start()
    {
        _controllerPort.ExecuteAction(this, GivePlayerInput);
    }
    /// <summary>
    /// Provides the player's input component to the associated character.
    /// </summary>
    /// <param name="character">The player character receiving the input component.</param>
    public void GivePlayerInput(PlayerCharacter character)
    {
        if (character != null)
        {
            character.InitializePlayerInputInPlayerUIManager(_playerInput);
        }
    }

    #region --- INPUT EVENTS ---

    /// <summary>
    /// Subscribes to the configured player input actions when the controller is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null || _playerInput.actions == null) return;

        ToggleSubscriptions( true);
    }
    /// <summary>
    /// Unsubscribes from the configured player input actions when the controller is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null || _playerInput.actions == null) return;

        ToggleSubscriptions( false);
    }
    /// <summary>
    /// Enables or disables subscriptions to the configured input actions.
    /// </summary>
    /// <param name="subscribe">
    /// <c>true</c> to subscribe to the input actions; <c>false</c> to unsubscribe.
    /// </param>
    private void ToggleSubscriptions(bool subscribe)
    {
        BindAction("Move", ReceiveMoveInput, subscribe,true);
        BindAction("Jump", ReceiveJumpInput, subscribe);
        BindAction("Sprint", ReceiveSprintInput, subscribe, true);
        BindAction("TP", ReceiveTPInput, subscribe, true);
        BindAction("CancelTP", ReceiveCancelTP, subscribe);

        // Actions Menu dans les 2 maps
        BindAction("Player/Menu", ReceiveMenuInput, subscribe);
        BindAction("UI/Menu", ReceiveMenuInput, subscribe);
    }
    /// <summary>
    /// Binds or unbinds a callback to an input action.
    /// </summary>
    /// <param name="actionNameOrPath">Name or path of the input action to bind.</param>
    /// <param name="callback">Callback invoked when the input action is triggered.</param>
    /// <param name="subscribe">
    /// <c>true</c> to subscribe the callback; <c>false</c> to unsubscribe it.
    /// </param>
    /// <param name="includeCanceled">
    /// <c>true</c> to also handle the canceled state of the input action.
    /// </param>
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

    /// <summary>
    /// Receives and stores the player's movement input.
    /// </summary>
    /// <param name="ctx">Input callback context containing the movement input value.</param>
    public void ReceiveMoveInput(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>();
        _controllerPort.ExecuteAction(this, Move);
    }
    /// <summary>
    /// Receives the player's jump input and triggers the jump action when performed.
    /// </summary>
    /// <param name="ctx">Input callback context for the jump action.</param>
    public void ReceiveJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, Jump);
        }
    }
    /// <summary>
    /// Receives the player's sprint input and updates the current sprint state.
    /// </summary>
    /// <param name="ctx">Input callback context for the sprint action.</param>
    public void ReceiveSprintInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _isSprinting = true;
        else if (ctx.canceled) _isSprinting = false;

        _controllerPort.ExecuteAction(this, Sprint);
    }

    /// <summary>
    /// Receives the player's teleport input and handles entering or exiting teleport mode.
    /// </summary>
    /// <param name="ctx">Input callback context for the teleport action.</param>
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
    /// <summary>
    /// Receives the input used to cancel the current teleport action.
    /// </summary>
    /// <param name="ctx">Input callback context for the cancel teleport action.</param>
    public void ReceiveCancelTP(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, CancelTP);
        }
    }
    /// <summary>
    /// Receives the menu input and toggles the player menu when performed.
    /// </summary>
    /// <param name="ctx">Input callback context for the menu action.</param>
    public void ReceiveMenuInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _controllerPort.ExecuteAction(this, Menu);
        }
    }
    #endregion

    #region --- CHARACTER ACTIONS ---

    /// <summary>
    /// Forwards the current movement input to the player character.
    /// </summary>
    /// <param name="character">Player character receiving the movement command.</param>
    private void Move(PlayerCharacter character) => character.Move(_input);
    /// <summary>
    /// Forwards the jump command to the player character.
    /// </summary>
    /// <param name="character">Player character receiving the jump command.</param>
    private void Jump(PlayerCharacter character) => character.Jump();
    /// <summary>
    /// Forwards the current sprint state to the player character.
    /// </summary>
    /// <param name="character">Player character receiving the sprint command.</param>
    private void Sprint(PlayerCharacter character) => character.Sprint(_isSprinting);
    /// <summary>
    /// Instructs the player character to enter teleport mode.
    /// </summary>
    /// <param name="character">Player character receiving the teleport command.</param>
    private void TPEnter(PlayerCharacter character) => character.TPEnter();
    /// <summary>
    /// Instructs the player character to exit teleport mode.
    /// </summary>
    /// <param name="character">Player character receiving the teleport command.</param>
    private void TPExit(PlayerCharacter character) => character.TPExit();
    /// <summary>
    /// Instructs the player character to cancel the current teleport action.
    /// </summary>
    /// <param name="character">Player character receiving the cancel teleport command.</param>
    private void CancelTP(PlayerCharacter character) => character.CancelTP();

    /// <summary>
    /// Toggles the player menu.
    /// </summary>
    /// <param name="character">Player character receiving the menu command.</param>
    private void Menu(PlayerCharacter character) => character.OpenCloseMenu();
    #endregion
}