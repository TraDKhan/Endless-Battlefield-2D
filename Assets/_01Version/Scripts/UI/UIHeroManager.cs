using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._01Version.Scripts.UI
{
    public class UIHeroManager : MonoBehaviour
    {
        public static UIHeroManager Instance;

        [Header("Data")]
        [SerializeField] private List<PlayerData> _playerDataList;

        [Header("UI Slots")]
        [SerializeField] private List<UIHeroSlot> _slots;

        private int _currentIndex = -1;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitSlots();
        }

        private void InitSlots()
        {
            int count = Mathf.Min(_playerDataList.Count, _slots.Count);

            for (int i = 0; i < count; i++)
            {
                _slots[i].Init(_playerDataList[i], i);
            }
        }

        public void OnSlotClicked(int index)
        {
            _currentIndex = index;

            // Update UI
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SetSelected(i == index);
            }

            // Set GameData
            var selectedData = _playerDataList[index];
            GameData.Instance.SetPlayer(selectedData);
        }

        public PlayerData GetCurrentPlayer()
        {
            if (_currentIndex < 0) return null;
            return _playerDataList[_currentIndex];
        }
    }
}