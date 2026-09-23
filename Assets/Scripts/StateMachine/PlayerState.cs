using UnityEngine;
public abstract class PlayerState : MonoBehaviour
{
    protected PlayerCharacter character;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerCharacter character, PlayerStateMachine stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}