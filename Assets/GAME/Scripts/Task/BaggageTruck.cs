using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaggageTruck : MonoBehaviour
{
    private List<GameObject> baggages = new List<GameObject>();

    public void AddBag(GameObject baggage)
    {
        baggages.Add(baggage);
        baggage.transform.SetParent(transform);
    }

    public int GetBagCount()
    {
        return baggages.Count;
    }
}
