using UnityEngine;
using TMPro;
using System.Text;

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
        compassInfoText.text = Extensions.StringBuilderAppend(direction.ToString());
    }

    private void ChangeDepthInfo(float depth)
    {
        depthInfoText.text = Extensions.StringBuilderAppend(Mathf.Abs(depth).ToString());

    }
}
