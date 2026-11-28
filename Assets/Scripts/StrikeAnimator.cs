using UnityEngine;

public class StrikeAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        //Maybe add a small delay for arm movement animation
        animator.Play("SlashFX");
    }

    private void OnDisable()
    {
        animator.Play("Empty");
    }
}



