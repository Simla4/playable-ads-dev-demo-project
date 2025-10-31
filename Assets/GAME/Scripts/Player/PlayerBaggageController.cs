using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private Tween takeTween;

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
        if (takeTween != null)
        {
            takeTween.Kill();
        }
        
        playerAnimationsController.ChangeLayer(1, 1);
        baggages.Add(e.baggage);
        e.baggage.transform.SetParent(targetBaggagePosition);
         e.baggage.transform.DOLocalJump(Vector3.right * spacing * baggages.Count, 0.5f, 1, 0.15f).SetEase(Ease.InOutSine);
        e.baggage.transform.rotation = targetBaggagePosition.rotation;
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
