using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    void OnPoolObjectSpawn();
    void OnPoolObjectDestroy();
}