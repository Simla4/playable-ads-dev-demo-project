using sb.eventbus;
using UnityEngine;

public class PlaceToDeviceEvent : IEvent
{
    public Baggage baggage;

    public PlaceToDeviceEvent(Baggage baggage)
    {
        this.baggage = baggage;
    }
}
