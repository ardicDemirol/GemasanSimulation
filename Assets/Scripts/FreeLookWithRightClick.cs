using UnityEngine;
using Cinemachine;

public class FreeLookWithRightClick : MonoBehaviour
{
    private CinemachineFreeLook _freeLookCamera;

    void Start()
    {
        if (!TryGetComponent<CinemachineFreeLook>(out _freeLookCamera))
        {
            Debug.LogError("CinemachineFreeLook component bulunamadý!");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Eðer fare sað týklandýysa
        {
            EnableFreeLook(true); // FreeLook kamera giriþlerini etkinleþtir
        }
        else if (Input.GetMouseButtonUp(1)) // Eðer fare sað týklamasý býrakýldýysa
        {
            EnableFreeLook(false); // FreeLook kamera giriþlerini devre dýþý býrak
        }
       
    }

    void EnableFreeLook(bool enable)
    {
        // Cinemachine FreeLook kamera bileþenindeki giriþlerin etkinliðini ayarla
        _freeLookCamera.m_XAxis.m_InputAxisName = enable ? "Mouse X" : ""; // X ekseninin giriþini ayarla
        _freeLookCamera.m_YAxis.m_InputAxisName = enable ? "Mouse Y" : ""; // Y ekseninin giriþini ayarla

        if (!enable)
        {
            // Eðer giriþler devre dýþý býrakýlýrsa, kameranýn rotasyon hýzýný sýfýrla
            _freeLookCamera.m_XAxis.m_InputAxisValue = 0f; // X ekseninin giriþ deðerini sýfýrla
            _freeLookCamera.m_YAxis.m_InputAxisValue = 0f; // Y ekseninin giriþ deðerini sýfýrla
        }
       
    }
}
