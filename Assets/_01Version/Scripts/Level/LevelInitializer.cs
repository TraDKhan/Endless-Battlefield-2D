using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelObject;

    private void Awake()
    {
        foreach (GameObject obj in levelObject)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void Start()
    {
        int currentLevel = LevelManager.Instance.GetSelectedLevel();

        int index = currentLevel - 1;

        if (index < 0 || index >= levelObject.Count)
        {
            return;
        }
        levelObject[index].SetActive(true);
    }
}
