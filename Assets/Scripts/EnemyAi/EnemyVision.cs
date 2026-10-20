using UnityEngine;
using System.Collections.Generic;

public class EnemyVision : MonoBehaviour
{

    [SerializeField] private float viewAngle;
    [SerializeField] private float ViewDistance;
    [SerializeField] private float ViewSpan;
    [SerializeField,Min(3)] private int ViewRayDensity;
    [SerializeField] private bool mirrorX;

    public void Flip()
    {
        mirrorX = !mirrorX;
    }

    public bool CanSeePlayer(out Vector2 spot)
    {
        spot = new Vector2();
        List<Ray2D> rayCone = GenerateVision();
        for (int i = 0; i < rayCone.Count; i++)
        {
            LayerMask mask = 6;
            RaycastHit2D hit = Physics2D.Raycast(rayCone[i].origin, rayCone[i].direction,ViewDistance);
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.tag == "Player")
            {
                spot = hit.collider.transform.position;
                return true;
            }
        }
        return false;
    }

    private List<Ray2D> GenerateVision()
    {
        float densityInverse = 1.0f / (float)ViewRayDensity;

        float lowRange = viewAngle - (ViewSpan * 0.5f);
        float highRange = viewAngle + (ViewSpan * 0.5f);

        List<Ray2D> rayCone = new List<Ray2D>();
        for (int i = 0; i < ViewRayDensity; i++)
        {
            float curAngle = Mathf.Lerp(lowRange, highRange, (float)i * densityInverse);
            Vector2 angle = new Vector2(Mathf.Cos(curAngle),Mathf.Sin(curAngle));
            if (mirrorX) angle.x *= -1.0f;
            Ray2D ray = new Ray2D(transform.position,angle.normalized);
            rayCone.Add(ray);
        }


        return rayCone;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        List<Ray2D> rayCone = GenerateVision();
        for (int i = 0; i < rayCone.Count; i++)
        {
            Gizmos.color = Color.limeGreen;
            Gizmos.DrawLine(rayCone[i].origin, (rayCone[i].direction * ViewDistance) + rayCone[i].origin);
        }
    }
#endif

}
