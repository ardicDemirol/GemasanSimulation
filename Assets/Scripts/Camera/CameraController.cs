using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] cmVirtualCameras;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Update()
    {
        bool enableFreeLook = Input.GetMouseButton(1);
        EnableFreeLook(enableFreeLook);
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CameraSignals.Instance.OnCameraChanged += CameraChanged;
    }

    private void UnSubscribeEvents()
    {
        CameraSignals.Instance.OnCameraChanged -= CameraChanged;
    }



    private void EnableFreeLook(bool enable)
    {
        CameraSignals.Instance.OnPressRightClick?.Invoke(true);
        freeLookCamera.m_XAxis.m_InputAxisName = enable ? "Mouse X" : "";
        freeLookCamera.m_YAxis.m_InputAxisName = enable ? "Mouse Y" : "";

        if (!enable)
        {
            CameraSignals.Instance.OnPressRightClick?.Invoke(false);
            freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
            freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
        }
        //CameraSignals.Instance.OnPressRightClick?.Invoke(enable);
    }

    private void CameraChanged(int index)
    {
        for (int i = 0; i < cmVirtualCameras.Length; i++)
        {
            cmVirtualCameras[i].SetActive(i == index - 1);
        }
    }


}
