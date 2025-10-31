using System;
using sb.eventbus;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;

    private Transform targetTransform;
    private EventListener<OnTaskChangedEvent> onTaskChanged;


    private void OnEnable()
    {
        onTaskChanged = new EventListener<OnTaskChangedEvent>(ChangeTargetTransform);
        EventBus<OnTaskChangedEvent>.AddListener(onTaskChanged);
    }

    private void OnDisable()
    {
        EventBus<OnTaskChangedEvent>.RemoveListener(onTaskChanged);
    }


    private void Update()
    {
        if (targetTransform == null) return;
        
        var distance =  Vector3.Distance(transform.position, targetTransform.position);

        if (distance < 1.5f)
        {
            arrowPrefab.SetActive(false);
        }
        else
        {
            arrowPrefab.SetActive(true);
        }

        var lookPosition = targetTransform.position;
        lookPosition.y = arrowPrefab.transform.position.y;
        arrowPrefab.transform.LookAt(lookPosition);
    }


    private void ChangeTargetTransform(OnTaskChangedEvent e)
    {
        targetTransform = e.targetTransform;
    }
}
