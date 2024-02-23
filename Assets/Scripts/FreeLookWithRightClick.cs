using UnityEngine;
using Cinemachine;

public class FreeLookWithRightClick : MonoBehaviour
{
    private CinemachineFreeLook _freeLookCamera;

    void Start()
    {
        if (!TryGetComponent<CinemachineFreeLook>(out _freeLookCamera))
        {
            Debug.LogError("CinemachineFreeLook component bulunamad�!");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // E�er fare sa� t�kland�ysa
        {
            EnableFreeLook(true); // FreeLook kamera giri�lerini etkinle�tir
        }
        else if (Input.GetMouseButtonUp(1)) // E�er fare sa� t�klamas� b�rak�ld�ysa
        {
            EnableFreeLook(false); // FreeLook kamera giri�lerini devre d��� b�rak
        }
       
    }

    void EnableFreeLook(bool enable)
    {
        // Cinemachine FreeLook kamera bile�enindeki giri�lerin etkinli�ini ayarla
        _freeLookCamera.m_XAxis.m_InputAxisName = enable ? "Mouse X" : ""; // X ekseninin giri�ini ayarla
        _freeLookCamera.m_YAxis.m_InputAxisName = enable ? "Mouse Y" : ""; // Y ekseninin giri�ini ayarla

        if (!enable)
        {
            // E�er giri�ler devre d��� b�rak�l�rsa, kameran�n rotasyon h�z�n� s�f�rla
            _freeLookCamera.m_XAxis.m_InputAxisValue = 0f; // X ekseninin giri� de�erini s�f�rla
            _freeLookCamera.m_YAxis.m_InputAxisValue = 0f; // Y ekseninin giri� de�erini s�f�rla
        }
       
    }
}
