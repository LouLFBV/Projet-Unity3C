using System;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    [SerializeField] private float manaRegenRate = 5f;
    [SerializeField] private float manaRegenDelay = 2f;
    [SerializeField] private float maxMana = 100f;

    public float CurrentMana => _currentMana;
    public float MaxMana => maxMana;

    public event Action OnManaChanged;

    private float _currentMana;
    private float _lastManaUseTime;
    private void Start()
    {
        _currentMana = maxMana;
    }

    private void Update()
    {
        if(Time.time - _lastManaUseTime >= manaRegenDelay)
        {
            RegenerateMana();
        }
    }

    private void RegenerateMana()
    {
        if (_currentMana < maxMana)
        {
            float previousMana = _currentMana;
            _currentMana += manaRegenRate * Time.deltaTime;
            _currentMana = Mathf.Min(_currentMana, maxMana);

            // On ne déclenche l'événement que si la valeur a réellement changé
            if (Mathf.Abs(_currentMana - previousMana) > 0.001f)
            {
                OnManaChanged?.Invoke();
            }
        }
    }

    public void ConsumeMana(float amount)
    {
        if (!HasEnoughMana(amount))
        {
            Debug.Log("Mana insuffisant !");
            return ;
        }

        _currentMana -= amount;
        _lastManaUseTime = Time.time;
        OnManaChanged?.Invoke();
        Debug.Log($"Mana consommé: {amount}. Mana restant: {_currentMana}");
    }

    public bool HasEnoughMana(float manaCost)
    {
        Debug.Log($"Vérification du mana: {_currentMana} >= {manaCost} ?");
        return _currentMana >= manaCost;
    }

    public void ResetMana()
    {
        _currentMana = maxMana;
        OnManaChanged?.Invoke();
    }
}
