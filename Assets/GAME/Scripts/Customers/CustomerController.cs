using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public void Move(Transform target)
    {
        gameObject.transform.position = target.position;
    }
}
