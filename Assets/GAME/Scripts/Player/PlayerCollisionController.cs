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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IFillable>(out IFillable fillable))
        {
            fillable.StopFilling();
        }
    }
}
