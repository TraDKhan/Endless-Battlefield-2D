using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private Transform lastCheckpoint;

    [SerializeField] private GameObject player = null;

    private void Awake() => Instance = this;
    private int respawnCount;
    private int respawnPrice;

    private void Start()
    {
        respawnPrice = 100;
        respawnCount = 1;
    }

    public int GetReviveCost() => respawnPrice;

    public void StartRespawnProcess()
    {   
        if (CurrencyManager.Instance.GetCoins() >= respawnPrice)
        {
            CurrencyManager.Instance.AddCoins(-respawnPrice);
            respawnCount++;
            respawnPrice = 100 * respawnCount;
            Debug.Log($"Đã trừ {respawnPrice} coins cho lần respawn thứ {respawnCount}");
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            Debug.Log("Không đủ coins để respawn.");
        }
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        player.SetActive(true);
        if (PlayerController.Instance != null && lastCheckpoint != null)
        {
            PlayerController.Instance.HandleRespawn(lastCheckpoint.position);
        }
    }

    public void UpdateCheckpoint(Transform newPoint) => lastCheckpoint = newPoint;
}