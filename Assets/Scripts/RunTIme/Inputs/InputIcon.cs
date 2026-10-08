using UnityEngine;
/// <summary>
/// Manages the visibility of keyboard/mouse and gamepad input icons
/// based on the currently active control scheme.
/// </summary>
public class InputIcon : MonoBehaviour
{
    #region --- INPUT ICON REFERENCES ---

    /// <summary>
    /// GameObject displaying the keyboard and mouse input icon.
    /// </summary>
    [SerializeField] private GameObject _keyboardSprite;
    /// <summary>
    /// GameObject displaying the gamepad input icon.
    /// </summary>
    [SerializeField] private GameObject _gamepadSprite;

    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the input icons with the default keyboard and mouse configuration.
    /// Logs an error when one or both icon references are missing.
    /// </summary>
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
    /// <summary>
    /// Subscribes to control scheme changes and synchronizes the icons
    /// with the currently active control scheme.
    /// </summary>
    private void OnEnable()
    {
        InputDeviceManager.OnControlSchemeChanged += OnSchemeChanged;

        if (InputDeviceManager.Instance != null)
        {
            OnSchemeChanged(InputDeviceManager.Instance.CurrentScheme);
        }
    }
    /// <summary>
    /// Unsubscribes from control scheme changes when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        InputDeviceManager.OnControlSchemeChanged -= OnSchemeChanged;
    }
    #endregion

    #region --- INPUT SCHEME MANAGEMENT ---

    /// <summary>
    /// Updates the active input icon according to the specified control scheme.
    /// </summary>
    /// <param name="scheme">
    /// Control scheme currently used by the player.
    /// </param>
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
    #endregion
}