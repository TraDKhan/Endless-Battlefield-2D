using UnityEngine;

public class GameResultController : MonoBehaviour
{
    [SerializeField] private UIGameResults ui;

    private int killCount;
    private float time;
    private GameResultType resultType;
    private UIState currentState = UIState.None;

    private void OnEnable()
    {
        //to do: logic truoc sau
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWin += OnGameWin;
            GameManager.Instance.GameLose += OnGameLose;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameWin -= OnGameWin;
            GameManager.Instance.GameLose -= OnGameLose;
        }
    }

    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        ui.cancelButton.onClick.RemoveAllListeners();
        ui.cancelButton.onClick.AddListener(OnCancel);

        ui.claimButton.onClick.RemoveAllListeners();
        ui.claimButton.onClick.AddListener(OnClaim);

        ui.x2ClaimButton.onClick.RemoveAllListeners();
        ui.x2ClaimButton.onClick.AddListener(OnX2Claim);

        ui.adsReviveButton.onClick.RemoveAllListeners();
        ui.adsReviveButton.onClick.AddListener(OnAdsRevive);

        ui.reviveButton.onClick.RemoveAllListeners();
        ui.reviveButton.onClick.AddListener(OnRevive);

        ui.homeButton.onClick.RemoveAllListeners();
        ui.homeButton.onClick.AddListener(OnHome);

        ui.restartButton.onClick.RemoveAllListeners();
        ui.restartButton.onClick.AddListener(OnRestart);

        ui.nextLevelButton.onClick.RemoveAllListeners();
        ui.nextLevelButton.onClick.AddListener(OnNextLevel);
    }

    private void ChangeState(UIState newState)
    {
        currentState = newState;
        Debug.Log($"UI State: {currentState}");

        switch (currentState)
        {
            case UIState.Lose:
                ui.ShowLose();
                break;

            case UIState.Reward:
                ui.ShowReward();
                break;

            case UIState.Result:
                ui.ShowResult();
                ui.SetFinalStats(resultType, killCount, time);
                break;
        }
    }

    // ================= EVENT =================

    private void OnGameWin(int kill, float t)
    {
        SetResult(GameResultType.Win, kill, t);
        ChangeState(UIState.Reward);
    }

    private void OnGameLose(int kill, float t)
    {
        SetResult(GameResultType.Lose, kill, t);
        ChangeState(UIState.Lose);
    }

    private void SetResult(GameResultType type, int kill, float t)
    {
        PauseGame();

        resultType = type;
        killCount = kill;
        time = t;
    }

    // ================= BUTTON =================
    private void OnCancel()
    {
        if (currentState == UIState.Lose)
        {
            Debug.Log("Lose → Reward");
            ChangeState(UIState.Reward);
        }
    }

    //to do: Thiết kế logic nhận thưởng theo thời gian và chỉ số kill
    private void OnClaim()
    {
        if (currentState == UIState.Reward)
        {
            Debug.Log("Reward → Result");
            ChangeState(UIState.Result);
        }
    }
    //to do: Xem quảng cáo để nhận thưởng gấp đôi
    private void OnX2Claim()
    {
        if (currentState == UIState.Reward)
        {
            Debug.Log("Reward → Result (x2)");
            ChangeState(UIState.Result);
        }
    }

    //to do: Xem quảng cáo để hồi sinh
    private void OnAdsRevive()
    {
        if (currentState == UIState.Lose)
        {
            Debug.Log("Dialog: Watch Ad → Revive");
            ResumeGame();
            ui.HideAll();
            currentState = UIState.None;
        }
    }

    //to do: Tiêu hao Gem để hồi sinh trực tiếp
    private void OnRevive()
    {
        if (currentState == UIState.Lose)
        {
            Debug.Log("Revive with Gems");
            ResumeGame();
            ui.HideAll();
            currentState = UIState.None;
        }
    }

    private void OnHome()
    {
        ResumeGame();
        GameManager.Instance.Handle_BackHomeGame();
    }

    private void OnRestart()
    {
        ResumeGame();
        GameManager.Instance.Handle_RestartGame();
    }

    private void OnNextLevel()
    {
        Debug.Log("Next Level");
        ResumeGame();
        GameManager.Instance.Handle_NextLevel();
    }

    // ================= TIME =================

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}