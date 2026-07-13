using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }
    public bool canChangeState;

    public void Init(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        if(!canChangeState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SwithOffStateMachine() => canChangeState = false;
}
