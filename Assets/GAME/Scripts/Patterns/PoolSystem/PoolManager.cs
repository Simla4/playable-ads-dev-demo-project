using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolManager: MonoSingleton<PoolManager>
{
    public Pool<Money> moneyPool { get; } = new Pool<Money>();
    [SerializeField] private Money moneyPrefab;
    
    private void Awake()
    {
        moneyPool.Initialize(moneyPrefab);
    }
}