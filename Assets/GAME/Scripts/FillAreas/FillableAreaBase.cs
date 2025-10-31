using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class FillableAreaBase : MonoBehaviour, IFillable
{
    [Header("References")]
    [SerializeField] protected FillableAreaData fillableAreaData;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private List<GameObject> unlockAreas;
    [SerializeField] private Image progressImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject arrowGameObject;
    
    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float duration = 0.5f;
    
    private bool canContinueFilling = true;
    private Tween unlockTween;
    private Tween canvasTween;
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
            for (int i = 0; i < unlockAreas.Count; i++)
            {
                unlockAreas[i].transform.localScale = Vector3.one;
            }
            
            canvasGroup.gameObject.SetActive(false);
            
            EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType));
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
        
        CanvasAnimation(1);
        
        while (canContinueFilling && CurencyManager.Instance.GetCurency() > 0)
        {
            yield return new WaitForSeconds(0.1f);
            cost -= amount;
            EventBus<CurencyManagementEvent>.Emit(new CurencyManagementEvent(-1));
            costText.text = cost.ToString();
            FillProgressBar();

            if (cost <= 0)
            {
                StartCoroutine(UnlockAnim());
                arrowGameObject.SetActive(false);
                CanvasAnimation(0);
                break;
            }
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

    private IEnumerator UnlockAnim()
    {
        for (var i = 0; i < unlockAreas.Count; i++)
        {
            if (unlockTween != null)
            {
                unlockTween.Kill();
            }
            
            unlockTween = unlockAreas[i].transform.DOScale(Vector3.one, duration)
                .SetEase(Ease.OutBounce)
                .OnComplete(()=>
                    EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType)));
            
            yield return new WaitForSeconds(duration);
        }
    }
    
    private void CanvasAnimation(float endValue)
    {
        if (canvasTween != null)
        {
            canvasTween.Kill();
        }
        
        canvasTween = canvasGroup.DOFade(endValue, fadeDuration);
    }
}
