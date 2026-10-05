using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private ManaSystem _manaSystem;
    [SerializeField] private Image manaImage;

    [SerializeField] private GameObject _pauseMenu;
    void OnEnable()
    {
        _manaSystem.OnManaChanged += UpdateManaBar;
    }

    private void UpdateManaBar()
    {
        if (_manaSystem != null && manaImage != null)
        {
            float manaPercentage = _manaSystem.CurrentMana / _manaSystem.MaxMana;
            manaImage.fillAmount = manaPercentage;
        }
    }
}
