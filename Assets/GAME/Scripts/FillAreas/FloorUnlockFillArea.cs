
using System;
using Unity.VisualScripting;
using UnityEngine;

public class FloorUnlockFillArea : FillableAreaBase
{
    private const string FLOOR_UNLOCK_FILL_AREA_NAME = "FloorUnlockFillArea";
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey(FLOOR_UNLOCK_FILL_AREA_NAME))
        {
            PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, cost);
            PlayerPrefs.Save();
        }
        
        cost = PlayerPrefs.GetInt(FLOOR_UNLOCK_FILL_AREA_NAME);
    }

    public new void StopFilling()
    {
        PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, cost);

    }
}
