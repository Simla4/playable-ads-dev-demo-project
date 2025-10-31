using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class CheckOutArea : TaskBase, ITask
{
    [SerializeField] private float speed;
    
    private bool canContinue = true;
    

    public void StartTask()
    {
        StartCoroutine(TakeBaggageRoutine());
        Debug.Log("FillArea");
    }
    
    private IEnumerator TakeBaggageRoutine()
    {
        canContinue = true;
        Debug.Log("TakeBaggageRoutine");
        
        while (canContinue)
        {
            EventBus<OnGetPlaneEvent>.Emit(new OnGetPlaneEvent());
            
            yield return new WaitForSeconds(speed);
        }
    }

    public void StopTask()
    {
        canContinue = false;
    }
}
