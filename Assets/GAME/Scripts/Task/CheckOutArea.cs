using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class CheckOutArea : MonoBehaviour, IFillable
{
    [SerializeField] private float speed;
    [SerializeField] private int prize;
    
    private bool canContinue = true;
    
    public void FillArea(int amount)
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
            EventBus<CurencyManagementEvent>.Emit(new CurencyManagementEvent(prize));
            
            yield return new WaitForSeconds(speed);
        }
    }

    public void StopFilling()
    {
        canContinue = false;
    }
}
