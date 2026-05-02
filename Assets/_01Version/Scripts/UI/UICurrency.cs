using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets._01Version.Scripts.UI
{
    public class UICurrency : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _gemText;

        private void Start()
        {
            int coinAmount = CurrencyManager.Instance.GetCoins();
            int gemAmount = CurrencyManager.Instance.GetGems();
            _coinText.text = coinAmount.ToString();
            _gemText.text = gemAmount.ToString();
        }

        private void OnEnable() => CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;


        private void OnDisable() => CurrencyManager.Instance.OnCurrencyChanged -= UpdateCurrencyUI;


        private void UpdateCurrencyUI()
        {
            _coinText.text = CurrencyManager.Instance.GetCoins().ToString();
            _gemText.text = CurrencyManager.Instance.GetGems().ToString();
        }
    }
}