using System.Collections;
using sb.eventbus;
using UnityEngine;

public class BaggageTransferArea : TaskBase, ITask
{
    [SerializeField] private float speed = 0.5f;
    
    private bool canContinueTakingBaggage = true;
    
    public void StartTask()
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

    public void StopTask()
    {
        canContinueTakingBaggage = false;
    }
}
