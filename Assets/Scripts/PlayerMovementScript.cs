using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    private float _speed = 5f;


    [Header("Wall checking Stuff")]
    [SerializeField] private float _vertWallCheckDistance = 1f;
    [SerializeField]private float _horWallCheckDistance = 1f;


    //probably move this away from being layers later but idk it works for walls
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _jumpForce = 1f;
    private bool _isGrounded = true;
    //use below for wallclimbing
    private bool _isAttachedToWall = false;
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

            move = context.ReadValue<Vector2>();

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
    public void OnCrouch(InputAction.CallbackContext context)
    {

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

    //I could probably move all these raycasts to a function that returns them I think instead of this
    private void Update()
    {
        if (move.x > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, _horWallCheckDistance, _wallLayer);


            Vector2 dirlow = new Vector2(_horWallCheckDistance, -_vertWallCheckDistance).normalized;

            RaycastHit2D hitlow = Physics2D.Raycast(transform.position,dirlow,_horWallCheckDistance,_wallLayer);


            Color debugColor = hitlow.collider != null ? Color.green : Color.red;
            Debug.DrawRay(transform.position, new Vector2(_horWallCheckDistance, -_vertWallCheckDistance), debugColor);
            
            if (hit.collider != null)
            {
                move.x = 0;
            }
            else if (hitlow.collider != null)
            {
                if (_isGrounded == false)
                {

                    move.x = 0;
                    Debug.Log("hitlow detection working");
                }
                //find a way to only activate this when jumping

            }
        }

        if (move.x < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left,- _horWallCheckDistance, _wallLayer);
            Vector2 dirlow = new Vector2(-_horWallCheckDistance, _vertWallCheckDistance).normalized;

            RaycastHit2D hitlow = Physics2D.Raycast(transform.position, dirlow, _horWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.x = 0;
            }
            if (hit.collider != null)
            {
                move.x = 0;
            }
            else if (hitlow.collider != null)
            {
                if (_isGrounded == false)
                {

                    move.x = 0;
                    Debug.Log("hitlow detection working");
                }

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
        //largely unnecessary this is a leftover from top down games, however, useful for wallclimbing
        if (move.y < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _vertWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;
            }
        }
        
        if(_isAttachedToWall == false)
        {
            Vector3 direction = new Vector3(move.x, 0, 0).normalized;
            transform.position += direction * _speed * Time.deltaTime;
        }
        //nothing sets this script to attach to a wall but like this is just me setting it up to like you move up the wall
        else if (_isAttachedToWall == true)
        {
            Vector3 direction = new Vector3(move.x, move.y, 0).normalized;
            transform.position += direction * _speed * Time.deltaTime;

        }

        if (_isGrounded == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _vertWallCheckDistance, _wallLayer);
            //to make it so the player can't fall off a ledge and jump
            if (hit.collider == null)
            {
                _isGrounded = false;
            }
        }
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
