using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI compassInfoText;
    [SerializeField] private TextMeshProUGUI depthInfoText;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.Instance.OnCompassInfoChanged += ChangeCompassInfo;
        UISignals.Instance.OnDephtInfoChanged += ChangeDepthInfo;
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.OnCompassInfoChanged -= ChangeCompassInfo;
        UISignals.Instance.OnDephtInfoChanged -= ChangeDepthInfo;
    }

    private void ChangeCompassInfo(CompassDirection direction)
    {
        compassInfoText.text = direction.ToString();
    }

    private void ChangeDepthInfo(float depth)
    {
        depthInfoText.text = Mathf.Abs(depth).ToString();
    }
}
