using UnityEngine;

public class ParticuleManager : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem _jumpParticleSystem;
    [SerializeField] private ParticleSystem _groundParticleSystem;

    [Header("Spawn Points")]
    [SerializeField] private Transform _jumpSpawnPoint;
    [SerializeField] private Transform _groundSpawnPoint;

    [Header("References")]
    [SerializeField] private PlayerCharacter _playerCharacter;

    private void Awake()
    {
        if (_playerCharacter == null)
            _playerCharacter = GetComponent<PlayerCharacter>();

        ConfigureWorldSpace(_jumpParticleSystem);
        ConfigureWorldSpace(_groundParticleSystem);
    }

    private void ConfigureWorldSpace(ParticleSystem ps)
    {
        if (ps == null) return;

        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        foreach (var childPS in ps.GetComponentsInChildren<ParticleSystem>())
        {
            var childMain = childPS.main;
            childMain.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    #region Subscriptions
    private void OnEnable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump += PlayJumpParticule;
            _playerCharacter.OnGround += PlayGroundParticule;
        }
    }

    private void OnDisable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump -= PlayJumpParticule;
            _playerCharacter.OnGround -= PlayGroundParticule;
        }
    }
    #endregion

    #region Play Particle Methods
    private void PlayJumpParticule()
    {
        if (_jumpParticleSystem == null) return;

        if (_jumpSpawnPoint != null)
        {
            _jumpParticleSystem.transform.position = _jumpSpawnPoint.position;
        }

        _jumpParticleSystem.Play();
    }

    private void PlayGroundParticule()
    {
        if (_groundParticleSystem == null) return;

        if (_groundSpawnPoint != null)
        {
            _groundParticleSystem.transform.position = _groundSpawnPoint.position;
        }

        _groundParticleSystem.Play();
    }
    #endregion
}