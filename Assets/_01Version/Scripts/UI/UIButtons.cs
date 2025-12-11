using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIButtons : MonoBehaviour
{
    [Header("PANEL")]
    public GameObject _settingPanel;
    public GameObject _pausePanel;
    [Header("BUTTONs")]
    public Button _pauseButton;
    public Button _resumeButton;
    public Button _restartButton;
    public Button _quitGameButton;

    public Button _settingButton;
    public Button _saveGame;
    public Button _resetGame;
    public Button _exitButton;
    private void Awake()
    {
        SetActiveFalse();
    }
    private void Start()
    {
        _pauseButton.onClick.AddListener(() => OnPauseButton());
        _resumeButton.onClick.AddListener(() => OnResumeButton());
        _restartButton.onClick.AddListener(() => OnReStartButton());
        _quitGameButton.onClick.AddListener(() => OnQuitGameButton());

        _settingButton.onClick.AddListener(() => OnSettingButton());
        _saveGame.onClick.AddListener(() => OnSaveGame());
        _resetGame.onClick.AddListener(() => OnResetGame()); 
        _exitButton.onClick.AddListener(() => OnExitButton());
    }
    public void SetActiveFalse()
    {
        _settingPanel?.SetActive(false);
        _pausePanel?.SetActive(false);
    }

    public void OnPauseButton()
    {
        _pausePanel?.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnResumeButton()
    {
        SetActiveFalse();
        Time.timeScale = 1.0f;
    }
    public void OnReStartButton()
    {
        SetActiveFalse();
        Time .timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitGameButton()
    {
        SetActiveFalse();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("HomeScene");
    }

    public void OnSettingButton()
    {
        _settingPanel?.SetActive(false);
        _settingPanel?.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void OnSaveGame()
    {
        Debug.Log("click SaveGame button");
    }
    public void OnResetGame()
    {
        Debug.Log("click ResetGame button");
    }
    public void OnExitButton()
    {
        SetActiveFalse();
        Time .timeScale = 1.0f;
    }
}

