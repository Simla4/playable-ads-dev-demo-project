using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Baggage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform liftTransform;
    [SerializeField] private Transform liftTargetTransform;
    [SerializeField] private BaggageTruck baggageTruck;
    
    [Header("Settings")]
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float liftHeight = 4f;
    [SerializeField] private float liftDuration = 1f;
    
    private bool inThePlayer = false;
    private Tween moveTween;
    private Tween transferLift;
    private Tween transferTruck;
    private Tween liftTween;
    
    public bool InThePlayer { get => inThePlayer; set => inThePlayer = value; }

    public void Move(Transform target, float duration)
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
        
        moveTween = transform.DOMove(target.position, duration).SetEase(Ease.Linear).OnComplete(()=>TransferLift());
    }

    private void TransferLift()
    {
        if (transferLift != null)
        {
            transferLift.Kill();
        }
        
        transferLift = transform.DOJump(liftTargetTransform.position, jumpPower, 1, moveDuration).OnComplete(()=> LiftAnimation());
        transform.parent = liftTransform;

    }

    private void LiftAnimation()
    {
        if (liftTween != null)
        {
            liftTween.Kill();
        }
        
        liftTween = liftTransform.DOMoveY(liftTransform.position.y + liftHeight, liftDuration).SetEase(Ease.Linear)
            .OnComplete(()=>liftTransform.DOMoveY(liftTransform.position.y - liftHeight, liftDuration));
        
        TransferTruck();
    }

    private void TransferTruck()
    {
        if (transferTruck != null)
        {
            transferTruck.Kill();
        }

        var targetPos = baggageTruck.transform.position + Vector3.up * 0.5f * baggageTruck.GetBagCount();
        
        transferTruck = transform.DOJump(targetPos, jumpPower, 1, moveDuration).SetEase(Ease.Linear).SetDelay(liftDuration).OnComplete(()=>baggageTruck.AddBag(gameObject));
        
    }
}
