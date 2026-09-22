using System.Collections.Generic;
using UnityEngine;

public class DroneBehavior : EnemyBehavior
{
    [SerializeField] private List<Vector2> PatrolPath;
    [SerializeField] private bool cyclicalPatrol;
}
