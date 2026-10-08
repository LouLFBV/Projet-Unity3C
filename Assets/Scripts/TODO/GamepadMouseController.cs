using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadMouseController : MonoBehaviour
{
    [SerializeField] private float _cursorSpeed = 1000f;
    [SerializeField] private InputActionProperty _rightStickAction;

    private void OnEnable() => _rightStickAction.action?.Enable();
    private void OnDisable() => _rightStickAction.action?.Disable();

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
}