using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBase : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform startPosition;

    [SerializeField] private Transform endPosition;
    [SerializeField] private GameObject stepPrefab;

    [Header("Settings")] [SerializeField] private float stepSpacing;
    [SerializeField] private float moveSpeed;

    private List<Transform> steps = new List<Transform>();
    private Vector3 moveDirection;
    private float escalatorLength;
    private int stepCount;

    private void Start()
    {
        InitializeEscalator();
    }


    void Update()
    {
        MoveSteps();
    }

    private void InitializeEscalator()
    {
        moveDirection = (endPosition.position - startPosition.position).normalized;
        escalatorLength = Vector3.Distance(startPosition.position, endPosition.position);

        int stepCount = Mathf.FloorToInt(escalatorLength / stepSpacing);

        for (int i = 0; i < stepCount; i++)
        {
            Vector3 spawnPos = startPosition.position + moveDirection * stepSpacing * i;
            GameObject step = Instantiate(stepPrefab, spawnPos, Quaternion.identity, transform);
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
}
