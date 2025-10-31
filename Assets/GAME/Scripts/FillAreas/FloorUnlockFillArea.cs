
using System;
using sb.eventbus;
using Unity.VisualScripting;
using UnityEngine;

public class FloorUnlockFillArea : FillableAreaBase
{
    private const string FLOOR_UNLOCK_FILL_AREA_NAME = "FloorUnlockFillArea";
    
    protected new void Start()
    {
        areaType = FillableAreaTypes.Floor;
        
        if (!PlayerPrefs.HasKey(FLOOR_UNLOCK_FILL_AREA_NAME))
        {
            PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, fillableAreaData.targetCost);
            PlayerPrefs.Save();
        }
        
        cost = PlayerPrefs.GetInt(FLOOR_UNLOCK_FILL_AREA_NAME);
        
        base.Start();
    }

    public override void FillArea(int amount)
    {
        base.FillArea(amount);
        
        if (cost <= 0)
        {
            EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType));
        }
    }

    public override void StopFilling()
    {
        base.StopFilling();
        
        PlayerPrefs.SetInt(FLOOR_UNLOCK_FILL_AREA_NAME, cost);

    }
}
