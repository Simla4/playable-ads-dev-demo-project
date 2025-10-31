using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public class BaggageDropArea : TaskBase, ITask
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float spacing;
    
    [Header("References")]
    [SerializeField] private Transform targetBaggageTransfom;
    [SerializeField] private Transform endTransform;
    
    private bool canContinueTakingBaggage = true;
    private List<Baggage> baggages = new List<Baggage>();
    private EventListener<PlaceToDeviceEvent> onPlaceToDevice;
    private EventListener<BaggageTransferEvent> onBaggageTransfered;
    private Tween jumpTween;
    private Tween rotateTween;


    private void OnEnable()
    {
        onPlaceToDevice = new EventListener<PlaceToDeviceEvent>(PlaceBaggages);
        EventBus<PlaceToDeviceEvent>.AddListener(onPlaceToDevice);

        onBaggageTransfered = new EventListener<BaggageTransferEvent>(TransferBaggage);
        EventBus<BaggageTransferEvent>.AddListener(onBaggageTransfered);
    }

    private void OnDisable()
    {
        EventBus<PlaceToDeviceEvent>.RemoveListener(onPlaceToDevice);
        EventBus<BaggageTransferEvent>.RemoveListener(onBaggageTransfered);
    }

    private IEnumerator TakeBaggageRoutine()
    {
        canContinueTakingBaggage = true;
        
        while (canContinueTakingBaggage)
        {
            EventBus<DropBaggageEvent>.Emit(new DropBaggageEvent());
            
            yield return new WaitForSeconds(speed);
        }
    }

    public void StartTask()
    {
        StartCoroutine(TakeBaggageRoutine());
    }

    public void StopTask()
    {
        canContinueTakingBaggage = false;
    }

    private void PlaceBaggages(PlaceToDeviceEvent e)
    {
        if (jumpTween != null)
        {
            jumpTween.Kill();
        }

        if (rotateTween != null)
        {
            rotateTween.Kill();
        }
        
        baggages.Add(e.baggage);
        e.baggage.transform.SetParent(gameObject.transform.parent);
        jumpTween = e.baggage.transform.DOJump(targetBaggageTransfom.position + Vector3.up * spacing * baggages.Count, 0.5f, 1, 0.25f).SetEase(Ease.Linear);
        rotateTween = e.baggage.transform.DORotateQuaternion(targetBaggageTransfom.rotation, 0.25f);
    }

    private void TransferBaggage(BaggageTransferEvent e)
    {
        if (baggages.Count <= 0)
        {
            EventBus<AllBaggagesTransferredEvent>.Emit(new AllBaggagesTransferredEvent());
            return;
        }
        
        var targetBaggage = baggages[0];
        targetBaggage.Move(endTransform, speed);
        baggages.RemoveAt(0);

        if (baggages.Count <= 0)
        {
            EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
        }

        for (int i = 0; i < baggages.Count; i++)
        {
            baggages[i].transform.position += Vector3.down * spacing;
        }
    }
}
