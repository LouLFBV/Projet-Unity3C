using UnityEngine;
public abstract class PlayerState : MonoBehaviour
{
    protected PlayerCharacter character;

    public PlayerState(PlayerCharacter character)
    {
        this.character = character;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}