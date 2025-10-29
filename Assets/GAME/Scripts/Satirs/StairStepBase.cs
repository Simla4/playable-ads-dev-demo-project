using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairStepBase : MonoBehaviour, IPool
{
    public void OnPoolObjectSpawn()
    {
        Debug.Log("StairStepBase.OnPoolObjectSpawn");
    }

    public void OnPoolObjectDestroy()
    {
        Debug.Log("StairStepBase.OnPoolObjectDestroy");
    }
}
