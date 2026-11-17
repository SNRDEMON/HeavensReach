using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.AI;
using System.Collections;
using UnityEditor;


public class DroneBehavior : EnemyBehavior
{
    [SerializeField] private List<Vector2> PatrolPath;
    [SerializeField] private bool cyclicalPatrol;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private NavMeshSurface myMesh;
    [SerializeField] private NavMeshAgent myAgent;
    [SerializeField] private EnemyVision eyes;

    [SerializeField] private float mySuspicionTotal = 0f;
    [SerializeField] private float suspicionMaximum;

    private PatrolState curState = PatrolState.Patrol;
    private int patrolLoopIndex;

    private Vector2 lastKnownPlayerLocation;

    private void OnEnable()
    {
        patrolLoopIndex = 0;
    }
    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();

        myAgent.updateRotation = false;
        myAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if(!gameObject.activeInHierarchy) return;

        switch (curState)
        {
            case PatrolState.Patrol:
                Patrol();
                break;
            case PatrolState.Suspicion:
                Suspicion();
                break;
            case PatrolState.Chase:
                Chase();
                break;
            case PatrolState.Tracking:
                break;
            case PatrolState.Disengage:
                break;
        }
    }

    private bool SeePlayer()
    {
        Vector2 locale;
        bool playerSeen = eyes.CanSeePlayer(out locale);
        if (playerSeen) lastKnownPlayerLocation = locale;
        return playerSeen;
    }

    private void Patrol()
    {
        myAgent.speed = patrolSpeed;
        Vector2 dir = PatrolPath[patrolLoopIndex] - (Vector2)transform.position;
        float dist = dir.magnitude;
        if(dist < patrolSpeed * 0.3f)
        {
            //transform.position = PatrolPath[patrolLoopIndex];
            patrolLoopIndex++;
            if (patrolLoopIndex >= PatrolPath.Count) patrolLoopIndex = 0;

            myAgent.SetDestination(PatrolPath[patrolLoopIndex]);
        }
        else
        {
            myAgent.SetDestination(PatrolPath[patrolLoopIndex]);
        }

        if (SeePlayer())
        {
            curState = PatrolState.Suspicion;
            mySuspicionTotal = 3.0f;
        }

    }

    private void Suspicion()
    {
        if (SeePlayer())
        {
            mySuspicionTotal += Time.deltaTime;
            myAgent.SetDestination(lastKnownPlayerLocation);
        }
        else
        {
            mySuspicionTotal -= Time.deltaTime;
        }
        float suspicionPercent = mySuspicionTotal / suspicionMaximum;
        if (suspicionPercent > 1.0f) suspicionPercent = 1.0f;
        myAgent.speed = patrolSpeed + ((chaseSpeed - patrolSpeed) * suspicionPercent);
        eyes.PointView(lastKnownPlayerLocation,suspicionPercent);


        if (mySuspicionTotal <= 0)
        {
            curState = PatrolState.Patrol;
            mySuspicionTotal = 0;
        }
        else if (mySuspicionTotal >= suspicionMaximum) curState = PatrolState.Chase;
                
    }

    private void Chase()
    {
        myAgent.SetDestination(lastKnownPlayerLocation);
        eyes.PointView(lastKnownPlayerLocation);
        SeePlayer();
    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Handles.color = Color.magenta;
        for (int i = 0; i < PatrolPath.Count; i++)
        {
            Gizmos.DrawSphere(PatrolPath[i], 0.2f);
            if (i < PatrolPath.Count - 1) Gizmos.DrawLine(PatrolPath[i], PatrolPath[i + 1]);
            else if(cyclicalPatrol) Gizmos.DrawLine(PatrolPath[i], PatrolPath[0]);
        }
    }
#endif


    private IEnumerator SustainedVision()
    {
        int i = 0;
        while (SeePlayer())
        {
            yield return null;
            i++;
            if (i >= 4) curState = PatrolState.Suspicion;
        }
    }

}
