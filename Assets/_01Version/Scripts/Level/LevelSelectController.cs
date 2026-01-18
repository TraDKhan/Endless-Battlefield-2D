using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private LevelSlotUI leftSlot;
    [SerializeField] private LevelSlotUI centerSlot;
    [SerializeField] private LevelSlotUI rightSlot;

    [Header("Play Button")]
    [SerializeField] private Button playButton;

    [Header("Levels")]
    [SerializeField] private List<LevelData> allLevels;

    private int currentIndex;

    void Start()
    {
        currentIndex = GetFirstUnlockedIndex();
        RefreshUI();

        playButton.onClick.AddListener(OnPlayClicked);
    }

    int GetFirstUnlockedIndex()
    {
        for (int i = 0; i < allLevels.Count; i++)
            if (allLevels[i].isUnlocked)
                return i;
        return 0;
    }

    void RefreshUI()
    {
        // CENTER
        Bind(centerSlot, currentIndex);

        // LEFT
        if (currentIndex > 0)
            Bind(leftSlot, currentIndex - 1);
        else
            leftSlot.Show(false);

        // RIGHT
        if (currentIndex < allLevels.Count - 1)
            Bind(rightSlot, currentIndex + 1);
        else
            rightSlot.Show(false);

        playButton.interactable = allLevels[currentIndex].isUnlocked;
    }

    void Bind(LevelSlotUI slot, int index)
    {
        slot.Show(true);
        slot.Bind(allLevels[index], OnSlotClicked);
    }

    void OnSlotClicked(LevelSlotUI clickedSlot)
    {
        if (clickedSlot == centerSlot)
            return;

        currentIndex = allLevels.IndexOf(clickedSlot.Level);
        RefreshUI();
    }

    void OnPlayClicked()
    {
        if (!allLevels[currentIndex].isUnlocked)
            return;

        SelectedLevelRuntime.SelectedLevelIndex =
            allLevels[currentIndex].levelIndex;

        SceneManager.LoadScene("GameScene");
    }
}
