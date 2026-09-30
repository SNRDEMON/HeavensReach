using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class FSM : MonoBehaviour
{
    //private float _timer = 0;
   [SerializeField] private bool isCrouched = false;
    private Animator _anim;
    private Animator _wingAnim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _wingAnim = GetComponentInChildren<Animator>();
    }

    public void OnMovePlay(InputAction.CallbackContext context)
    {
        if (context.started)
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
        }
    }

    public void OnCrouchPlay(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Play crouch down 
          
            isCrouched = !isCrouched;
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
