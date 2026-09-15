using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class FSM : MonoBehaviour
{
    private float _timer = 0;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OnMovePlay(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _anim.Play("Walk");
        }
        else if (context.canceled)
        {
            _anim.Play("Idle");
        }
    }
}
