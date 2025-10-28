using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class CurencyManager : MonoSingleton<CurencyManager>
{
    private const string CURRENCY_TAG = "Curency";
    private EventListener<CurencyManagementEvent> curencyListener;
    
    [SerializeField] private int startingCurency;
    

    private void Start()
    {
        if (!PlayerPrefs.HasKey(CURRENCY_TAG))
        {
            PlayerPrefs.SetInt(CURRENCY_TAG, startingCurency);
            PlayerPrefs.Save();
        }
        EventBus<ChangeUITextEvent>.Emit(new ChangeUITextEvent(PlayerPrefs.GetInt(CURRENCY_TAG)));
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
        
        EventBus<ChangeUITextEvent>.Emit(new ChangeUITextEvent(curency));
    }

    public int GetCurency()
    {
        return PlayerPrefs.GetInt(CURRENCY_TAG, startingCurency);
    }
}
