using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        var playerData = GameData.Instance.selectedPlayer;

        if (playerData == null)
        {
            Debug.LogError("No player selected!");
            return;
        }

        Instantiate(playerData.prefab, spawnPoint.position, Quaternion.identity);
    }
}