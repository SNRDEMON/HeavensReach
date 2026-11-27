using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string AdjacentScene;
    [SerializeField] private Vector2 ExitDirection;
    private Vector2 entryPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        entryPos = other.transform.position;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Vector2 exitPos = collision.transform.position;

        Vector2 posDelta = exitPos - entryPos;
        posDelta = posDelta.normalized;

        float dot = Vector2.Dot(posDelta, ExitDirection);
        if (dot > 0)
        {
            if (!SceneManager.GetSceneByName(AdjacentScene).isLoaded)
            SceneManager.LoadSceneAsync(AdjacentScene,LoadSceneMode.Additive);
        }
        else if (dot < 0)
        {
            if (SceneManager.GetSceneByName(AdjacentScene).isLoaded)
            SceneManager.UnloadSceneAsync(AdjacentScene);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.hotPink;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawLine ((Vector2)transform.position, (Vector2)transform.position + ExitDirection);
    }
#endif

}
