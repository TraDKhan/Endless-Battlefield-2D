using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGeneratorTool : EditorWindow
{
    private LevelDatabase database;

    private int totalLevels = 50;

    // ===== Coin =====
    private int startCoin = 100;
    private int coinIncrease = 50;

    private Sprite coinIcon;
    private Sprite coinBackground;

    // ===== Gem =====
    private int startGem = 5;
    private int gemIncrease = 2;

    private Sprite gemIcon;
    private Sprite gemBackground;

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelGeneratorTool>("Level Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Generator", EditorStyles.boldLabel);

        database = (LevelDatabase)EditorGUILayout.ObjectField(
            "Database",
            database,
            typeof(LevelDatabase),
            false);

        totalLevels = EditorGUILayout.IntField(
            "Total Levels",
            totalLevels);

        GUILayout.Space(10);

        // ===== Coin =====

        GUILayout.Label("Coin", EditorStyles.boldLabel);

        startCoin = EditorGUILayout.IntField(
            "Start Coin",
            startCoin);

        coinIncrease = EditorGUILayout.IntField(
            "Coin Increase",
            coinIncrease);

        coinIcon = (Sprite)EditorGUILayout.ObjectField(
            "Coin Icon",
            coinIcon,
            typeof(Sprite),
            false);

        coinBackground = (Sprite)EditorGUILayout.ObjectField(
            "Coin Background",
            coinBackground,
            typeof(Sprite),
            false);

        GUILayout.Space(10);

        // ===== Gem =====

        GUILayout.Label("Gem", EditorStyles.boldLabel);

        startGem = EditorGUILayout.IntField(
            "Start Gem",
            startGem);

        gemIncrease = EditorGUILayout.IntField(
            "Gem Increase",
            gemIncrease);

        gemIcon = (Sprite)EditorGUILayout.ObjectField(
            "Gem Icon",
            gemIcon,
            typeof(Sprite),
            false);

        gemBackground = (Sprite)EditorGUILayout.ObjectField(
            "Gem Background",
            gemBackground,
            typeof(Sprite),
            false);

        GUILayout.Space(20);

        if (GUILayout.Button("Generate Levels"))
        {
            GenerateLevels();
        }
    }

    private void GenerateLevels()
    {
        if (database == null)
        {
            Debug.LogError("Database NULL");
            return;
        }

        List<LevelData> levelList = new List<LevelData>();

        string folderPath = "Assets/_01Version/Data/Levels";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/_01Version/Data", "Levels");
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            string assetPath =
                $"{folderPath}/Level_{i}.asset";

            // Xóa asset cũ nếu tồn tại
            LevelData oldLevel =
                AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);

            if (oldLevel != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            LevelData level =
                ScriptableObject.CreateInstance<LevelData>();

            level.levelIndex = i;

            level.rewards = new List<RewardData>();

            // ===== Coin =====

            RewardData coinReward = new RewardData
            {
                type = RewardType.Coin,

                amount = startCoin +
                         (i - 1) * coinIncrease,

                icon = coinIcon,

                backgroundIcon = coinBackground,

                isSpecial = false
            };

            // ===== Gem =====

            RewardData gemReward = new RewardData
            {
                type = RewardType.Gem,

                amount = startGem +
                         (i - 1) * gemIncrease,

                icon = gemIcon,

                backgroundIcon = gemBackground,

                isSpecial = false
            };

            level.rewards.Add(coinReward);
            level.rewards.Add(gemReward);

            AssetDatabase.CreateAsset(level, assetPath);

            levelList.Add(level);
        }

        database.levels = levelList.ToArray();

        EditorUtility.SetDirty(database);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Generate Levels Success");
    }
}