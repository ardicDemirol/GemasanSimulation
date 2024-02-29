using UnityEngine;
using UnityEngine.Events;

public class CameraSignals : MonoSingleton<CameraSignals>
{
    public UnityAction<int> OnCameraChanged = delegate {  };
}
