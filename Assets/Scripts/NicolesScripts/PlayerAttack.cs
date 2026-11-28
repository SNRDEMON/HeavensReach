using System.Collections;
using System.Timers;
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
    [SerializeField] private GameObject _rightAttackHitBox;
    private GameObject _currentUsedAttack;
    private Coroutine _attackCoroutine;
    private float _attackDuration = 0.5f;
    private bool _isAttacking  = false;
    private int _defaultDirection;
    public void OnAimLeftRight(InputAction.CallbackContext context)
    {


        _moveHoriz = context.ReadValue<Vector2>();


    }
    public void OnAimUpDown(InputAction.CallbackContext context)
    {
        _vertMove = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        if (_moveHoriz.x < -0.1f)
        {
            _defaultDirection = 1;
        }
        else if (_moveHoriz.x > 0.1f)
        {
            _defaultDirection = -1;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_isAttacking == false)
        {
            if (_vertMove.y == 1f)
            {
                _currentUsedAttack = _upAttackHitBox;
                _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
            }
            else if (_vertMove.y == -1f)
            {
                _currentUsedAttack = _downAttackHitBox;
                _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
            }
            else if (_vertMove.y == 0f)
            {
                if (_moveHoriz.x == 1f)
                {
                    _currentUsedAttack = _rightAttackHitBox;
                    _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
                }
                else if (_moveHoriz.x == -1f)
                {
                    _currentUsedAttack = _leftAttackHitBox;
                    _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
                }
                else if (_moveHoriz.x == 0f)
                {
                    if (_defaultDirection == -1)
                    {
                        _currentUsedAttack = _rightAttackHitBox;
                        _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
                    }
                    else if (_defaultDirection == 1)
                    {
                        _currentUsedAttack = _leftAttackHitBox;
                        _attackCoroutine = StartCoroutine(AttackCoro(_attackDuration));
                    }
                }
            }
        }

    }
    private IEnumerator AttackCoro(float duration)
    {
        _isAttacking = true;
        _currentUsedAttack.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();

        }
        _isAttacking = false;
        _currentUsedAttack.SetActive(false);
    }
}
