using UnityEngine.Events;

public class UISignals : MonoSingleton<UISignals>
{
    public UnityAction<CompassDirection> OnCompassInfoChanged = delegate { }; 
    public UnityAction<float> OnDephtInfoChanged = delegate { }; 
}
