using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public abstract class FillableAreaBase : MonoBehaviour, IFillable
{
    [SerializeField] protected int cost;
    [SerializeField] private GameObject unlockedArea;
    
    private bool canContinueFilling = true;
    private Tween unlockTween;
    
    public void FillArea(int amount)
    {
        if(cost <= 0) return;
        
        StartCoroutine(FillAreaRoutine(amount));
    }

    private IEnumerator FillAreaRoutine(int amount)
    {
        canContinueFilling = true;
        
        while (canContinueFilling && CurencyManager.Instance.GetCurency() > 0)
        {
            yield return new WaitForSeconds(0.1f);
            cost -= amount;
            EventBus<CurencyManagementEvent>.Emit(new CurencyManagementEvent(-1));

            if (cost <= 0)
            {
                unlockedArea.SetActive(true);
                canContinueFilling = false;
            }
        }
    }
    

    public void StopFilling()
    {
        canContinueFilling = false;
    }

    private void UnlockAnim()
    {
        
    }
}
