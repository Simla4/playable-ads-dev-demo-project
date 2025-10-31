using UnityEngine;

public class StairStepBase : MonoBehaviour, IPool
{
    public void OnPoolObjectSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnPoolObjectDestroy()
    {
        gameObject.SetActive(false);
    }
}
