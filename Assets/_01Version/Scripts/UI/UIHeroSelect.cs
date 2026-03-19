using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIHeroSelect : MonoBehaviour
{

    public void OnSelectHero(PlayerData player)
    {
        GameData.Instance.SetPlayer(player);

        SceneManager.LoadScene("HomeScene");
    }
}