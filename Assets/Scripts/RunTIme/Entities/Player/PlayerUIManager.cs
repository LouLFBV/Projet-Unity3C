using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// Manages the player's UI, including the mana bar, pause menu and options panel.
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    #region --- PLAYER INPUT ---

    /// <summary>
    /// Reference to the player's input system.
    /// </summary>
    [SerializeField] private PlayerInput _playerInput;
    #endregion

    #region --- MANA SYSTEM ---

    /// <summary>
    /// Reference to the player's mana system.
    /// </summary>
    [Header("ManaSystem")]
    [SerializeField] private ManaSystem _manaSystem;
    /// <summary>
    /// UI image used to display the current mana amount.
    /// </summary>
    [SerializeField] private Image manaImage;
    #endregion

    #region --- PAUSE MENU ---

    /// <summary>
    /// Reference to the pause menu UI object.
    /// </summary>
    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;
    /// <summary>
    /// Reference to the options panel UI object.
    /// </summary>

    [SerializeField] private GameObject _optionsPanel;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Subscribes to the mana system's mana changed event.
    /// </summary>
    void OnEnable()
    {
        _manaSystem.OnManaChanged += UpdateManaBar;
    }
    /// <summary>
    /// Initializes the pause menu and options panel.
    /// </summary>
    public void Start()
    {
        if (_pauseMenu != null)
        {
            _pauseMenu.SetActive(false);
        }
        if (_optionsPanel != null)
        {
            _optionsPanel.SetActive(false);
        }
    }
    #endregion

    #region --- PLAYER MANAGEMENT ---

    /// <summary>
    /// Sets the player input reference used by the UI manager.
    /// </summary>
    /// <param name="playerInput">Input system associated with the player.</param>
    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }
    #endregion

    #region --- MANA MANAGEMENT ---

    /// <summary>
    /// Updates the mana bar according to the player's current mana percentage.
    /// </summary>
    private void UpdateManaBar()
    {
        if (_manaSystem != null && manaImage != null)
        {
            float manaPercentage = _manaSystem.CurrentMana / _manaSystem.MaxMana;
            manaImage.fillAmount = manaPercentage;
        }
    }
    #endregion

    #region --- PAUSE MENU MANAGEMENT ---

    /// <summary>
    /// Closes the pause menu, resumes the game and switches the input action map to the player map.
    /// </summary>
    public void OnClickContinueButton()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _playerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Restarts the current scene after closing the pause menu.
    /// </summary>
    public void OnClickRestartButton()
    {
        OnClickContinueButton();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Opens the options panel.
    /// </summary>
    public void OnClickOptionsButton()
    {
        _optionsPanel.SetActive(true);
    }
    /// <summary>
    /// Returns to the main menu after closing the pause menu.
    /// </summary>
    public void OnClickMenuButton()
    {
        OnClickContinueButton();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Toggles the pause menu and switches between the player and UI input action maps.
    /// </summary>
    public void ToggleMenu()
    {
        if (_pauseMenu == null)
        {
            return;
        }

        bool willBeActive = !_pauseMenu.activeSelf;
        Debug.Log($"Toggling menu. Will be active: {willBeActive}");
        _pauseMenu.SetActive(willBeActive);
        _optionsPanel.SetActive(false);
        Time.timeScale = willBeActive ? 0f : 1f;

        string mapName = willBeActive ? "UI" : "Player";
        _playerInput?.SwitchCurrentActionMap(mapName);
    }
    #endregion
}
