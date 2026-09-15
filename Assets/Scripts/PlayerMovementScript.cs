using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField] private float _vertWallCheckDistance = 1f;
    [SerializeField]private float _horWallCheckDistance = 1f;
    //probably move this away from being layers later but idk it works for walls
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _jumpForce = 1f;
    private bool _isGrounded = true;
    private Vector2 move;
    private Rigidbody2D _playerRB;
    
    //probably a better way to do this with like a for loop or coroutine but idk I'm lazy
    //yeah rereading this half this shit needs to be on coroutines instead of if statements I just didn't want to impliment them
    private float _jumpCooldown = 0f;


    //dash variables
    private float _dashCooldown = 0f;
    private float _dashAirTimeLeft = 0f;
    [SerializeField] private float _dashAirTimeSet = 1f;
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
    public void OnDash(InputAction.CallbackContext context)


    {
        if (_dashCooldown <= 0f)
        {
            if (move.x < 0f)
            {
                _playerRB.AddForce(Vector2.left * _dashDistance, ForceMode2D.Impulse);
                _dashCooldown = 1f;
                _playerRB.gravityScale = 0f;
            }
            else if (move.x > 0f)
            {
                _playerRB.AddForce(Vector2.right * _dashDistance, ForceMode2D.Impulse);
                _dashCooldown = 1f;
                Debug.Log("dashtried");
                _playerRB.gravityScale = 0f;
            }
        }
       


    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded == true && (_jumpCooldown <= 0))
        {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpCooldown = 1f;
            _isGrounded =false;

        }

    }
    private void Update()
    {

        //add raycasts for bottom left, bottomright, etc, etc, in order to stop jittering on corners

        if (move.x > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, _horWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.x = 0;
            }
        }

        if (move.x < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, _horWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.x = 0;
            }
        }
        
        if (move.y > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, _vertWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;


            }
        }
        if (move.y < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _vertWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;
            }
        }
        Vector3 direction = new Vector3(move.x, 0, 0).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        //this feels bad to check this every frame but idk, all the stuff above is raycasting to check for walls
        if (_isGrounded == false && (_jumpCooldown <= 0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _vertWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                _isGrounded = true;

            }
        }
        if (_dashAirTimeLeft <= 0)
        {
            _playerRB.gravityScale = 1f;
        }
        //bad way to do a cooldown but this is here because you could jump twice before you could get off the ground so you'll want to replace it with a coroutine probably
        _jumpCooldown -= Time.deltaTime;
        _dashCooldown -= Time.deltaTime;
          
        }
    //some code from my old game
}
