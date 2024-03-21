using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI compassInfoText;
    [SerializeField] private TextMeshProUGUI depthInfoText;
    [SerializeField] private GameObject boundaryWarningPanel;
   

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
        UISignals.Instance.OnBoundaryInfoPanelOpen += ChangeBoundaryPanel;
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.OnCompassInfoChanged -= ChangeCompassInfo;
        UISignals.Instance.OnDephtInfoChanged -= ChangeDepthInfo;
        UISignals.Instance.OnBoundaryInfoPanelOpen -= ChangeBoundaryPanel;
    }

    private void ChangeBoundaryPanel(bool arg0)
    {
        boundaryWarningPanel.SetActive(arg0);
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
