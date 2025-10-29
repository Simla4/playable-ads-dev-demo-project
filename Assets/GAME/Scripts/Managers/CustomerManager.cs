using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<CustomerMovement> customers;
    [SerializeField] private float duration = 0.25f;


    private void DropBaggage()
    {
        
    }
    
    private IEnumerator DropBaggageRoutine()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            yield return new WaitForSeconds(duration);
            customers[i].Move();
        }
    }
}
