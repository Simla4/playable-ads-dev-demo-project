using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class BaggageTakeArea : MonoBehaviour, IFillable
{
    [SerializeField] private float speed;
    
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
            EventBus<OnTakeBaggageEvent>.Emit(new OnTakeBaggageEvent());
            
            yield return new WaitForSeconds(speed);
        }
    }

    public void StopFilling()
    {
        canContinueTakingBaggage = false;
    }
}
