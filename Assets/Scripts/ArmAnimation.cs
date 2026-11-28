using UnityEngine;

public class ArmAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("SlashArm");
    }

    private void OnDisable()
    {
        animator.Play("NoArm");
    }
}


