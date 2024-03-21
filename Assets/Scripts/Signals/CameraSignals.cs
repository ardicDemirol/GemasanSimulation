using UnityEngine.Events;

public class CameraSignals : MonoSingleton<CameraSignals>
{
    public UnityAction<int> OnCameraChanged = delegate {  };
    public UnityAction<bool> OnPressRightClick = delegate {  };
    public UnityAction<bool> OnPressMiddleClick = delegate {  };
}
