using sb.eventbus;

public class NewAreaOppenedEvent : IEvent
{
    public FillableAreaTypes areaType;

    public NewAreaOppenedEvent(FillableAreaTypes areaType)
    {
        this.areaType = areaType;  
    }
}
