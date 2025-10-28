using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using Unity.VisualScripting;
using UnityEngine;

public abstract class FillableAreaBase : MonoBehaviour, IFillable
{
    [SerializeField] protected int cost;
    [SerializeField] private GameObject unlockedArea;
    
    private bool canContinueFilling = true;
    private Tween unlockTween;
    
    public void FillArea(int amount)
    {
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
