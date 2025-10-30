using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private CustomerAnimationController animationController;
    [SerializeField] private Baggage baggage;
    
    private int targetIndex = 0;
    private Tween moveTween;
    private bool isDropedBaggage = false;
    private bool canFly = false;
    
    public bool IsDropedBaggage {set => isDropedBaggage = value; }
    public bool CanFly { get => canFly; set => canFly = value; }

    public void Move(Transform targetPosition)
    {
        DropBaggage();
        
        animationController.ChangeAnimation("Walk");
        transform.rotation = Quaternion.Euler(0, targetPosition.rotation.eulerAngles.y, 0);
        
        float distance = Vector3.Distance(transform.position, targetPosition.position);
        float duration = distance / moveSpeed;

        if (moveTween != null)
        {
            moveTween.Kill();
        }
        

        moveTween = transform.DOMove(targetPosition.position, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => NextTarget());
    }

    private void DropBaggage()
    {
        if (isDropedBaggage && !baggage.InThePlayer)
        {
            baggage.InThePlayer = true;
            EventBus<TakeBaggageEvent>.Emit(new TakeBaggageEvent(baggage));
        }
    }

    private void NextTarget()
    {
        animationController.ChangeAnimation("Idle");
        
        if (targetIndex < targets.Count - 1 && isDropedBaggage)
        {
            Move(targets[targetIndex]);
            targetIndex++;
        }
        else if(targetIndex >= targets.Count - 1 && isDropedBaggage)
        {
            EventBus<OnTargetListOverEvent>.Emit(new OnTargetListOverEvent());
        }
    }
}
