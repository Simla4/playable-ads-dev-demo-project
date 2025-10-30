using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class PlayerBaggageController : MonoBehaviour
{
    [SerializeField] private Transform targetBaggagePosition;
    [SerializeField] private float spacing = 0.5f;
    
    private List<Baggage> baggages = new List<Baggage>();
    private EventListener<TakeBaggageEvent> onTakeBaggage;

    private void OnEnable()
    {
        onTakeBaggage = new EventListener<TakeBaggageEvent>(TakeBaggage);
        EventBus<TakeBaggageEvent>.AddListener(onTakeBaggage);
    }

    private void OnDisable()
    {
        EventBus<TakeBaggageEvent>.RemoveListener(onTakeBaggage);
    }


    private void TakeBaggage(TakeBaggageEvent e)
    {
        baggages.Add(e.baggage);
        e.baggage.transform.SetParent(gameObject.transform);
        e.baggage.transform.position = targetBaggagePosition.position + Vector3.up * spacing * baggages.Count;
    }
}
