using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI compassInfoText;
    [SerializeField] private TextMeshProUGUI depthInfoText;
    [SerializeField] private TextMeshProUGUI cleanedDirtText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject boundaryWarningPanel;
    [SerializeField] private GameObject pausePanel;


    private ushort _cleanedDirtAmount;
    private uint _score;
    private static readonly byte _sixty = 60;

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
        UISignals.Instance.OnTimerInfoChanged += ChangeTimer;
        UISignals.Instance.OnPausePanelToggle += TogglePausePanel;
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.OnCompassInfoChanged -= ChangeCompassInfo;
        UISignals.Instance.OnDephtInfoChanged -= ChangeDepthInfo;
        UISignals.Instance.OnBoundaryInfoPanelOpen -= ChangeBoundaryPanel;
        UISignals.Instance.OnDirtClean -= ChangeSuccessClean;
        UISignals.Instance.OnTimerInfoChanged -= ChangeTimer;
        UISignals.Instance.OnPausePanelToggle -= TogglePausePanel;
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
        depthInfoText.text = Extensions.StringBuilderAppend(Mathf.Abs(depth).ToString() + "m");
    }

    private void ChangeSuccessClean()
    {
        cleanedDirtText.text = Extensions.StringBuilderAppend((++_cleanedDirtAmount).ToString());
        ChangeScore(10);
    }

    private void ChangeScore(uint value)
    {
        scoreText.text = Extensions.StringBuilderAppend((_score+=value).ToString());
    }

    private void ChangeTimer(uint time)
    {
        int minute = (int)(time / _sixty); 
        float leftSecond = time % _sixty;
        string formattedTime = minute.ToString("D2") + ":" + leftSecond.ToString("00");
        timerText.text = Extensions.StringBuilderAppend(formattedTime);
    }

    private void TogglePausePanel()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = pausePanel.activeInHierarchy ? 0 : 1;
    }
}
