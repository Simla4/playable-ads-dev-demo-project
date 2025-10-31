using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskBase> tasks;
    
    private int index = 0;
    private EventListener<OnTaskCompleteEvent> onTakeBaggage;


    private void OnEnable()
    {
        onTakeBaggage = new EventListener<OnTaskCompleteEvent>(ChangeTask);
        EventBus<OnTaskCompleteEvent>.AddListener(onTakeBaggage);
    }

    private void Start()
    {
        tasks[index].ActivateTask();
    }

    private void OnDisable()
    {
        EventBus<OnTaskCompleteEvent>.RemoveListener(onTakeBaggage);
    }


    private void ChangeTask(OnTaskCompleteEvent e)
    {
        if(tasks.Count <= 0) return;
        
        tasks[index].InactivateTask();
        index++;
        
        if(index >= tasks.Count) return;
        tasks[index].ActivateTask();
        
        Debug.Log("task activated" + index);
    }
}
