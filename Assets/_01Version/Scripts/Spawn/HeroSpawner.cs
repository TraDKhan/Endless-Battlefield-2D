using Unity.Cinemachine;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineCamera vcam;

    void Start()
    {
        var playerData = GameData.Instance.selectedPlayer;
        if (playerData == null)
        {
            Debug.LogError("No player selected!");
            return;
        }

        // Kiểm tra spawnPoint
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint not assigned!");
            return;
        }

        // Spawn hero
        GameObject hero = Instantiate(playerData.prefab, spawnPoint.position, Quaternion.identity);

        // Kiểm tra camera
        if (vcam == null)
        {
            Debug.LogError("Virtual Camera not assigned!");
            return;
        }

        vcam.Follow = hero.transform;
        vcam.LookAt = hero.transform;

        GameObject boundObj = GameObject.FindGameObjectWithTag("BoundingShape");

        if (boundObj == null)
        {
            Debug.LogError("No object with tag 'BoundingShape' found!");
            return;
        }

        Collider2D col = boundObj.GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("BoundingShape object has no Collider2D!");
            return;
        }

        var confiner = vcam.GetComponent<CinemachineConfiner2D>();

        if (confiner == null)
        {
            Debug.LogError("CinemachineConfiner2D not found on camera!");
            return;
        }

        confiner.BoundingShape2D = col;
        confiner.InvalidateBoundingShapeCache();
    }
}