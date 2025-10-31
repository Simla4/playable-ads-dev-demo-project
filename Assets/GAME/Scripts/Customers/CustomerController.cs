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
    private Tween scaleTween;
    private bool isDropedBaggage = false;
    private bool canFly = false;
    private bool inPlane = false;
    
    public bool IsDropedBaggage {set => isDropedBaggage = value; }
    public bool CanFly { get => canFly; set => canFly = value; }
    public bool InPlane { get => inPlane; set => inPlane = value; }

    public void Move(Transform targetPosition, bool isWalking = true)
    {
        DropBaggage();

        if (isWalking)
        {
            animationController.ResetAnimation("Idle");
            animationController.ChangeAnimation("Walk");
        }
        
        transform.rotation = Quaternion.Euler(0, targetPosition.rotation.eulerAngles.y, 0);
        
        float distance = Vector3.Distance(transform.position, targetPosition.position);
        float duration = distance / moveSpeed;

        if (moveTween != null)
        {
            moveTween.Kill();
        }
        

        moveTween = transform.DOMove(targetPosition.position, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if (inPlane)
                {
                    EventBus<OnCustomerArrivedEvent>.Emit(new OnCustomerArrivedEvent());
                    gameObject.SetActive(false);
                }
                NextTarget();
            });

        
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
        bool isWalking = targetIndex != 1;
        animationController.ChangeAnimation("Idle");
        
        if (targetIndex < targets.Count && isDropedBaggage)
        {
            Move(targets[targetIndex],  isWalking);
            targetIndex++;
        }
        else if(targetIndex >= targets.Count && isDropedBaggage)
        {
            EventBus<OnTargetListOverEvent>.Emit(new OnTargetListOverEvent());
        }
    }
}
