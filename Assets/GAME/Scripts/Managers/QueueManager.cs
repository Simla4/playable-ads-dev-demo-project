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
    [SerializeField] private Transform deskPosition;
    [SerializeField] private Transform planePosition;
    
    private List<CustomerController> baggageQueueCustomers = new List<CustomerController>();
    private List<CustomerController> checkOutQueueCustomers = new List<CustomerController>();
    private EventListener<OnTakeBaggageEvent> onTakeBaggage;
    private EventListener<OnGetPlaneEvent> onGetPlane;
    private EventListener<OnTargetListOverEvent> onTargetListOver;
    private int checkOutTargetIndex = 0;


    private void Start()
    {
        InitializeCustomers();
    }

    private void OnEnable()
    {
        onTakeBaggage = new EventListener<OnTakeBaggageEvent>(AdvanceBaggageQueue);
        EventBus<OnTakeBaggageEvent>.AddListener(onTakeBaggage);

        onGetPlane = new EventListener<OnGetPlaneEvent>(AdvenceCheckOutQueue);
        EventBus<OnGetPlaneEvent>.AddListener(onGetPlane);

        onTargetListOver = new EventListener<OnTargetListOverEvent>(PlaceCheckOutQueue);
        EventBus<OnTargetListOverEvent>.AddListener(onTargetListOver);
    }

    private void OnDisable()
    {
        EventBus<OnTakeBaggageEvent>.RemoveListener(onTakeBaggage);
        EventBus<OnGetPlaneEvent>.RemoveListener(onGetPlane);
        EventBus<OnTargetListOverEvent>.RemoveListener(onTargetListOver);
    }


    private void InitializeCustomers()
    {
        for (int i = 0; i < customerCount; i++)
        {
            var customer = Instantiate(customerPrefab, baggageQueueList[i].position, baggageQueueList[i].rotation);
            
            baggageQueueCustomers.Add(customer);
        }
    }

    private void AdvanceBaggageQueue(OnTakeBaggageEvent e)
    {
        if(baggageQueueCustomers.Count <= 0) return;

        var targetCustomer = baggageQueueCustomers[0];
        
        targetCustomer.IsDropedBaggage = true;
        targetCustomer.Move(deskPosition);
        checkOutQueueCustomers.Add(targetCustomer);
        baggageQueueCustomers.RemoveAt(0);

        if (baggageQueueCustomers.Count <= 0)
        {
            EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
        }

        for (int i = 0; i < baggageQueueCustomers.Count; i++)
        {
            baggageQueueCustomers[i].Move(baggageQueueList[i]);
        }
    }

    private void PlaceCheckOutQueue(OnTargetListOverEvent e)
    {
        checkOutQueueCustomers[checkOutTargetIndex].Move(checkOutQueueList[checkOutTargetIndex]);
        checkOutQueueCustomers[checkOutTargetIndex].CanFly = true;
        checkOutTargetIndex++;
    }

    private void AdvenceCheckOutQueue(OnGetPlaneEvent e)
    {
        if(checkOutQueueCustomers.Count <= 0) return;
        
        var targetCustomer = checkOutQueueCustomers[0];
        
        if(!targetCustomer.CanFly) return;
        
        targetCustomer.Move(planePosition);
        checkOutQueueCustomers.RemoveAt(0);

        if (checkOutQueueCustomers.Count <= 0)
        {
            EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
        }
        
        EventBus<SpawnMoneyEvent>.Emit(new SpawnMoneyEvent());

        for (int i = 0; i < checkOutQueueCustomers.Count; i++)
        {
            checkOutQueueCustomers[i].Move(checkOutQueueList[i]);
        }
        
    }
}