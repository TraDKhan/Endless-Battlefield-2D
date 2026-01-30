using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmPopup : MonoBehaviour
{
    public static UIConfirmPopup Instance { get; private set; }

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onConfirm;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        yesButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            Hide();
        });
        noButton.onClick.AddListener(Hide);
    }

    public void Show(string message, Action confirmAction)
    {
        messageText.text = message;
        onConfirm = confirmAction;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        onConfirm = null;
    }
}