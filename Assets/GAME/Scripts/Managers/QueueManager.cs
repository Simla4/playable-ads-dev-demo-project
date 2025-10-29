using System;
using UnityEngine;
using System.Collections.Generic;
using sb.eventbus;
using Unity.VisualScripting;

public class QueueManager : MonoBehaviour
{
    [Header("Customer Infomations")]
    [SerializeField] private int customerCount = 5;
    [SerializeField] private CustomerController customerPrefab;
    [SerializeField] private Transform queueStartPoint;
    
    [Header("Queues")]
    [SerializeField] private List<Transform> baggageQueueList;
    [SerializeField] private List<Transform> checkOutQueueList;
    [SerializeField] private Transform stairPosition;
    
    private List<CustomerController> baggageQueueCustomers = new List<CustomerController>();
    private List<CustomerController> checkOutQueueCustomers = new List<CustomerController>();
    private EventListener<OnTakeBaggageEvent> onTakeBaggage;
    private int index = 0;


    private void Start()
    {
        InitializeCustomers();
    }

    private void OnEnable()
    {
        onTakeBaggage = new EventListener<OnTakeBaggageEvent>(AdvanceQueue);
        EventBus<OnTakeBaggageEvent>.AddListener(onTakeBaggage);
    }

    private void OnDisable()
    {
        EventBus<OnTakeBaggageEvent>.RemoveListener(onTakeBaggage);
    }


    private void InitializeCustomers()
    {
        for (int i = 0; i < customerCount; i++)
        {
            var customer = Instantiate(customerPrefab, baggageQueueList[i].position, baggageQueueList[i].rotation);
            Debug.Log(baggageQueueList[i].position);
            
            baggageQueueCustomers.Add(customer);
        }
    }

    private void AdvanceQueue(OnTakeBaggageEvent e)
    {
        if(baggageQueueCustomers.Count <= 0) return;
        
        
        baggageQueueCustomers[0].Move(stairPosition);
        checkOutQueueCustomers.Add(baggageQueueCustomers[0]);
        Debug.Log(baggageQueueCustomers[0].name + "deleted");
        baggageQueueCustomers.RemoveAt(0);

        for (int i = 0; i < baggageQueueCustomers.Count; i++)
        {
            baggageQueueCustomers[i].Move(baggageQueueList[i]);
        }
    }
}