using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private GameObject stepPrefab;

    [Header("Settings")]
    [SerializeField] private float stepSpacing;
    [SerializeField] private float moveSpeed;
    
    private List<Transform> steps = new List<Transform>();
    private Vector3 moveDirection;
    private float escalatorLength;
    private Pool<StairStepBase> stairStepPool;
    private int stepCount;

    private void Start()
    {
        stairStepPool = PoolManager.Instance.stairStepPool;
        
        InitializeEscalator();
    }

    void InitializeEscalator()
    {
        moveDirection = (endPosition.position - startPosition.position).normalized;
        escalatorLength = Vector3.Distance(endPosition.position, startPosition.position);

        stepCount = Mathf.CeilToInt(escalatorLength / stepSpacing);

        for (int i = 0; i < stepCount; i++)
        {
            var step = Instantiate(stepPrefab, transform);
            step.transform.position = startPosition.position + moveDirection * stepSpacing * i;
            steps.Add(step.transform);
        }
    }

    void Update()
    {
        foreach (var step in steps)
        {
            step.position += moveDirection * moveSpeed * Time.deltaTime;

            float distanceFromBottom = Vector3.Distance(step.position, startPosition.position);

            if (distanceFromBottom > escalatorLength)
            {
                step.position = startPosition.position - moveDirection * stepSpacing;
            }
        }
    }
}
