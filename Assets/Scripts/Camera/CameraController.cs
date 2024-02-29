using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] cmVirtualCameras;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private void Start()
    {
        if (!TryGetComponent<CinemachineFreeLook>(out freeLookCamera)) return;
    }

    private void Update()
    {
        bool enableFreeLook = Input.GetMouseButton(1);
        EnableFreeLook(enableFreeLook);
    }


    private void EnableFreeLook(bool enable)
    {
        freeLookCamera.m_XAxis.m_InputAxisName = enable ? "Mouse X" : "";
        freeLookCamera.m_YAxis.m_InputAxisName = enable ? "Mouse Y" : "";

        if (!enable)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
            freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
        }

    }
}
