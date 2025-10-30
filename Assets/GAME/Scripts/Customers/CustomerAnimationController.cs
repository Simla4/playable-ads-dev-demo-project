using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CustomerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }
}
