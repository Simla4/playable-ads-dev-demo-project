using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float maksPaintSize;
    [SerializeField] private float minPaintSize;
    [SerializeField] private float minPos = -350;
    [SerializeField] private float maksPos = 350;
    [SerializeField] private PaintIn3D.P3dPaintSphere redButtonSize;
    [SerializeField] private PaintIn3D.P3dPaintSphere yellowButtonSize;
    [SerializeField] private PaintIn3D.P3dPaintSphere blueButtonSize;

    [SerializeField] private RectTransform barTransform, circleTransform;

    private Vector3 paintSize;

    private void ChangePaintSize(Vector3 size)
    {
        redButtonSize.Scale = size;
        yellowButtonSize.Scale = size;
        blueButtonSize.Scale = size;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        circleTransform.anchoredPosition += eventData.delta;

        var clampedX = Mathf.Clamp(circleTransform.anchoredPosition.x, minPos, maksPos);

        circleTransform.anchoredPosition = new Vector2(clampedX, barTransform.anchoredPosition.y);
        
        float mappedValue = MapValue(clampedX, minPos, maksPos, minPaintSize, maksPaintSize);
        
        ChangePaintSize(new Vector3(mappedValue, mappedValue, mappedValue));
    }
    
    private float MapValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return outputMin + (outputMax - outputMin) * ((value - inputMin) / (inputMax - inputMin));
    }
}
