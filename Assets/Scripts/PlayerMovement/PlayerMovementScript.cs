using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{

    private float _speed = 5f;
    private float _runSpeed = 8f;
    private float _crouchSpeed = 2.5f;
    private float _walkSpeed = 5f;
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
    private Vector2 _vertMove;

    //Some Stuff for the sprites
    [Header("Manual Setup For Sprites")]
    private Rigidbody2D _playerRB;
    private CapsuleCollider2D _playerCollider;
    private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private GameObject _wingObject;
    [SerializeField] private SpriteRenderer _wingSpriteRenderer;
    //probably a better way to do this with like a for loop or coroutine but idk I'm lazy
    //yeah rereading this half this shit needs to be on coroutines instead of if statements I just didn't want to impliment them
    private float _jumpCooldown = 0f;
    private bool _isUnableToUncrouch = false;
    [Header("Dash Stuff")]
    //dash variables
    [SerializeField] private float _dashDistance = 5f;
    public float _dashCooldown = 0f;
    private float _dashAirTimeLeft = 0f;
    private bool _isDashing = false;
    [SerializeField] private float _dashAirTimeSet = 1f;
    [SerializeField] private float _ledgeNudgeAmount = 0.05f;
    [SerializeField] private float _ledgeNudgeCooldown = 0.1f;
    private float _ledgeNudgeTimer;

    [Header("Slide stuff")]
    [SerializeField] private float _slideTime = 1f;
    [SerializeField] private float _slideDistance = 6f;

    [Header("Sprint stuff")]
    [SerializeField] private bool _isSprinting;

    [Header("AirStrafe Stuff")]
    [SerializeField] private float _jumpStrafeDrag = 0.8f;
    [SerializeField] private float _jumpStrafeSpeedUp = 1.2f;
    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<CapsuleCollider2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private bool _xMoveButtonHeldDown;
    public void OnMove(InputAction.CallbackContext context)
    {


            move = context.ReadValue<Vector2>();
            _xMoveButtonHeldDown = Mathf.Abs(move.x) > 0.01f;
        

    }
    public void OnClimb(InputAction.CallbackContext context)
    {
        _vertMove = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting = true;
            _speed = _runSpeed;
        }


        if (context.canceled)
        {
            _isSprinting = false;
            _speed = _walkSpeed;
        }

    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (_dashCooldown <= 0f)
        {
            if (!_isCrouched)
            {
                if (move.x < 0f)
                {
                     _playerRB.linearVelocityX = 0f;       
                    _playerRB.linearVelocityY = 0f;
                    KeepAirDirection();
                    _playerRB.AddForce(Vector2.left * _dashDistance, ForceMode2D.Impulse);
                    _dashCooldown = 1f;
                    _dashAirTimeLeft = _dashAirTimeSet;
                    _isDashing = true;
                    _playerRB.gravityScale = 0.1f;
                   
                    Debug.Log("Tried To dash1");
                }
                else if (move.x > 0f)
                {
                    _playerRB.linearVelocityX = 0f;
                    _playerRB.linearVelocityY = 0f;
                    KeepAirDirection();
                    _playerRB.AddForce(Vector2.right * _dashDistance, ForceMode2D.Impulse);
                    _dashCooldown = 1f;
                    _dashAirTimeLeft = _dashAirTimeSet;
                    _isDashing = true;
                   _playerRB.gravityScale = 0.1f;
 
                    Debug.Log("Tried To dash2");
                }
            }
            Debug.Log("Tried To dash3");
        }




    }
    //is a mess work on this
    private bool _isSliding = false;
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!_isSprinting)
            {

                if (_isCrouched == false)
                {
                    _playerRB.MovePosition(_playerRB.position + Vector2.down * .7f);
                    _speed = _crouchSpeed;
                    _isCrouched = true;
                    _playerCollider.size = new Vector2(0.8f, 0.8f);
                }
                //put a crouch function here
                Debug.Log("CrouchStarted");
            }

            //put a cooldown and duration for these
            else if (_isSprinting)
            {
                if (CheckForGround())
                {
                    if (move.x < 0f)
                    {
                        _playerRB.MovePosition(_playerRB.position + Vector2.down * .7f);
                        _playerRB.AddForce(Vector2.left * _slideDistance, ForceMode2D.Impulse);
                        _isSliding = true;
                        _isCrouched = true;
                        _playerCollider.size = new Vector2(0.8f, 0.8f);
                    }
                    else if (move.x > 0f)
                    {
                        _playerRB.MovePosition(_playerRB.position + Vector2.down * .7f);
                        _playerRB.AddForce(Vector2.right * _slideDistance, ForceMode2D.Impulse);
                        _isSliding = true;
                        _isCrouched = true;
                        _playerCollider.size = new Vector2(0.8f, 0.8f);
                    }
                }

            }
        }
     if (context.canceled)
     {
        Debug.Log("UncrouchedScriptRun");
         if (_isCrouched == true && _isUnableToUncrouch == false)
        {
                    _playerRB.MovePosition(_playerRB.position + Vector2.up * .8f);
                   _speed = _walkSpeed;
                    _isCrouched = false;
                    _isSliding = false;
                    _playerCollider.size = new Vector2(1, 1.9f);
                    
         }

                //put uncrouch functions here
     }

        
    



}
    //Note to self, don't keep these as hardcoded make them variables later as I clean this up

    //Note: Can currently stand up in crouched spaces. Could lead to clipping

    private bool _jumpMovementGraceActive = false;
    private float _jumpMovementGrace;
    //this gives time for you to change direction as you jump, adjust as necessary
    private float _jumpMovementGraceDuration = 0.1f;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded == true && (_jumpCooldown <= 0))
        {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpCooldown = 1f;
            _isGrounded = false;
            _jumpMovementGraceActive = true;
            _jumpMovementGrace = _jumpMovementGraceDuration;

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
                    if (hit.collider.TryGetComponent(out StickyWall stick))
                    {
                        if (!CheckForGround())
                        {

                            _isAttachedToWall = true;
                        }
                    }
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
        
        _isAttachedToWall = false;
        return false;
    }
    private Vector2[] TopRayOrigins = new Vector2[3];
    private bool CheckForCeiling()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        bool lefthit = false;
        bool centerhit = false;
        bool righthit = false;
        TopRayOrigins[0] = new Vector2(bounds.min.x, bounds.max.y);
        TopRayOrigins[1] = new Vector2(bounds.center.x, bounds.max.y);
        TopRayOrigins[2] = new Vector2(bounds.max.x, bounds.max.y);
        for (int i = 0; i < TopRayOrigins.Length; i++)
        {
            Vector2 v = TopRayOrigins[i];
            RaycastHit2D hit = Physics2D.Raycast(v, Vector2.up, _horWallCheckDistance, _wallLayer);
            Debug.DrawRay(v, Vector2.up * _horWallCheckDistance, Color.green);
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
    public bool _inAirLeft = false; //reset to private later
    private bool _inAirRight = false;

    //function for checking what direction the player is moving for the sake of moving back and forth while in air
    private float _jumpDirection;
    private void KeepAirDirection()
    {
        _jumpDirection = Mathf.Sign(move.x);
    }
    private void AttachToWall()
    {
        _playerRB.gravityScale = 0f;
    }

    //shove more of the update stuff in here maybe
    private void MovePlayerWithPhysicsHorizontal(float velocityX)
    {
        _playerRB.linearVelocity = new Vector2(velocityX, _playerRB.linearVelocity.y);
    }
    //I could probably move all these raycasts to a function that returns them I think instead of this (Update, working on it)
    private void Update()
    {
        if (_dashAirTimeLeft <= 0f)
        {
            _isDashing = false;
        }
        if (!_isDashing && !_isSliding)
        {
            if (move.x > 0f)
            {
                _playerSpriteRenderer.flipX = false;
                _wingSpriteRenderer.flipX = false;

                _wingObject.transform.localPosition = new Vector3 (-0.868f, 0,0);
                
            }

            else if (move.x < 0f)
            {
                _playerSpriteRenderer.flipX = true;
                _wingSpriteRenderer.flipX = true;

                _wingObject.transform.localPosition = new Vector3(0.868f, 0, 0);
            }

            if (_isGrounded || _jumpMovementGraceActive || _isAttachedToWall)
            {
                float velocityX = move.x * _speed;
                KeepAirDirection();
               // Debug.Log(move.x);
                if (CheckForWalls(move.x))
                {
                    velocityX = 0f;

                }
                MovePlayerWithPhysicsHorizontal(velocityX);
            }
            if (!_isGrounded && !_jumpMovementGraceActive)
            {
                if (!_xMoveButtonHeldDown)
                {

                    float velocityX = _playerRB.linearVelocityX;
                    velocityX = Mathf.MoveTowards(velocityX, 0f, _jumpStrafeDrag * Time.deltaTime);
                    if (CheckForWalls(1))
                    {
                        velocityX = 0f;
                    }

                    if (CheckForWalls(-1))
                    {
                        velocityX = 0f;
                    }
                    MovePlayerWithPhysicsHorizontal(velocityX);
                }
                //this one is so long I could probably shorten it but whatever at this point

                
                if (_xMoveButtonHeldDown && _jumpDirection != 0f)
                {
                    float inputDirection = Mathf.Sign(move.x);
                   
                    if (inputDirection != 0f)
                    {
                        float velocityX = _playerRB.linearVelocityX;
                            
                        if (inputDirection != _jumpDirection)
                        {
                           velocityX = Mathf.MoveTowards(velocityX, inputDirection * 15f * 0.9f, _jumpStrafeDrag * Time.deltaTime); //replace these with different things, like the 15f with a max velocity
                  
                        }
                        if (inputDirection == _jumpDirection)
                        {
                            velocityX = move.x * _speed;
                            if (CheckForWalls(move.x))
                            {
                                velocityX = 0f;

                            }

                        }
                        MovePlayerWithPhysicsHorizontal(velocityX);
                    }
                  /*  if (_inAirLeft)
                    {
                        if (move.x > 0f)
                        {
                        }
                        else if (move.x < 0f)
                        {
                            
                            float velocityX = _playerRB.linearVelocityX * 1.02f;

                        }
                    }
                    if (_inAirRight)
                    {
                        float velocityX = move.x * _speed;
                    } */

                }
            }
        }
        if (_isAttachedToWall == true)
        {
            float velocityY = _vertMove.y * _speed;
            KeepAirDirection();
            _playerRB.linearVelocity = new Vector2(_playerRB.linearVelocity.x, velocityY);
        }
        
            

       /* if (move.y > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, _vertWallCheckDistance, _wallLayer);
            if (hit.collider != null)
            {
                move.y = 0;


            }
        }
        //largely unnecessary this is a leftover from top down games, however, useful for wallclimbing
       /* if (move.y < 0)
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


        CheckForCeiling();
        if (CheckForCeiling() == true)
        {
            _isUnableToUncrouch = true;
        }
        else if (CheckForCeiling() == false)
        {
            _isUnableToUncrouch = false;
        }

        CheckForGround();
        if (CheckForGround() == true)
        {
            if (!_jumpMovementGraceActive)
            {
                _inAirLeft = false;
                _inAirRight = false;
            }

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
        if (_dashAirTimeLeft <= 0)
        {
            _playerRB.gravityScale = 1f;
        }
        if (_jumpMovementGrace <= 0)
        {
            _jumpMovementGraceActive = false;

        }
  
        //bad way to do a cooldown but this is here because you could jump twice before you could get off the ground so you'll want to replace it with a coroutine probably
        _jumpCooldown -= Time.deltaTime;
        _dashAirTimeLeft -= Time.deltaTime;
        _ledgeNudgeTimer -= Time.deltaTime;
        _dashCooldown -= Time.deltaTime;
        _jumpMovementGrace -= Time.deltaTime;
        }
    //some code from my old game
}
