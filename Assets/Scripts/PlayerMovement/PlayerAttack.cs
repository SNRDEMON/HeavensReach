using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //can probably just grab these from the move script tbh it's less variables, but it seems simple to move em and I worry about needing to type that every time
    private Vector2 _moveHoriz;
    private Vector2 _vertMove;
    [SerializeField] private GameObject _upAttackHitBox;
    [SerializeField] private GameObject _leftAttackHitBox;
    [SerializeField] private GameObject _downAttackHitBox;
    [SerializeField] private GameObject _RightAttackHitBox;
    public void OnAimLeftRight(InputAction.CallbackContext context)
    {


        _moveHoriz = context.ReadValue<Vector2>();


    }
    public void OnAimUpDown(InputAction.CallbackContext context)
    {
        _vertMove = context.ReadValue<Vector2>();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        
        if (_vertMove.y >= 1f)
        {
            _upAttackHitBox.SetActive(true);
        }
    }
}
