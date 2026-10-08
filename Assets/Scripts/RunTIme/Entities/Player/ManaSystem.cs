using System;
using UnityEngine;
/// <summary>
/// Manages the character's mana, including consumption, regeneration and reset operations.
/// </summary>
public class ManaSystem : MonoBehaviour
{
    #region --- MANA SETTINGS ---

    /// <summary>
    /// Amount of mana regenerated per second.
    /// </summary>
    [SerializeField] private float manaRegenRate = 5f;
    /// <summary>
    /// Delay in seconds before mana regeneration starts after mana consumption.
    /// </summary>
    [SerializeField] private float manaRegenDelay = 2f;
    /// <summary>
    /// Maximum amount of mana available.
    /// </summary>

    [SerializeField] private float maxMana = 100f;
    #endregion

    #region --- MANA STATE ---

    /// <summary>
    /// Gets the current amount of mana.
    /// </summary>
    public float CurrentMana => _currentMana;
    /// <summary>
    /// Gets the maximum amount of mana.
    /// </summary>
    public float MaxMana => maxMana;
    /// <summary>
    /// Invoked whenever the current mana value changes.
    /// </summary>
    public event Action OnManaChanged;
    /// <summary>
    /// Stores the current amount of mana.
    /// </summary>
    private float _currentMana;
    /// <summary>
    /// Stores the time when mana was last consumed.
    /// </summary>
    private float _lastManaUseTime;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the current mana to its maximum value.
    /// </summary>
    private void Start()
    {
        _currentMana = maxMana;
    }
    /// <summary>
    /// Handles mana regeneration over time after the regeneration delay.
    /// </summary>
    private void Update()
    {
        if(Time.time - _lastManaUseTime >= manaRegenDelay)
        {
            RegenerateMana();
        }
    }
    #endregion

    #region --- MANA MANAGEMENT ---

    /// <summary>
    /// Regenerates mana over time until the maximum mana value is reached.
    /// </summary>
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
    /// <summary>
    /// Consumes the specified amount of mana if enough mana is available.
    /// </summary>
    /// <param name="amount">Amount of mana to consume.</param>
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
    /// <summary>
    /// Checks whether enough mana is available for the specified mana cost.
    /// </summary>
    /// <param name="manaCost">Amount of mana required.</param>
    /// <returns><c>true</c> if enough mana is available; otherwise, <c>false</c>.</returns>
    public bool HasEnoughMana(float manaCost)
    {
        Debug.Log($"Vérification du mana: {_currentMana} >= {manaCost} ?");
        return _currentMana >= manaCost;
    }
    /// <summary>
    /// Resets the current mana to its maximum value.
    /// </summary>
    public void ResetMana()
    {
        _currentMana = maxMana;
        OnManaChanged?.Invoke();
    }
    #endregion
}
