using UnityEngine;

public class PatrolStateMachine
{
    private PatrolState curState;
}

public enum PatrolState
{
    Patrol,
    Suspicion,
    Chase,
    Tracking,
    Disengage
}
