using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    private float _speed = 5f;
    private float _runSpeed = 5f;
    private float _crouchSpeed = 2.5f;
    private bool _isCrouched = false;
    [Header("Wall checking Stuff")]
    [SerializeField] private float _vertWallCheckDistance = 1f;
    [SerializeField] private float _horWallCheckDistance = 1f;
    //temp variable

    //probably move this away from being layers later but idk it works for walls

    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _jumpForce = 1f;
    private bool _isGrounded = true;
    //use below for wallclimbing
    private bool _isAttachedToWall = false;
    private Vector2 move;
    private Rigidbody2D _playerRB;
    private CapsuleCollider2D _playerCollider;
    //probably a better way to do this with like a for loop or coroutine but idk I'm lazy
    //yeah rereading this half this shit needs to be on coroutines instead of if statements I just didn't want to impliment them
    private float _jumpCooldown = 0f;


    //dash variables
    [SerializeField] private float _dashDistance = 5f;
    public float _dashCooldown = 0f;
    private float _dashAirTimeLeft = 0f;
    private bool _isDashing = false;
    [SerializeField] private float _dashAirTimeSet = 1f;
    [SerializeField] private float _ledgeNudgeAmount = 0.05f;
    [SerializeField] private float _ledgeNudgeCooldown = 0.1f;
    private float _ledgeNudgeTimer;


    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<CapsuleCollider2D>();
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
                _dashAirTimeLeft = _dashAirTimeSet;
                _isDashing = true;
                //    _playerRB.gravityScale = 0f;
            }
            else if (move.x > 0f)
            {
                _playerRB.AddForce(Vector2.right * _dashDistance, ForceMode2D.Impulse);
                _dashCooldown = 1f;
                _dashAirTimeLeft = _dashAirTimeSet;
                _isDashing = true;
                //    _playerRB.gravityScale = 0f;
            }
        }




    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        _speed = _crouchSpeed;
        if (_isCrouched == false)
        {
            _speed = _crouchSpeed;
            _isCrouched = true;
            _playerCollider.size = new Vector2(0.8f, 0.8f);
        }
        else if (_isCrouched == true)
        {

            _speed = _runSpeed;
            _isCrouched = false;
            _playerCollider.size = new Vector2(1,2); //Note to self, don't keep these as hardcoded make them variables later as I clean this up
    }
        }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded == true && (_jumpCooldown <= 0))
        {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpCooldown = 1f;
            _isGrounded = false;

        }

    }
    private Vector2[] SideRayOrigins = new Vector2[4];  
  
    private bool CheckForWalls(float moveX)
    {
        if (moveX == 0)
            return false;

        Vector2 direction = moveX > 0
            ? Vector2.right
            : Vector2.left;

        Bounds bounds = GetComponent<Collider2D>().bounds;

        bool centerhit =false;
        bool tophit = false;
        bool bottomhit = false;
        bool lowerhit = false;
        SideRayOrigins[0] = new Vector2(bounds.center.x, bounds.center.y);
        SideRayOrigins[1] = new Vector2(bounds.center.x, bounds.max.y);
        SideRayOrigins[2] = new Vector2(bounds.center.x, bounds.min.y);
        SideRayOrigins[3] = new Vector2(bounds.center.x, bounds.min.y + 0.1f); //probs set this 0.1f to a variable later
   

        for (int i = 0; i < SideRayOrigins.Length;  i++)
        {
            Vector2 v = SideRayOrigins[i];

            RaycastHit2D hit = Physics2D.Raycast(v, direction, _horWallCheckDistance, _wallLayer);

            Debug.DrawRay(v, direction * _horWallCheckDistance, Color.green);

            if (hit.collider != null)
            {
                if (i == 0)
                {
                    centerhit = true;

                }
                if (i == 1)
                {
                    tophit = true;
                }
                if (i == 2)
                {
                    bottomhit = true;
                }
                if (i == 3)
                {
                    lowerhit = true;
                }
            }
        }
        if (bottomhit && !lowerhit && !centerhit && !tophit)
        {
            if (_ledgeNudgeTimer <= 0f)
            { 
                _playerRB.position += Vector2.up * _ledgeNudgeAmount;
                _ledgeNudgeTimer = _ledgeNudgeCooldown;
            }
            return false;
        }

        if (centerhit|| lowerhit || tophit)
        {
            return true;
        }
        return false;
    }
    //making the ground check also be an array too, but it's less important
    private Vector2[] BottomRayOrigins = new Vector2[3];
    private bool CheckForGround()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        bool lefthit = false;
        bool centerhit = false;
        bool righthit = false;
        BottomRayOrigins[0] = new Vector2(bounds.min.x, bounds.min.y);
        BottomRayOrigins[1] = new Vector2(bounds.center.x, bounds.min.y);
        BottomRayOrigins[2] = new Vector2(bounds.max.x, bounds.min.y);
        for (int i = 0; i < BottomRayOrigins.Length; i++)
        {
            Vector2 v = BottomRayOrigins[i];
            RaycastHit2D hit = Physics2D.Raycast(v, Vector2.down, _horWallCheckDistance, _wallLayer);
            Debug.DrawRay(v, Vector2.down * _horWallCheckDistance, Color.green);
            if (hit.collider != null)
            {
                if (i == 0)
                {
                    lefthit = true;

                }
                if (i == 1)
                {
                    centerhit = true;
                }
                if (i == 2)
                {
                    righthit = true;
                }
            }
                
        }
        if (centerhit || lefthit || righthit)
        {
            return true;
        }
        return false;

}
    //I could probably move all these raycasts to a function that returns them I think instead of this (Update, working on it)
    private void Update()
    {
        if (_dashAirTimeLeft <= 0f)
        {
            _isDashing = false;
        }
        if (!_isDashing)
        {
            float velocityX = move.x * _speed;
            if (CheckForWalls(move.x))
            {
                velocityX = 0f;

            }
            _playerRB.linearVelocity = new Vector2(velocityX, _playerRB.linearVelocity.y);
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
        /*
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
        */
        CheckForGround();
        if (CheckForGround() == true)
        {
            if (_isGrounded == false && (_jumpCooldown <= 0))
            {
                _isGrounded = true;
            }
               // Debug.Log("check for ground array working");
        }
        else if (CheckForGround() == false)
        {
            if (_isGrounded == true)
            {
                _isGrounded = false;
            }
               // Debug.Log("This is the false return for jump");
        }
  /*      if (_dashAirTimeLeft <= 0)
        {
            _playerRB.gravityScale = 1f;
        }

  */
        //bad way to do a cooldown but this is here because you could jump twice before you could get off the ground so you'll want to replace it with a coroutine probably
        _jumpCooldown -= Time.deltaTime;
        _dashAirTimeLeft -= Time.deltaTime;
        _ledgeNudgeTimer -= Time.deltaTime;
        _dashCooldown -= Time.deltaTime;
        }
    //some code from my old game
}
