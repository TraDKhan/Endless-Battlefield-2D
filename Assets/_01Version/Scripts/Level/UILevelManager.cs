using System.Collections;
using UnityEngine;

namespace Assets._01Version.Scripts.Level
{
    public class UILevelManager : MonoBehaviour
    {
        public Transform contentParent;
        public UILevelSlot levelItemPrefab;

        private void Start()
        {
            GenerateLevels();
        }

        void GenerateLevels()
        {
            int maxLevel = LevelManager.Instance.GetMaxLevel();
            Debug.Log(maxLevel);

            for (int i = 1; i <= maxLevel; i++)
            {
                UILevelSlot item = Instantiate(levelItemPrefab, contentParent);
                item.Setup(i);
            }
        }
    }
}