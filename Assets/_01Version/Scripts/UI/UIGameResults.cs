using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResults : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private UIPanelAnimator losePanel;
    [SerializeField] private UIPanelAnimator rewardPanel;
    [SerializeField] private UIPanelAnimator resultPanel;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI respawnPrice;

    [Header("Buttons")]
    public Button claimButton;
    public Button x2ClaimButton;
    public Button cancelButton;
    public Button reviveButton;
    public Button adsReviveButton;
    public Button homeButton;
    public Button restartButton;
    public Button nextLevelButton;

    private void Awake()
    {
        //HideAll();
        SetActiveFalse();
    }

    private void SetActiveFalse()
    {
        losePanel.gameObject.SetActive(false);
        rewardPanel.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(false);
    }

    public void HideAll()
    {
        losePanel.Hide();
        rewardPanel.Hide();
        resultPanel.Hide();
    }

    public void ShowLose()
    {
        HideAll();
        losePanel.Show();
    }

    public void ShowReward()
    {
        HideAll();
        rewardPanel.Show();
    }

    public void ShowResult()
    {
        HideAll();
        resultPanel.Show();
    }

    public void SetFinalStats(GameResultType type, int kill, float time)
    {
        resultText.text = GetResultText(type);
        killText.text = $"Enemies Killed: {kill}";
        timeText.text = $"Time has survived: {FormatTime(time)}";
    }

    public void SetRevivePrice(int price)
    {
        respawnPrice.text = $"{price}";
    }

    private string GetResultText(GameResultType type)
    {
        switch (type)
        {
            case GameResultType.Win: return "You Win!";
            case GameResultType.Lose: return "You Lose!";
        }
        return "";
    }

    private string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);
        return $"{min}:{sec:00}";
    }
}