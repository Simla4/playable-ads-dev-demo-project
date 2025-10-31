using System;
using System.Collections;
using System.Collections.Generic;
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
        baggages.Add(e.baggage);
        e.baggage.transform.SetParent(gameObject.transform.parent);
        e.baggage.transform.position = targetBaggageTransfom.position + Vector3.up * spacing * baggages.Count;
        e.baggage.transform.rotation = targetBaggageTransfom.rotation;
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
