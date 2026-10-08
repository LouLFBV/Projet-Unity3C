using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
/// <summary>
/// Manages the currently active input device and control scheme.
/// Detects input activity from keyboard, mouse and gamepad devices
/// and notifies listeners when the active control scheme changes.
/// </summary>
public class InputDeviceManager : SingletonMonoObject<InputDeviceManager>
{
    #region --- INPUT SETTINGS ---

    /// <summary>
    /// Delay in seconds before allowing a switch back to keyboard and mouse
    /// after gamepad input has been detected.
    /// </summary>
    [Tooltip("Délai en secondes avant de réautoriser le passage à la souris après un mouvement manette.")]
    [SerializeField] private float _gamepadToKbMouseDelay = 1f;
    #endregion

    #region --- INPUT STATE ---

    /// <summary>
    /// Gets the control scheme currently detected as active.
    /// </summary>
    public ControlScheme CurrentScheme { get; private set; } = ControlScheme.KeyboardMouse;
    /// <summary>
    /// Event invoked when the active control scheme changes.
    /// </summary>
    public static event Action<ControlScheme> OnControlSchemeChanged;
    /// <summary>
    /// Time at which the last gamepad input was detected.
    /// </summary>
    private float _lastGamepadTime = -10f;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Subscribes to Unity Input System action change notifications.
    /// </summary>
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }
    /// <summary>
    /// Unsubscribes from Unity Input System action change notifications.
    /// </summary>
    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }
    #endregion

    #region --- INPUT DETECTION ---

    /// <summary>
    /// Processes input action changes and determines the device responsible
    /// for the performed action.
    /// </summary>
    /// <param name="obj">
    /// Object associated with the input action change.
    /// </param>
    /// <param name="change">
    /// Type of input action change detected.
    /// </param>
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        InputAction action = (InputAction)obj;
        InputControl control = action.activeControl;

        if (control == null) return;

        InputDevice device = control.device;

        if (device is Gamepad)
        {
            _lastGamepadTime = Time.unscaledTime;
            SetControlScheme(ControlScheme.Gamepad);
        }
        else if (device is Keyboard)
        {
            SetControlScheme(ControlScheme.KeyboardMouse);
        }
        else if (device is Mouse)
        {
            // Vérifie si l'action est un vrai clic de souris
            bool isClick = control is ButtonControl;
            bool delayPassed = (Time.unscaledTime - _lastGamepadTime) > _gamepadToKbMouseDelay;

            // On ne repasse au Clavier/Souris que si le délai est écoulé OU si le joueur clique physiquement
            if (delayPassed || isClick)
            {
                SetControlScheme(ControlScheme.KeyboardMouse);
            }
        }
    }
    #endregion

    #region --- CONTROL SCHEME MANAGEMENT ---

    /// <summary>
    /// Updates the current control scheme and notifies listeners when it changes.
    /// </summary>
    /// <param name="newScheme">
    /// New control scheme to set as active.
    /// </param>
    private void SetControlScheme(ControlScheme newScheme)
    {
        if (CurrentScheme == newScheme) return;

        CurrentScheme = newScheme;

        OnControlSchemeChanged?.Invoke(CurrentScheme);
    }
    #endregion
}
/// <summary>
/// Defines the available input control schemes.
/// </summary>
public enum ControlScheme
{
    KeyboardMouse,
    Gamepad
}