using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.AI;


public class DroneBehavior : EnemyBehavior
{
    [SerializeField] private List<Vector2> PatrolPath;
    [SerializeField] private bool cyclicalPatrol;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private NavMeshSurface myMesh;
    [SerializeField] private NavMeshAgent myAgent;

    private PatrolState curState = PatrolState.Patrol;
    private int patrolLoopIndex;

    private void OnEnable()
    {
        patrolLoopIndex = 0;
    }

    private void Update()
    {
        if(!gameObject.activeInHierarchy) return;

        switch (curState)
        {
            case PatrolState.Patrol:
                Patrol();
                break;
        }
    }

    private void Patrol()
    {
        Vector2 dir = PatrolPath[patrolLoopIndex] - (Vector2)transform.position;
        float dist = dir.magnitude;
        if(dist < patrolSpeed)
        {
            transform.position = PatrolPath[patrolLoopIndex];
            patrolLoopIndex++;
            if (patrolLoopIndex >= PatrolPath.Count) patrolLoopIndex = 0;
            NavMeshPath path = new NavMeshPath();
            if (myAgent.CalculatePath(PatrolPath[patrolLoopIndex], path))
            {
                myAgent.SetPath(path);
            }
        }
        else
        {
            if (myAgent.path == null)
            {
                NavMeshPath path = new NavMeshPath();
                if (myAgent.CalculatePath(PatrolPath[patrolLoopIndex], path))
                {
                    myAgent.SetPath(path);
                }
            }
        }

    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        for (int i = 0; i < PatrolPath.Count; i++)
        {
            Gizmos.DrawSphere(PatrolPath[i], 0.2f);
            if (i < PatrolPath.Count - 1) Gizmos.DrawLine(PatrolPath[i], PatrolPath[i + 1]);
            else if(cyclicalPatrol) Gizmos.DrawLine(PatrolPath[i], PatrolPath[0]);
        }
    }
#endif
}
