using sb.eventbus;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    [SerializeField] private int maxAdjacentCount = 3;
    [SerializeField] private float spacingZ = 0.5f;
    [SerializeField] private float spacingY = 0.5f;
    
    private EventListener<SpawnMoneyEvent> spawnMoneyEvent;
    private Pool<Money> moneyPool;
    private int count = 0;


    private void Start()
    {
        moneyPool = PoolManager.Instance.moneyPool;
    }

    private void OnEnable()
    {
        spawnMoneyEvent = new EventListener<SpawnMoneyEvent>(SpawnMoney);
        EventBus<SpawnMoneyEvent>.AddListener(spawnMoneyEvent);
    }

    private void OnDisable()
    {
        EventBus<SpawnMoneyEvent>.RemoveListener(spawnMoneyEvent);
    }


    private void SpawnMoney(SpawnMoneyEvent e)
    {
        count++;
        int height = 0;
        var money = moneyPool.Spawn();

        height = count / maxAdjacentCount;
        
        float posY = height * spacingY;
        float posZ = (count % maxAdjacentCount) * spacingZ;
        
        money.transform.position = transform.position + new Vector3(0, posY, -posZ);
    }
}
