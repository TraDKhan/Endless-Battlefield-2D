using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int _EnemyKilled;
    
    private float _Time;

    private bool _isGameResult;

    //to do: Sau chuyển thành sruct để thay thế 2 biến int, float
    // tối ưu bắn kết quả một lần duy nhất
    public event Action<int, float> GameWin;
    public event Action<int, float> GameLose;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _Time += Time.deltaTime;
    }
    
    public void Handle_GameWin()
    {
        AudioManager.Instance.PlayGameWin();
        //int currentLevel = SelectedLevelRuntime.SelectedLevelIndex;
        //LevelProgress.UnlockNextLevel(currentLevel);

        _isGameResult = true;
        GameWin?.Invoke(_EnemyKilled, _Time);
    }

    public void Handle_GameLose()
    {
        AudioManager.Instance.PlayGameLose();
        _isGameResult = false;

        GameLose?.Invoke(_EnemyKilled, _Time);
    }

    public void AddEnemyKill()
    {
        _EnemyKilled++;
    }

    public void Handle_RestartGame()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian được reset về bình thường
        ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Handle_BackHomeGame()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void Handle_NextLevel()
    {
        //int currentLevel = SelectedLevelRuntime.SelectedLevelIndex;
        //int nextLevel = currentLevel + 1;

        // nếu vượt quá level đã có → quay về Home hoặc loop
        //if (!LevelProgress.IsUnlocked(nextLevel))
        //{
        //    Debug.Log("No more levels!");
        //    SceneManager.LoadScene("HomeScene");
        //    return;
        //}

        //SelectedLevelRuntime.SelectedLevelIndex = nextLevel;
        ResetData();
        SceneManager.LoadScene("GameScene");
    }

    public bool GetGameResult () => _isGameResult;

    public int CalculateLossGold()
    {
        int goldFromMonsters = _EnemyKilled * 2;
        int goldFromTime = Mathf.FloorToInt(_Time / 20f);

        int totalGold = goldFromMonsters + goldFromTime;

        return Mathf.Clamp(totalGold, 1, 100);
    }

    private void ResetData()
    {
        _EnemyKilled = 0;
        _Time = 0;
    }
}
