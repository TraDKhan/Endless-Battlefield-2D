using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHeroItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private PlayerData data;
    private UIHeroSelect parent;

    public void Setup(PlayerData playerData, UIHeroSelect ui)
    {
        data = playerData;
        parent = ui;

        iconImage.sprite = data.icon;
    }

    public void OnClick()
    {
        parent.OnSelectHero(data);
    }
}