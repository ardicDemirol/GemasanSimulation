using UnityEngine.Events;

public class UISignals : MonoSingleton<UISignals>
{
    public UnityAction<CompassDirection> OnCompassInfoChanged = delegate { }; 
    public UnityAction<float> OnDephtInfoChanged = delegate { }; 
    public UnityAction<bool> OnBoundaryInfoPanelOpen = delegate { };
    public UnityAction OnDirtClean = delegate { };
}
