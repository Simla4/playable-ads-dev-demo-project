using sb.eventbus;
using UnityEngine;

public class OnTaskChangedEvent : IEvent
{
    public Transform targetTransform;

    public OnTaskChangedEvent(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }
}
