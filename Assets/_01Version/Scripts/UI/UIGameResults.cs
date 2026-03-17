using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResults : MonoBehaviour
{
    //todo
    //tạo logic ButtonController để kiểm soát các button có cùng chức năng
    //quản lý tất cả button trong GameScene
    [SerializeField] private GameObject _PanelResults;
    [SerializeField] private GameObject _PanelWin;
    [SerializeField] private GameObject _PanelLose;

    [SerializeField] private TextMeshProUGUI _TimeResultText;
    [SerializeField] private TextMeshProUGUI _EnemyKillText;
    [SerializeField] private TextMeshProUGUI _ResultsText;

    [SerializeField] private Button _ClaimButton;
    [SerializeField] private Button _X2ClaimButton;

    [SerializeField] private Button _CancelButton;
    [SerializeField] private Button _ReviveButton;
    [SerializeField] private Button _AdsReviveButton;

    [SerializeField] private Button _HomeButton;
    [SerializeField] private Button _RestartButton;
    [SerializeField] private Button _NextLevelButton;

    public PlayerHealthController playerCtrl;

    private int _KillCount;
    private float _Time;
    private string _Result;

    private void Start()
    {
        SetActiveFalse();
        SetActiveButton();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWin += HandleGameWin;
            GameManager.Instance.GameLose += HandleGameLose;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWin -= HandleGameWin;
            GameManager.Instance.GameLose -= HandleGameLose;
        }
    }

    public void SetActiveFalse()
    {
        _PanelResults.SetActive(false);
        _PanelWin.SetActive(false);
        _PanelLose.SetActive(false);
    }

    private void SetActiveButton()
    {
        //
        _ClaimButton.onClick.AddListener(() => OnGameResult());
        _CancelButton.onClick.AddListener(() => OnGameResult());

        //
        _X2ClaimButton.onClick.AddListener(() => On_X2ClaimButton());
        _AdsReviveButton.onClick.AddListener(() => On_AdsReviveButton());

        //
        _ReviveButton.onClick.AddListener(() => On_ReviveButton());

        //
        _HomeButton.onClick.AddListener(() => On_HomeButton());
        _RestartButton.onClick.AddListener(() => On_RestartButton());
        _NextLevelButton.onClick.AddListener(() => On_NextLevelButton());
    }

    private void HandleGameLose(int killCount, float time)
    {
        Time.timeScale = 0f;
        _Result = "You Lose!";
        _KillCount = killCount;
        _Time = time;

        SetActiveFalse();
        _PanelLose.SetActive(true);
    }

    private void HandleGameWin(int killCount, float time)
    {
        Time.timeScale = 0f;
        _Result = "You Win!";
        _KillCount = killCount;
        _Time = time;

        SetActiveFalse();
        _PanelWin.SetActive(true);
    }

    private void OnGameResult()
    {
        Time.timeScale = 0f;
        SetActiveFalse();
        _PanelResults.SetActive(true);

        _ResultsText.text = _Result;
        _EnemyKillText.text = $"Enemies Killed: {_KillCount}";
        _TimeResultText.text = $"Time: {_Time:F1}s";
    }

    #region WATCH ADS

    private void On_X2ClaimButton()
    {
        Debug.Log("click X2ClaimButton");
    }

    private void On_AdsReviveButton()
    {
        Debug.Log("click AdsContinueButton");
    }
    #endregion

    #region CONTINUE GAME

    private void On_ReviveButton()
    {
        Debug.Log("click ContinueButton");
    }
    #endregion


    private void On_HomeButton()
    {
        GameManager.Instance.Handle_BackHomeGame();
    }

    private void On_RestartButton()
    {
        GameManager.Instance.Handle_RestartGame();
    }

    private void On_NextLevelButton()
    {
        Debug.Log("click NextLevelButton");
    }
}
