using UnityEngine;

public class ParticuleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _jumpParticleSystem;
    //[SerializeField] private Transform _spawnPositionJumpParticule;
    [SerializeField] private PlayerCharacter _playerCharacter;

    private void Awake()
    {
        if (_playerCharacter == null)
        {
            _playerCharacter = GetComponent<PlayerCharacter>();
        }

        if (_jumpParticleSystem != null)
        {
            var main = _jumpParticleSystem.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    private void OnEnable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump += PlayJumpParticule;
        }
    }

    private void OnDisable()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.OnJump -= PlayJumpParticule;
        }
    }

    private void PlayJumpParticule()
    {
        if (_jumpParticleSystem != null)
        {
            //if (_spawnPositionJumpParticule != null)
            //{
            //    _jumpParticleSystem.transform.position = _spawnPositionJumpParticule.position;
            //}

            _jumpParticleSystem.Play();
        }
    }
}