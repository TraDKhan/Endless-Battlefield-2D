using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIHeroSelect : MonoBehaviour
{
    [SerializeField] private List<PlayerData> playerList;
    [SerializeField] private GameObject heroItemPrefab;
    [SerializeField] private Transform content;

    private void Start()
    {
        LoadHeroes();
    }

    void LoadHeroes()
    {
        foreach (var player in playerList)
        {
            GameObject go = Instantiate(heroItemPrefab, content);
            UIHeroItem item = go.GetComponent<UIHeroItem>();
            item.Setup(player, this);
        }
    }

    public void OnSelectHero(PlayerData player)
    {
        GameData.Instance.SetPlayer(player);
        SceneManager.LoadScene("HomeScene");
    }
}