
using System;
using Unity.VisualScripting;
using UnityEngine;

public class FloorUnlockFillArea : FillableAreaBase
{
    private const string FLOOR_UNLOCK_FILL_AREA_NAME = "FloorUnlockFillArea";
    
    protected new void Start()
    {
        
        if (!PlayerPrefs.HasKey(FLOOR_UNLOCK_FILL_AREA_NAME))
        {
            PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, fillableAreaData.targetCost);
            PlayerPrefs.Save();
        }
        
        cost = PlayerPrefs.GetInt(FLOOR_UNLOCK_FILL_AREA_NAME);
        
        base.Start();
    }

    public override void StopFilling()
    {
        base.StopFilling();
        
        PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, cost);

    }
}
