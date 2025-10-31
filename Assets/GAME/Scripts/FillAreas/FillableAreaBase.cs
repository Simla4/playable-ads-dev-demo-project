using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class FillableAreaBase : MonoBehaviour, IFillable
{
    [SerializeField] protected FillableAreaData fillableAreaData;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject unlockedArea;
    [SerializeField] private Image progressImage;
    
    private bool canContinueFilling = true;
    private Tween unlockTween;
    protected int cost;
    protected FillableAreaTypes areaType;

    protected void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (cost <= 0)
        {
            unlockedArea.SetActive(true);
        }
        costText.text = cost.ToString();
    }

    public virtual void FillArea(int amount)
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
            costText.text = cost.ToString();
            FillProgressBar();

            if (cost <= 0)
            {
                unlockedArea.SetActive(true);
                EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType));
                break;
            }
        }

        if (cost <= 0)
        {
            unlockedArea.SetActive(true);EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType));
        }
    }

    private void FillProgressBar()
    {
        var targetCost = fillableAreaData.targetCost;
        progressImage.fillAmount = (float)(targetCost - cost)/targetCost;
    }
    

    public virtual void StopFilling()
    {
        canContinueFilling = false;
    }

    private void UnlockAnim()
    {
        
    }
}
