using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class TaskBase : MonoBehaviour
{
    [Header("Task References")]
    [SerializeField] private Image taskImage;
    [SerializeField] private BoxCollider collider;
    
    [Header("Settings")]
    [SerializeField] private float maxScale = 1.25f;
    [SerializeField] private float minScale = 1;
    [SerializeField] private float taskDuration = 0.25f;

    private Tween scaleTween;
    private Tween colorTween;

    protected void Awake()
    {
        collider.enabled = false;
    }

    public void ActivateTask()
    {
        collider.enabled = true;
        
        if(taskImage == null) return;
        
        if (scaleTween != null)
        {
            scaleTween.Kill();
        }
        
        if (colorTween != null)
        {
            colorTween.Kill();
        }
        
        scaleTween = taskImage.transform.DOScale(maxScale, taskDuration).SetEase(Ease.InOutSine);
        colorTween = taskImage.DOColor(Color.green, taskDuration);
    }

    public void InactivateTask()
    {
        if(taskImage == null) return;
        
        if (scaleTween != null)
        {
            scaleTween.Kill();
        }

        if (colorTween != null)
        {
            colorTween.Kill();
        }
        
        scaleTween = taskImage.transform.DOScale(minScale, taskDuration).SetEase(Ease.InOutSine);
        colorTween = taskImage.DOColor(Color.white, taskDuration);
    }
}
