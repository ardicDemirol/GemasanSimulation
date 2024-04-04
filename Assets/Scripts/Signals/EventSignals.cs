using UnityEngine;
using UnityEngine.Events;

public class EventSignals : MonoSingleton<EventSignals>
{
    public UnityAction<GameObject> OnDirtClean = delegate { };
}
