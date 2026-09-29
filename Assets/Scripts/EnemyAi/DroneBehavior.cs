using System.Collections.Generic;
using UnityEngine;

public class DroneBehavior : EnemyBehavior
{
    [SerializeField] private List<Vector2> PatrolPath;
    [SerializeField] private bool cyclicalPatrol;
    [SerializeField] private float patrolSpeed;

    private PatrolState curState = PatrolState.Patrol;
    private int patrolLoopIndex;

    private void OnEnable()
    {
        patrolLoopIndex = 0;
    }

    private void Update()
    {
        if(!gameObject.activeInHierarchy) return;
    }

    private void Patrol()
    {
        Vector2 dir = PatrolPath[patrolLoopIndex] - (Vector2)transform.position;

    }
}
