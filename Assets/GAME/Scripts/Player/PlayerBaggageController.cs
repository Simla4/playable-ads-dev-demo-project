using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class PlayerBaggageController : MonoBehaviour
{
    [SerializeField] private Transform targetBaggagePosition;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private PlayerAnimationsController playerAnimationsController;
    
    private List<Baggage> baggages = new List<Baggage>();
    private EventListener<TakeBaggageEvent> onTakeBaggage;
    private EventListener<DropBaggageEvent> onDropBaggage;

    private void OnEnable()
    {
        onTakeBaggage = new EventListener<TakeBaggageEvent>(TakeBaggage);
        EventBus<TakeBaggageEvent>.AddListener(onTakeBaggage);
        
        onDropBaggage = new EventListener<DropBaggageEvent>(DropBaggage);
        EventBus<DropBaggageEvent>.AddListener(onDropBaggage);
    }

    private void OnDisable()
    {
        EventBus<TakeBaggageEvent>.RemoveListener(onTakeBaggage);
        EventBus<DropBaggageEvent>.RemoveListener(onDropBaggage);
    }


    private void TakeBaggage(TakeBaggageEvent e)
    {
        playerAnimationsController.ChangeLayer(1, 1);
        baggages.Add(e.baggage);
        e.baggage.transform.SetParent(gameObject.transform);
        e.baggage.transform.position = targetBaggagePosition.position + Vector3.up * spacing * baggages.Count;
    }

    private void DropBaggage( DropBaggageEvent e)
    {
        if(baggages.Count <= 0) return;
        
        int index = baggages.Count - 1;
        
        EventBus<PlaceToDeviceEvent>.Emit(new PlaceToDeviceEvent(baggages[index]));
        
        baggages.RemoveAt(index);
        
        if (baggages.Count <= 0)
        {
            EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
            playerAnimationsController.ChangeLayer(1, 0);
        }
    }
}
