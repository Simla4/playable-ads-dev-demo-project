using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using TMPro;
using UnityEngine;

public class PlaneTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro customerCountTxt;
    
    private EventListener<OnCustomerArrivedEvent> onCustomerArrived;
    private int count = 0;

    private void OnEnable()
    {
        onCustomerArrived = new EventListener<OnCustomerArrivedEvent>(ChangeCustomerCountText);
        EventBus<OnCustomerArrivedEvent>.AddListener(onCustomerArrived);
    }

    private void OnDisable()
    {
        EventBus<OnCustomerArrivedEvent>.RemoveListener(onCustomerArrived);
    }

    private void ChangeCustomerCountText(OnCustomerArrivedEvent e)
    {
        count++;
        customerCountTxt.text = $" {count} / 5";
    }
}
