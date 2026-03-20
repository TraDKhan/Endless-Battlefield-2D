using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public PlayerData selectedPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadFromPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadFromPrefs()
    {
        if (playerDatabase == null)
        {
            Debug.LogError("PlayerDatabase chưa được gán!");
            return;
        }
        int id = PlayerPrefs.GetInt("SelectedPlayer", 0);

        // cần reference database
        selectedPlayer = playerDatabase.GetByID(id);
    }

    [SerializeField] private PlayerDatabase playerDatabase;

    public void SetPlayer(PlayerData player)
    {
        selectedPlayer = player;

        PlayerPrefs.SetInt("SelectedPlayer", player.id);
        PlayerPrefs.Save();
    }
}