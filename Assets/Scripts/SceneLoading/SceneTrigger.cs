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
            SceneManager.LoadSceneAsync(AdjacentScene,LoadSceneMode.Additive);
        }
        else if (dot < 0)
        {
            SceneManager.UnloadSceneAsync(AdjacentScene);
        }
    }
}
