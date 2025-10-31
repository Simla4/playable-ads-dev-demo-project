using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationsController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(string animationName, float speed)
    {
        animator.SetFloat(animationName, speed);
    }

    public void ChangeLayer(int layerIndex, float layerWeight)
    {
        animator.SetLayerWeight(layerIndex, layerWeight);
    }
}
