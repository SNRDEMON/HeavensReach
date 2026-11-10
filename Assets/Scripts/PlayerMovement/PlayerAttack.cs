using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //can probably just grab these from the move script tbh it's less variables, but it seems simple to move em and I worry about needing to type that every time
    private Vector2 _moveHoriz;
    private Vector2 _vertMove;
    public void OnAimLeftRight(InputAction.CallbackContext context)
    {


        _moveHoriz = context.ReadValue<Vector2>();


    }
    public void OnAimUpDown(InputAction.CallbackContext context)
    {
        _vertMove = context.ReadValue<Vector2>();
    }

//    public void
}
