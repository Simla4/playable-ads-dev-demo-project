using sb.eventbus;

public class NewAreaOpenedEvent : IEvent
{
    public FillableAreaTypes areaType;

    public NewAreaOpenedEvent(FillableAreaTypes areaType)
    {
        this.areaType = areaType;  
    }
}
