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
    [SerializeField] private float spawnDuration = 0.1f;
    [SerializeField] private bool isTaskEnabled;

    private List<StairStepBase> steps = new List<StairStepBase>();
    private Vector3 moveDirection;
    private float escalatorLength;
    private int stepCount;
    private Tween moveTween;
    private EventListener<NewAreaOpenedEvent> newAreaOpened;
    private Pool<StairStepBase> stepPool;
    private WaitForSeconds creationDelay;
    private Coroutine creationCoroutine;
    private bool isTaskCompleted = false;

    protected override void Awake()
    {
        collider.enabled = !isTaskEnabled;
    }

    private void OnEnable()
    {
        newAreaOpened = new EventListener<NewAreaOpenedEvent>(InitializeEscalator);
        EventBus<NewAreaOpenedEvent>.AddListener(newAreaOpened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOpenedEvent>.RemoveListener(newAreaOpened);
    }

    private void Start()
    {
        stepPool = PoolManager.Instance.stairStepPool;
        creationDelay = new WaitForSeconds(spawnDuration);
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
            CreateStep(spawnPos);
        }

        if (creationCoroutine != null)
        {
            StopCoroutine(creationCoroutine);
        }
        
        creationCoroutine = StartCoroutine(StepCreator());
    }

    private IEnumerator StepCreator()
    {
        while (true)
        {
            if(transform.localScale.x < 1) yield return null;

            yield return creationDelay;

            CreateStep(startPosition.position);
        }
    }

    private void CreateStep(Vector3 spawnPos)
    {
        var step = stepPool.Spawn();
        step.transform.position = spawnPos;
        step.transform.rotation = Quaternion.identity;
        step.transform.SetParent(transform.parent);
        step.transform.localScale = Vector3.one;
        steps.Add(step);
    }
    
    private void MoveSteps()
    {
        var temp = new List<StairStepBase>(steps);
        
        for (int i = 0; i < temp.Count; i++)
        {
            var step = temp[i];
            step.transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float distanceFromStart = Vector3.Distance(startPosition.position, step.transform.position);
            if (distanceFromStart > escalatorLength)
            {
                steps.Remove(step);
                stepPool.ReturnToPool(step);
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
        
        if(isTaskCompleted || !isTaskEnabled) return;
        
        EventBus<OnTaskCompleteEvent>.Emit(new OnTaskCompleteEvent());
        isTaskCompleted = true;
    }
}
