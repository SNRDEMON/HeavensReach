using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField]private float _wallCheckDistance = 1f;
    //probably move this away from being layers later but idk it works for walls
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _jumpForce = 1f;
    private bool _isGrounded = true;
    private Vector2 move;
    private Rigidbody2D _playerRB;

    //probably a better way to do this with like a for loop or something but idk I'm lazy rn
    private float _jumpCooldown = 0f;
    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
    }
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
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded && (_jumpCooldown <= 0))
        {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isGrounded=false;
            _jumpCooldown = 1f;
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
                _isGrounded = true;

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

        //this feels bad to check this every frame but idk, all the stuff above is raycasting to check for walls
        if (_isGrounded == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _wallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                _isGrounded = true;

            }
        }
        //bad way to do a cooldown but this is here because you could jump twice before you could get off the ground so you'll want to replace it
        _jumpCooldown -= Time.deltaTime;
        }
    //some code from my old game
}
