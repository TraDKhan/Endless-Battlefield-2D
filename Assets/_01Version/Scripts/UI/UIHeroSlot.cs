using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets._01Version.Scripts.UI
{
    public class UIHeroSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _HeroNameText;
        [SerializeField] private TextMeshProUGUI _HeroParameterText;
        [SerializeField] private Image _HeroImage;
        [SerializeField] private GameObject _IsSelect;

        private int _index;

        public void Init(PlayerData data, int index)
        {
            _index = index;
            _IsSelect.SetActive(false);

            _HeroNameText.text = data.playerName;
            _HeroParameterText.text =
                $"HP: {data.GetBaseStat(CharacterStatType.MaxHP)}\n" +
                $"DEF: {data.GetBaseStat(CharacterStatType.Armor)}\n" +
                $"SPD: {data.GetBaseStat(CharacterStatType.MoveSpeed)}";

            _HeroImage.sprite = data.icon;
        }

        public void SetSelected(bool isSelected)
        {
            _IsSelect.SetActive(isSelected);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UIHeroManager.Instance.OnSlotClicked(_index);
            GameData.Instance.SetPlayer(UIHeroManager.Instance.GetCurrentPlayer());
        }
    }
}