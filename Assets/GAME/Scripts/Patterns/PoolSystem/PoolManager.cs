using UnityEngine;

public class PoolManager: MonoSingleton<PoolManager>
{
    public Pool<Money> moneyPool { get; } = new Pool<Money>();
    [SerializeField] private Money moneyPrefab;
    
    public Pool<StairStepBase> stairStepPool { get; } = new Pool<StairStepBase>();
    [SerializeField] private StairStepBase stairStepPrefab;
    
    private void Awake()
    {
        moneyPool.Initialize(moneyPrefab);
        stairStepPool.Initialize(stairStepPrefab);
    }
}