using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Controls the mouse cursor position using the right stick of a gamepad.
/// </summary>
public class GamepadMouseController : MonoBehaviour
{
    #region --- CURSOR SETTINGS ---

    /// <summary>
    /// Speed at which the gamepad moves the mouse cursor.
    /// </summary>
    [SerializeField] private float _cursorSpeed = 1000f;
    /// <summary>
    /// Input action used to read the gamepad right stick value.
    /// </summary>
    [SerializeField] private InputActionProperty _rightStickAction;   
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Enables the right stick input action when the component is enabled.
    /// </summary>
    private void OnEnable() => _rightStickAction.action?.Enable();
    /// <summary>
    /// Disables the right stick input action when the component is disabled.
    /// </summary>
    private void OnDisable() => _rightStickAction.action?.Disable();
    /// <summary>
    /// Updates the mouse cursor position according to the current gamepad right stick input.
    /// </summary>
    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 stickValue = _rightStickAction.action.ReadValue<Vector2>();

        if (stickValue.sqrMagnitude > 0.01f)
        {
            Vector2 currentMousePos = Mouse.current.position.ReadValue();
            Vector2 newMousePos = currentMousePos + (_cursorSpeed * Time.unscaledDeltaTime * stickValue);

            newMousePos.x = Mathf.Clamp(newMousePos.x, 0, Screen.width);
            newMousePos.y = Mathf.Clamp(newMousePos.y, 0, Screen.height);

            Mouse.current.WarpCursorPosition(newMousePos);
        }
    }
    #endregion
}