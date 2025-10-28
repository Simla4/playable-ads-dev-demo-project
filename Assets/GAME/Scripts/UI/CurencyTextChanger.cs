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
    
    private EventListener<ChangeUITextEvent> onCurencyChanged;

    private void OnEnable()
    {
        onCurencyChanged = new EventListener<ChangeUITextEvent>(ChangeText);
        EventBus<ChangeUITextEvent>.AddListener(onCurencyChanged);
    }

    private void OnDisable()
    {
        EventBus<ChangeUITextEvent>.RemoveListener(onCurencyChanged);
    }


    private void ChangeText(ChangeUITextEvent e)
    {
        curenccyText.text = e.amount.ToString();
    }
}
