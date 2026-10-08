using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [Header("ManaSystem")]
    [SerializeField] private ManaSystem _manaSystem;
    [SerializeField] private Image manaImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsPanel;

    void OnEnable()
    {
        _manaSystem.OnManaChanged += UpdateManaBar;
    }

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
    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }
    private void UpdateManaBar()
    {
        if (_manaSystem != null && manaImage != null)
        {
            float manaPercentage = _manaSystem.CurrentMana / _manaSystem.MaxMana;
            manaImage.fillAmount = manaPercentage;
        }
    }

    public void OnClickContinueButton()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _playerInput.SwitchCurrentActionMap("Player");
    }
    public void OnClickRestartButton()
    {
        OnClickContinueButton();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void OnClickOptionsButton()
    {
        _optionsPanel.SetActive(true);
    }
    public void OnClickMenuButton()
    {
        OnClickContinueButton();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

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
}
