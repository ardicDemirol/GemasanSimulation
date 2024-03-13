using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] cmVirtualCameras;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private static readonly string _mouseX = "Mouse X";
    private static readonly string _mouseY = "Mouse Y";
    private static readonly string _empty = "";


    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Update()
    {
        EnableFreeLook(Input.GetMouseButton(1));
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
        freeLookCamera.m_XAxis.m_InputAxisName = enable ? _mouseX : _empty;
        freeLookCamera.m_YAxis.m_InputAxisName = enable ? _mouseY : _empty;

        if (!enable)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
            freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
        }
        CameraSignals.Instance.OnPressRightClick?.Invoke(enable);
    }

    private void CameraChanged(int index)
    {
        for (int i = 0; i < cmVirtualCameras.Length; i++)
        {
            cmVirtualCameras[i].SetActive(i == index - 1);
        }
    }


}
