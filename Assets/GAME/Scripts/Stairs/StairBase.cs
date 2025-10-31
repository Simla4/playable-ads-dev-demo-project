using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public class StairBase : TaskBase
{
    [Header("References")] [SerializeField]
    private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private FloatingJoystick joystick;

    [Header("Settings")] 
    [SerializeField] private float stepSpacing;
    [SerializeField] private float moveSpeed;

    private List<Transform> steps = new List<Transform>();
    private Vector3 moveDirection;
    private float escalatorLength;
    private int stepCount;
    private Tween moveTween;
    private EventListener<NewAreaOpenedEvent> newAreaOpened;


    private void OnEnable()
    {
        newAreaOpened = new EventListener<NewAreaOpenedEvent>(InitializeEscalator);
        EventBus<NewAreaOpenedEvent>.AddListener(newAreaOpened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOpenedEvent>.RemoveListener(newAreaOpened);
    }

    void Update()
    {
        if(transform.localScale.x <= 0) return;
        
        MoveSteps();
    }

    private void InitializeEscalator(NewAreaOpenedEvent e)
    {
        if (e.areaType != FillableAreaTypes.Floor) return;
        if(transform.localScale.x <= 0) return;
        
        moveDirection = (endPosition.position - startPosition.position).normalized;
        escalatorLength = Vector3.Distance(startPosition.position, endPosition.position);

        int stepCount = Mathf.FloorToInt(escalatorLength / stepSpacing) + 1;

        for (int i = 0; i < stepCount; i++)
        {
            Vector3 spawnPos = startPosition.position + moveDirection * stepSpacing * i;
            GameObject step = Instantiate(stepPrefab, spawnPos, Quaternion.identity, transform.parent);
            steps.Add(step.transform);
        }
    }
    
    private void MoveSteps()
    {
        for (int i = 0; i < steps.Count; i++)
        {
            Transform step = steps[i];
            step.position += moveDirection * moveSpeed * Time.deltaTime;

            float distanceFromStart = Vector3.Distance(startPosition.position, step.position);
            if (distanceFromStart > escalatorLength)
            {
                step.position = startPosition.position;
            }
        }
    }

    public void MovePlayer(Transform player)
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
     
        joystick.OnPointerUp(null);
        joystick.gameObject.SetActive(false);
        
        float distance = Vector3.Distance(startPosition.position, endPosition.position);
        float duration = distance / moveSpeed;

        moveTween = player.transform.DOMove(endPosition.position + new Vector3(0.5f, 0, 0), duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnMoveComplete());
    }

    private void OnMoveComplete()
    {
        joystick.gameObject.SetActive(true);
        EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
    }
}
