using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public class BaggageTruck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform parenTransform;
    
    [Header("Settings")]
    [SerializeField] private float duration = 1.0f;
    
    private List<GameObject> baggages = new List<GameObject>();
    private EventListener<AllBaggagesTransferredEvent> onAllBaggagesTransferred;
    private bool canMove = true;
    private Vector3 startPosition;
    private Tween moveTween;


    private void OnEnable()
    {
        onAllBaggagesTransferred = new EventListener<AllBaggagesTransferredEvent>(Move);
        EventBus<AllBaggagesTransferredEvent>.AddListener(onAllBaggagesTransferred);
    }

    private void OnDisable()
    {
        EventBus<AllBaggagesTransferredEvent>.RemoveListener(onAllBaggagesTransferred);
    }


    public void AddBag(GameObject baggage)
    {
        baggages.Add(baggage);
        baggage.transform.SetParent(transform);
    }

    public int GetBagCount()
    {
        return baggages.Count;
    }

    private void Move(AllBaggagesTransferredEvent e)
    {
        if(!canMove) return;
        
        canMove = false;
        startPosition = parenTransform.position;
        
        if (moveTween != null)
        {
            moveTween.Kill();
        }
        
        moveTween = parenTransform.DOMove(targetPosition.position, duration).SetEase(Ease.Linear).SetDelay(1f)
            .OnComplete(()=>UnloadTruck());
    }

    private void UnloadTruck()
    {
        for (int i = 0; i < baggages.Count; i++)
        {
            baggages[i].SetActive(false);
        }

        if (moveTween != null)
        {
            moveTween.Kill();
        }
        
        moveTween = parenTransform.DOMove(startPosition, duration).SetEase(Ease.Linear);
    }
}
