using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class FSM : MonoBehaviour
{
    //private float _timer = 0;
   [SerializeField] private bool isCrouched = false;
    private Animator _anim;
    private Animator _wingAnim;
    [SerializeField] private PlayerMovementScript _playerMovementScript;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _wingAnim = GetComponentInChildren<Animator>();
    }

    public void OnMovePlay(InputAction.CallbackContext context)
    {
     /*   if (context.started)
        {
            if (isCrouched == true)
            {

                _anim.Play("Crouch");
            }
            else
                _anim.Play("Walk");
        }
        else if (context.canceled)
        {
            if (isCrouched == true)
            {

                _anim.Play("CrouchIdle");
            }
            else
                _anim.Play("Idle");
            _wingAnim.Play("WingFlap");
        } */
    }
    public void Update()
    {
        if (_playerMovementScript._isWalking && !_playerMovementScript._isCrouched)
        {
            _anim.Play("Walk");
        }
        else if (!_playerMovementScript._isWalking && !_playerMovementScript._isCrouched)
        {
            _anim.Play("Idle");
        }

        else if (_playerMovementScript._isCrouched && _playerMovementScript._isWalking)
        {
            _anim.Play("Crouch");
        }
        else if (_playerMovementScript._isCrouched && !_playerMovementScript._isWalking)
        {
            _anim.Play("CrouchIdle");
        }

    }
    public void OnCrouchPlay(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Play crouch down 
          
          //  isCrouched = !isCrouched;
        }
       
    }

    public void OnJumpPlay(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _anim.Play("JumpStart");
        }
    }
}
