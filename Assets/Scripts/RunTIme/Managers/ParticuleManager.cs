using UnityEngine;
/// <summary>
/// Manages the particle effects associated with player actions,
/// such as jumping and landing.
/// </summary>
public class ParticuleManager : MonoBehaviour
{
    #region --- PARTICLE SYSTEMS ---
    [Header("Particle Systems")]

    /// <summary>
    /// Particle system played when the player jumps.
    /// </summary>
    [SerializeField] private ParticleSystem _jumpParticleSystem;
    /// <summary>
    /// Particle system played when the player lands on the ground.
    /// </summary>
    [SerializeField] private ParticleSystem _groundParticleSystem;
    #endregion

    #region --- SPAWN POINTS ---
    [Header("Spawn Points")]
    /// <summary>
    /// Transform defining the spawn position of the jump particle effect.
    /// </summary>
    [SerializeField] private Transform _jumpSpawnPoint;
    /// <summary>
    /// Transform defining the spawn position of the ground particle effect.
    /// </summary>
    [SerializeField] private Transform _groundSpawnPoint;
    #endregion

    #region --- REFERENCES ---
    [Header("References")]
    /// <summary>
    /// Player character associated with this particle manager.
    /// </summary>
    [SerializeField] private PlayerCharacter _playerCharacter;
    #endregion

    /// <summary>
    /// Initializes the player character reference and configures
    /// the particle systems to simulate in world space.
    /// </summary>
    private void Awake()
    {
        if (_playerCharacter == null)
            _playerCharacter = GetComponent<PlayerCharacter>();

        ConfigureWorldSpace(_jumpParticleSystem);
        ConfigureWorldSpace(_groundParticleSystem);
    }

    /// <summary>
    /// Configures a particle system and all of its child particle systems
    /// to simulate in world space.
    /// </summary>
    /// <param name="ps">Particle system to configure.</param>
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
    #region --- SUBSCRIPTIONS ---

    /// <summary>
    /// Subscribes to the player character events when the component is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump += PlayJumpParticule;
            _playerCharacter.OnGround += PlayGroundParticule;
        }
    }
    /// <summary>
    /// Unsubscribes from the player character events when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump -= PlayJumpParticule;
            _playerCharacter.OnGround -= PlayGroundParticule;
        }
    }
    #endregion

    #region --- PARTICLE PLAYBACK ---

    /// <summary>
    /// Positions and plays the jump particle effect.
    /// </summary>
    private void PlayJumpParticule()
    {
        if (_jumpParticleSystem == null) return;

        if (_jumpSpawnPoint != null)
        {
            _jumpParticleSystem.transform.position = _jumpSpawnPoint.position;
        }

        _jumpParticleSystem.Play();
    }
    /// <summary>
    /// Positions and plays the ground particle effect.
    /// </summary>
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