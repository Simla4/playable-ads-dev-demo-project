using sb.eventbus;
using UnityEngine;

public class TakeBaggageEvent : IEvent
{
    public Baggage baggage;

    public TakeBaggageEvent(Baggage baggage)
    {
        this.baggage = baggage;
    }
}
