using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class CurencyManager : MonoBehaviour
{
    private const string CURRENCY_TAG = "Curency";
    private EventListener<CurencyManagementEvent> curencyListener;
    
    [SerializeField] private int startingCurency;
    

    private void Start()
    {
        PlayerPrefs.GetInt(CURRENCY_TAG, startingCurency);
    }

    private void OnEnable()
    {
        curencyListener = new EventListener<CurencyManagementEvent>(ChangeCurency);
        EventBus<CurencyManagementEvent>.AddListener(curencyListener);
    }

    private void OnDisable()
    {
        EventBus<CurencyManagementEvent>.RemoveListener(curencyListener);
    }

    private void ChangeCurency(CurencyManagementEvent e)
    {
        int curency = PlayerPrefs.GetInt(CURRENCY_TAG, startingCurency);
        curency += e.amount;
        PlayerPrefs.SetInt(CURRENCY_TAG, curency);
    }
}
