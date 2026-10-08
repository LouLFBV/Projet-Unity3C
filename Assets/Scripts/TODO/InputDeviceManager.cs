using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputDeviceManager : SingletonMonoObject<InputDeviceManager>
{
    [Tooltip("Délai en secondes avant de réautoriser le passage à la souris après un mouvement manette.")]
    [SerializeField] private float _gamepadToKbMouseDelay = 1f;

    public ControlScheme CurrentScheme { get; private set; } = ControlScheme.KeyboardMouse;

    public static event Action<ControlScheme> OnControlSchemeChanged;

    private float _lastGamepadTime = -10f;

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

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

    private void SetControlScheme(ControlScheme newScheme)
    {
        if (CurrentScheme == newScheme) return;

        CurrentScheme = newScheme;

        OnControlSchemeChanged?.Invoke(CurrentScheme);
    }
}

public enum ControlScheme
{
    KeyboardMouse,
    Gamepad
}