using UnityEngine;

public class LightningController : MonoBehaviour
{
    [SerializeField] private GameObject lightningSkillPrefab;

    private void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("LightningController: Player not found");
            return;
        }

        Instantiate(lightningSkillPrefab, player);
    }
}
