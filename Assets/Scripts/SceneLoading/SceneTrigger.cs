using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string AdjacentScene;
    [SerializeField] private Vector2 ExitDirection;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggering {AdjacentScene}");
        Vector2 velocity = other.gameObject.GetComponent<Rigidbody2D>().linearVelocity;
        Debug.Log(velocity.x);
        float dot = Vector2.Dot(velocity, ExitDirection);
        if (dot > 0)
        {
            SceneManager.LoadSceneAsync(AdjacentScene);
        }else if (dot < 0)
        {
            SceneManager.UnloadSceneAsync(AdjacentScene);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Triggering {AdjacentScene}");
        Vector2 velocity = collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity;
        Debug.Log(velocity.x);
        float dot = Vector2.Dot(velocity, ExitDirection);
        if (dot > 0)
        {
            SceneManager.LoadSceneAsync(AdjacentScene);
        }
        else if (dot < 0)
        {
            SceneManager.UnloadSceneAsync(AdjacentScene);
        }
    }
}
