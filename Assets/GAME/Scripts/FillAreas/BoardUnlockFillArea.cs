
using sb.eventbus;
using UnityEngine;

public class BoardUnlockFillArea : FillableAreaBase
{
    private const string BOARD_UNLOCK_FILL_AREA_NAME = "BoardUnlockFillArea";
    
    protected new void Start()
    {
        areaType = FillableAreaTypes.Board;
        
        if (!PlayerPrefs.HasKey(BOARD_UNLOCK_FILL_AREA_NAME))
        {
            PlayerPrefs.SetInt(BOARD_UNLOCK_FILL_AREA_NAME, fillableAreaData.targetCost);
            PlayerPrefs.Save();
        }
        
        cost = PlayerPrefs.GetInt(BOARD_UNLOCK_FILL_AREA_NAME);
        
        base.Start();
    }

    public override void FillArea(int amount)
    {
        if (cost <= 0)
        {
            EventBus<NewAreaOpenedEvent>.Emit(new NewAreaOpenedEvent(areaType));
        }
        
        base.FillArea(amount);
    }

    public override void StopFilling()
    {
        base.StopFilling();
        
        PlayerPrefs.SetInt(BOARD_UNLOCK_FILL_AREA_NAME, cost);

    }
}
