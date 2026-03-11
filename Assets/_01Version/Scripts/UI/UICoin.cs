using UnityEngine;
using TMPro;

public class UICoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coinValue;
    // Use this for initialization
    void Start()
    {
        coinValue = CurrencyManager.Instance.GetCoins();
        coinText.text = coinValue.ToString();
    }
}
