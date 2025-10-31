using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IFillable>(out IFillable fillable))
        {
            fillable.FillArea(1);
        }
        else if(other.TryGetComponent<ITask>(out ITask task))
        {
            task.StartTask();
        }
        else if (other.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            collectible.OnCollect();
        }

        if (other.TryGetComponent<StairBase>(out StairBase stairBase))
        {
            stairBase.MovePlayer(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IFillable>(out IFillable fillable))
        {
            fillable.StopFilling();
        }
        else if(other.TryGetComponent<ITask>(out ITask task))
        {
            task.StopTask();
        }
    }
}
