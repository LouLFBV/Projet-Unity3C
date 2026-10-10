using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [Header("ManaSystem")]
    [SerializeField] private ManaSystem _manaSystem;
    [SerializeField] private Image manaImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsPanel;

    [Header("First Selected Buttons")]
    [SerializeField] private GameObject _firstPauseButton;  
    [SerializeField] private GameObject _firstOptionButton;

    void OnEnable()
    {
        _manaSystem.OnManaChanged += UpdateManaBar;

        //if (InputDeviceManager.Instance != null)
        //{
        //    InputDeviceManager.OnControlSchemeChanged += OnControlSchemeChanged;
        //    UpdateScheme(InputDeviceManager.Instance.CurrentScheme);
        //}
    }


    private void OnDisable()
    {
        //if (InputDeviceManager.Instance != null)
        //{
        //    InputDeviceManager.OnControlSchemeChanged -= OnControlSchemeChanged;
        //}
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
        OpenOptions();
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

        if (_pauseMenu.activeSelf) CloseOptions();
        Debug.Log($"Toggling menu. Will be active: {willBeActive}");
        _pauseMenu.SetActive(willBeActive);
        if (!_pauseMenu.activeSelf) CloseOptions();

        Time.timeScale = willBeActive ? 0f : 1f;

        string mapName = willBeActive ? "UI" : "Player";
        _playerInput?.SwitchCurrentActionMap(mapName);
    }

    private void OpenOptions()
    {
        _optionsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstOptionButton);
    }

    public void CloseOptions()
    {
        if (!_pauseMenu.activeSelf) return;
        _optionsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstPauseButton);
    }

    private void OnControlSchemeChanged(ControlScheme scheme)
    {
        UpdateScheme(scheme);
    }

    private void UpdateScheme(ControlScheme scheme)
    {
        bool isGamepad = (scheme == ControlScheme.Gamepad);

        if (isGamepad)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
