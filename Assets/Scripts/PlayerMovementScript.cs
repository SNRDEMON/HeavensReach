using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField]private float _wallCheckDistance = 1f;
    //probably move this away from being layers later but idk it works for walls
    [SerializeField] private LayerMask _wallLayer;
    private bool _isGrounded = true;
    private Vector2 move;

    public void OnMove(InputAction.CallbackContext context)
    {

        if (_isGrounded)
        {
            move = context.ReadValue<Vector2>();
        }
        else
        {
            move = context.ReadValue<Vector2>();
        }

    }

    private void Update()
    {

        if (move.x > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.x = 0;
            }
        }

        if (move.x < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.x = 0;
            }
        }

        if (move.y > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, _wallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;
            }
        }
        if (move.y < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _wallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;
            }
        }
        Vector3 direction = new Vector3(move.x, 0, 0).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }
    //some code from my old game
}
