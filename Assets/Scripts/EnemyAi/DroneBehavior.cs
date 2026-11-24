using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.AI;
using System.Collections;
using UnityEditor;


public class DroneBehavior : EnemyBehavior
{
    [SerializeField] private List<Vector2> PatrolPath;
    [SerializeField] private List<float> PatrolViewAngles;
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
                Tracking();
                break;
            case PatrolState.Disengage:
                Disengage();
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

    bool forward = true;
    private void Patrol()
    {
        myAgent.speed = patrolSpeed;
        Vector2 dir = PatrolPath[patrolLoopIndex] - (Vector2)transform.position;
        float dist = dir.magnitude;
        if(dist < patrolSpeed * 0.3f)
        {
            //transform.position = PatrolPath[patrolLoopIndex];
            if (!cyclicalPatrol && !forward) patrolLoopIndex--;
            else patrolLoopIndex++;

            if (patrolLoopIndex < 0)
            {
                forward = true;
                patrolLoopIndex = 0;
            }
            if (!cyclicalPatrol && patrolLoopIndex >= PatrolPath.Count)
            {
                forward = false;
                patrolLoopIndex--;
            }
            else if (patrolLoopIndex >= PatrolPath.Count) patrolLoopIndex = 0;

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

    private bool sightMaintained = false;
    private float timeMaintained = 0;
    private void Suspicion()
    {
        if (SeePlayer())
        {
            if (sightMaintained == false) timeMaintained = 0;
            sightMaintained = true;
            mySuspicionTotal += Time.deltaTime + timeMaintained;
            myAgent.SetDestination(lastKnownPlayerLocation);
        }
        else
        {
            if(sightMaintained) timeMaintained = 0;
            sightMaintained = false;
            mySuspicionTotal -= (Time.deltaTime + timeMaintained);
        }
        timeMaintained += Time.deltaTime;
        float suspicionPercent = mySuspicionTotal / suspicionMaximum;
        if (suspicionPercent > 1.0f) suspicionPercent = 1.0f;
        myAgent.speed = patrolSpeed + ((chaseSpeed - patrolSpeed) * suspicionPercent);
        eyes.PointView(lastKnownPlayerLocation,suspicionPercent);


        if (mySuspicionTotal <= 0)
        {
            curState = PatrolState.Disengage;
            mySuspicionTotal = 0;
        }
        else if (mySuspicionTotal >= suspicionMaximum) curState = PatrolState.Chase;
                
    }

    private void Chase()
    {
        myAgent.SetDestination(lastKnownPlayerLocation);
        eyes.PointView(lastKnownPlayerLocation);
        if (!SeePlayer())
        {
            curState = PatrolState.Suspicion;
        }
    }

    private void Tracking()
    {

    }

    private void Disengage()
    { 
        int minIndex = 0;
        float minDist = Vector2.Distance(transform.position, PatrolPath[0]);
        for (int i = 1; i < PatrolPath.Count; i++)
        {
            float delta = Vector2.Distance(transform.position, PatrolPath[i]);
            if (delta < minDist)
            {
                minIndex = i;
                minDist = delta;
            }
        }
        patrolLoopIndex = minIndex;
        curState = PatrolState.Patrol;

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

        if (PatrolViewAngles.Count > 0) 
        {
            for (int j = 0; j < PatrolViewAngles.Count; j++) { 
            List<Ray2D> rayCone = eyes.GenerateVision(PatrolViewAngles[j], PatrolPath[j]);
                for (int i = 0; i < rayCone.Count; i++)
                {
                    Gizmos.color = Color.limeGreen;
                    Gizmos.DrawLine(rayCone[i].origin, (rayCone[i].direction * eyes.viewDistance) + rayCone[i].origin);
                }
            }
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
