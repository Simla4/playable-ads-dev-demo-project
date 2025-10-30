using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StairBase : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private FloatingJoystick joystick;

    [Header("Settings")] [SerializeField] private float stepSpacing;
    [SerializeField] private float moveSpeed;

    private List<Transform> steps = new List<Transform>();
    private Vector3 moveDirection;
    private float escalatorLength;
    private int stepCount;
    private Tween moveTween;

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
            .OnComplete(() => joystick.gameObject.SetActive(true));
    }
}
