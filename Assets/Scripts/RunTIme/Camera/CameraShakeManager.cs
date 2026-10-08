using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    [SerializeField] private float _hurtShakeForce = 1f;
    [SerializeField] private float _attackShakeForce = 0.5f;

    private PlayerCharacter _playerCharacter;

    private void Awake()
    {
        if (_impulseSource == null)
            _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

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

    private void OnDisable() => UnsubscribeEvents();
    private void OnDestroy() => UnsubscribeEvents();

    private void UnsubscribeEvents()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnHurt -= TriggerShakeOnHurt;
            _playerCharacter.OnAttack -= TriggerShakeOnAttack;
            _playerCharacter = null;
        }
    }

    private void TriggerShakeOnAttack() => GenerateShake(_attackShakeForce);
    private void TriggerShakeOnHurt() => GenerateShake(_hurtShakeForce);

    public void GenerateShake(float force = 1f)
    {
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulseWithForce(force);
        }
    }
}