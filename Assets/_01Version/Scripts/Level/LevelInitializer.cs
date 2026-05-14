using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnerController> enemySpawners;
    [SerializeField] private List<GameObject> maps;

    private void Awake()
    {
        foreach (EnemySpawnerController spawner in enemySpawners)
        {
            if (spawner != null)
                spawner.gameObject.SetActive(false);
        }

        foreach (GameObject map in maps)
        {
            if (map != null)
                map.SetActive(false);
        }
    }

    private void Start()
    {
        int currentLevel = LevelManager.Instance.GetSelectedLevel();

        int index = currentLevel - 1;

        if (index < 0 || index >= enemySpawners.Count)
        {
            return;
        }
        RandomMap();
        enemySpawners[index].gameObject.SetActive(true);
    }

    private void RandomMap()
    {
        int randomIndex = Random.Range(0, maps.Count);
        maps[randomIndex].SetActive(true);
    }
}
