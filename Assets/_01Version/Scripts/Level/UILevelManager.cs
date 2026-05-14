using System.Collections;
using UnityEngine;


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

            for (int i = 1; i <= maxLevel; i++)
            {
                UILevelSlot item = Instantiate(levelItemPrefab, contentParent);
                item.Setup(i);
            }
        }
    }
