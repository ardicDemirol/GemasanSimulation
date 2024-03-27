using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI compassInfoText;
    [SerializeField] private TextMeshProUGUI depthInfoText;
    [SerializeField] private TextMeshProUGUI cleanedDirtText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject boundaryWarningPanel;


    private short _cleanedDirtAmount;
    private int _score;

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
        UISignals.Instance.OnDirtClean += ChangeSuccessClean;
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.OnCompassInfoChanged -= ChangeCompassInfo;
        UISignals.Instance.OnDephtInfoChanged -= ChangeDepthInfo;
        UISignals.Instance.OnBoundaryInfoPanelOpen -= ChangeBoundaryPanel;
        UISignals.Instance.OnDirtClean -= ChangeSuccessClean;
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

    private void ChangeSuccessClean()
    {
        cleanedDirtText.text = Extensions.StringBuilderAppend((++_cleanedDirtAmount).ToString());
        ChangeScore(10);
    }

    private void ChangeScore(int value)
    {
        scoreText.text = Extensions.StringBuilderAppend((_score+=value).ToString());
    }
}
