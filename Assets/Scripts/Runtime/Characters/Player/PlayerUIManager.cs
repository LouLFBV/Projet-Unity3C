using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private ManaSystem _manaSystem;
    [SerializeField] private Image manaImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private Button _continueButtton;
    [SerializeField] private Button _restartButtton;
    [SerializeField] private Button _optionsButtton;
    [SerializeField] private Button _menuButtton;

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
    }
    public void OnClickRestartButton()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void OnClickOptionsButton()
    {
        _optionsPanel.SetActive(true);
    }
    public void OnClickMenuButton()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
