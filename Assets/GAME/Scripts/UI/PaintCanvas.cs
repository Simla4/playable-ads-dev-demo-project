using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PaintCanvas : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.5f;
    
    private CanvasGroup canvasGroup;
    private Tween tween;
    private EventListener<NewAreaOpenedEvent> onNewAreaOpened;


    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        onNewAreaOpened = new EventListener<NewAreaOpenedEvent>(ShowPaintCanvas);
        EventBus<NewAreaOpenedEvent>.AddListener(onNewAreaOpened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOpenedEvent>.RemoveListener(onNewAreaOpened);
    }


    private void ShowPaintCanvas(NewAreaOpenedEvent e)
    {
        if(e.areaType != FillableAreaTypes.Board) return;
        
        if (tween != null)
        {
            tween.Kill();
        }
        
        tween = canvasGroup.DOFade(1, fadeDuration);
    }
}
