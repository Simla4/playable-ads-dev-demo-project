using System.Collections;
using sb.eventbus;
using UnityEngine;

public class BaggageTransferArea : MonoBehaviour, IFillable
{
    [SerializeField] private float speed = 0.5f;
    
    private bool canContinueTakingBaggage = true;
    
    public void FillArea(int amount)
    {
        StartCoroutine(TakeBaggageRoutine());
    }

    private IEnumerator TakeBaggageRoutine()
    {
        canContinueTakingBaggage = true;
        
        while (canContinueTakingBaggage)
        {
            EventBus<BaggageTransferEvent>.Emit(new BaggageTransferEvent());
            
            yield return new WaitForSeconds(speed);
        }
    }

    public void StopFilling()
    {
        canContinueTakingBaggage = false;
    }
}
