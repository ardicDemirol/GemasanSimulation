using UnityEngine;
using Cinemachine;

public class FreeLookWithRightClick : MonoBehaviour
{
    private CinemachineFreeLook _freeLookCamera;

    private void Start()
    {
        if (!TryGetComponent<CinemachineFreeLook>(out _freeLookCamera)) return;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            EnableFreeLook(true);
        }
        else
        {
            EnableFreeLook(false);
        }
    }

    private void EnableFreeLook(bool enable)
    {
        _freeLookCamera.m_XAxis.m_InputAxisName = enable ? "Mouse X" : "";
        _freeLookCamera.m_YAxis.m_InputAxisName = enable ? "Mouse Y" : "";

        if (!enable)
        {
            _freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
            _freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
        }

    }
}
