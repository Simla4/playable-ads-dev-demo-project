using sb.eventbus;

public class ChangeUITextEvent : IEvent
{
    public int amount;

    public ChangeUITextEvent(int amount)
    {
        this.amount = amount;
    }
}
