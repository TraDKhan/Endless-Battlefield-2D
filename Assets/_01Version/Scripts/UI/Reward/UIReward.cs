using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Assets._01Version.Scripts.UI.Reward
{
    public class UIReward : MonoBehaviour
    {
        public UISlotReward rewardSlotPrefab;
        public Transform container;
        [SerializeField] private Sprite goldIcon;
        [SerializeField] private Sprite bgGoldIcon;
        

        public void ShowRewards(int levelIndex)
        {
            Debug.Log($"Showing rewards for level {levelIndex}");
            LevelData data = LevelManager.Instance.GetLevelData(levelIndex);

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            for(int i = 0; i < data.rewards.Count; i++)
            {
                var reward = data.rewards[i];

                UISlotReward newSlot = Instantiate(rewardSlotPrefab, container);

                var icon = reward.icon;
                var backgroundIcon = reward.backgroundIcon;
                bool isSpecialReward = reward.isSpecial;

                float delay = i * 0.2f;
                DOVirtual.DelayedCall(delay, () =>
                {
                    newSlot.Setup(icon, backgroundIcon, reward.amount, isSpecialReward);
                });
            }
        }

        public void ShowRewards()
        {
            int levelIndex = LevelManager.Instance.GetSelectedLevel();
            Debug.Log($"Showing rewards for level {levelIndex}");
            LevelData data = LevelManager.Instance.GetLevelData(levelIndex);

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            bool isWin = GameManager.Instance.GetGameResult();
            if (!isWin)
            {
                int lossGold = GameManager.Instance.CalculateLossGold();

                UISlotReward newSlot = Instantiate(rewardSlotPrefab, container);

                newSlot.Setup(goldIcon, bgGoldIcon, lossGold, false);
            }
            else
            {
                for (int i = 0; i < data.rewards.Count; i++)
                {
                    var reward = data.rewards[i];

                    UISlotReward newSlot = Instantiate(rewardSlotPrefab, container);

                    var icon = reward.icon;
                    var backgroundIcon = reward.backgroundIcon;
                    bool isSpecialReward = reward.isSpecial;

                    float delay = i * 0.2f;
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        newSlot.Setup(icon, backgroundIcon, reward.amount, isSpecialReward);
                    });
                }
            }
        }
    }
}