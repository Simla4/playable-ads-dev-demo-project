using sb.eventbus;

public class CurencyManagementEvent : IEvent
{
    public int amount;

    public CurencyManagementEvent(int amount)
    {
        this.amount = amount;
    }
}
