using UnityEngine;

public class InputIcon : MonoBehaviour
{
    [SerializeField] private GameObject _keyboardSprite;
    [SerializeField] private GameObject _gamepadSprite;

    private void Awake()
    {
        if (_keyboardSprite == null || _gamepadSprite == null)
        {
            Debug.LogError("InputIcon: Please assign both keyboard and gamepad sprites in the inspector.");
            return;
        }
        _keyboardSprite.SetActive(true);
        _gamepadSprite.SetActive(false);
    }

    private void OnEnable()
    {
        InputDeviceManager.OnControlSchemeChanged += OnSchemeChanged;

        if (InputDeviceManager.Instance != null)
        {
            OnSchemeChanged(InputDeviceManager.Instance.CurrentScheme);
        }
    }

    private void OnDisable()
    {
        InputDeviceManager.OnControlSchemeChanged -= OnSchemeChanged;
    }

    private void OnSchemeChanged(ControlScheme scheme)
    {
        if (_keyboardSprite == null || _gamepadSprite == null)
        {
            return;
        }
        switch(scheme)
        {
            case ControlScheme.KeyboardMouse:
                _keyboardSprite.SetActive(true);
                _gamepadSprite.SetActive(false);
                break;
            case ControlScheme.Gamepad:
                _keyboardSprite.SetActive(false);
                _gamepadSprite.SetActive(true);
                break;
            default:
                break;
        }
    }
}