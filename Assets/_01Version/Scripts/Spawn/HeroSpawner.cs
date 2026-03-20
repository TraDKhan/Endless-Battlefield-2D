using Unity.Cinemachine;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineCamera vcam;

    void Start()
    {
        // Kiểm tra dữ liệu player
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

        // Gán follow và lookAt để camera theo dõi hero
        vcam.Follow = hero.transform;
        vcam.LookAt = hero.transform;

        // 🔥 TÌM BOUNDING SHAPE THEO TAG
        GameObject boundObj = GameObject.FindGameObjectWithTag("BoundingShape");

        if (boundObj == null)
        {
            Debug.LogError("No object with tag 'BoundingShape' found!");
            return;
        }

        // Lấy collider (ưu tiên Composite Collider)
        Collider2D col = boundObj.GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("BoundingShape object has no Collider2D!");
            return;
        }

        // 🔥 LẤY CONFINDER 2D
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();

        if (confiner == null)
        {
            Debug.LogError("CinemachineConfiner2D not found on camera!");
            return;
        }

        // GÁN BOUNDING SHAPE
        confiner.BoundingShape2D = col;

        // 🔥 QUAN TRỌNG: Refresh lại cache
        confiner.InvalidateBoundingShapeCache();
    }
}