using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sb.eventbus;
using UnityEngine;

public class Money : MonoBehaviour, ICollectible, IPool
{
    [Header("References")] 
    [SerializeField] private ParticleSystem moneyParticle;
    
    [Header("Settings")]
    [SerializeField] private int moneyValue;
    [SerializeField] private float duration;
    
    private Pool<Money> moneyPool;
    private Tween moneyTween;


    private void Start()
    {
        moneyPool = PoolManager.Instance.moneyPool;
    }


    public void OnCollect()
    {
        moneyParticle.transform.SetParent(null);
        moneyParticle.Play();
        EventBus<CurencyManagementEvent>.Emit(new CurencyManagementEvent(moneyValue));
        moneyPool.ReturnToPool(this);
    }

    private void MoneyAnimation(Vector3 scale)
    {
        if (moneyTween != null)
        {
            moneyTween.Kill();
        }

        moneyTween = transform.DOScale(scale, duration).SetEase(Ease.OutBounce);
    }

    public void OnPoolObjectSpawn()
    {
        moneyParticle.Stop();
        MoneyAnimation(Vector3.one);
    }

    public void OnPoolObjectDestroy()
    {
        
        MoneyAnimation(Vector3.zero);
    }
    
    
}
