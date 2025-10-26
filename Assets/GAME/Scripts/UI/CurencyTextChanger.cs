using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using TMPro;
using UnityEngine;

public class CurencyTextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI curenccyText;
    [SerializeField] private float duration = 0.25f;
    
    private EventListener<CurencyManagementEvent> onCurencyChanged;

    private void OnEnable()
    {
        onCurencyChanged = new EventListener<CurencyManagementEvent>(ChangeText);
        EventBus<CurencyManagementEvent>.AddListener(onCurencyChanged);
    }

    private void OnDisable()
    {
        EventBus<CurencyManagementEvent>.RemoveListener(onCurencyChanged);
    }


    private void ChangeText(CurencyManagementEvent e)
    {
        StartCoroutine(ChangeTextRoutine(e.amount));
    }

    private IEnumerator ChangeTextRoutine(int amount)
    {
        yield return new WaitForSeconds(duration);
    }
}
