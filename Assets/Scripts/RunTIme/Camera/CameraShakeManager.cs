using UnityEngine;
using Unity.Cinemachine;
/// <summary>
/// Manages camera shake effects triggered by player events.
/// </summary>
public class CameraShakeManager : MonoBehaviour
{
    #region --- REFERENCES ---

    /// <summary>
    /// Impulse source used to generate camera shake effects.
    /// </summary>
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    #endregion

    #region --- SHAKE SETTINGS ---

    /// <summary>
    /// Force applied to the camera shake when the player is hurt.
    /// </summary>
    [SerializeField] private float _hurtShakeForce = 1f;
    /// <summary>
    /// Force applied to the camera shake when the player attacks.
    /// </summary>
    [SerializeField] private float _attackShakeForce = 0.5f;
    #endregion

    #region --- PLAYER STATE ---

    /// <summary>
    /// Player character whose events are used to trigger camera shake effects.
    /// </summary>
    private PlayerCharacter _playerCharacter;
    #endregion

    #region --- UNITY LIFECYCLE ---

    /// <summary>
    /// Initializes the impulse source used to generate camera shake effects.
    /// Attempts to retrieve the component from the current GameObject when none is assigned.
    /// </summary>
    private void Awake()
    {
        if (_impulseSource == null)
            _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    #endregion

    #region --- PLAYER MANAGEMENT ---

    /// <summary>
    /// Initializes the camera shake manager with the specified player.
    /// Subscribes to the player's hurt and attack events.
    /// </summary>
    /// <param name="player">
    /// Player character whose events will trigger camera shake effects.
    /// </param>
    public void Initialize(PlayerCharacter player)
    {
        UnsubscribeEvents();

        _playerCharacter = player;

        if (_playerCharacter != null)
        {
            _playerCharacter.OnHurt += TriggerShakeOnHurt;
            _playerCharacter.OnAttack += TriggerShakeOnAttack;
        }
    }
    /// <summary>
    /// Unsubscribes from player events when the component is disabled.
    /// </summary>
    private void OnDisable() => UnsubscribeEvents();
    /// <summary>
    /// Unsubscribes from player events when the component is destroyed.
    /// </summary>
    private void OnDestroy() => UnsubscribeEvents();
    /// <summary>
    /// Unsubscribes from all player events and clears the current player reference.
    /// </summary>
    private void UnsubscribeEvents()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnHurt -= TriggerShakeOnHurt;
            _playerCharacter.OnAttack -= TriggerShakeOnAttack;
            _playerCharacter = null;
        }
    }
    #endregion

    #region --- SHAKE EVENTS ---

    /// <summary>
    /// Triggers a camera shake when the player performs an attack.
    /// </summary>
    private void TriggerShakeOnAttack() => GenerateShake(_attackShakeForce);
    /// <summary>
    /// Triggers a camera shake when the player is hurt.
    /// </summary>
    private void TriggerShakeOnHurt() => GenerateShake(_hurtShakeForce);
    #endregion

    #region --- SHAKE MANAGEMENT ---

    /// <summary>
    /// Generates a camera shake impulse using the specified force.
    /// </summary>
    /// <param name="force">
    /// Force applied to the generated camera shake impulse.
    /// </param>
    public void GenerateShake(float force = 1f)
    {
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulseWithForce(force);
        }
    }
    #endregion
}