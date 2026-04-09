using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("Root")]
    [SerializeField] private Transform popupRoot;

    private Stack<BasePopup> popupStack = new Stack<BasePopup>();

    private int baseSortingOrder = 100;
    private int currentSortingOrder = 100;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #region PUSH

    public T Push<T>(T popupPrefab) where T : BasePopup
    {
        // Instantiate
        T popup = Instantiate(popupPrefab, popupRoot);

        // Set sorting order
        SetSortingOrder(popup);

        // Disable popup dưới (nếu có)
        if (popupStack.Count > 0)
        {
            var top = popupStack.Peek();
            SetInteractable(top, false);
        }

        popupStack.Push(popup);
        popup.Show();

        return popup;
    }

    #endregion

    #region POP

    public void Pop()
    {
        if (popupStack.Count == 0) return;

        BasePopup top = popupStack.Pop();
        top.Hide();

        currentSortingOrder -= 10;

        // Enable lại popup dưới
        if (popupStack.Count > 0)
        {
            var next = popupStack.Peek();
            SetInteractable(next, true);
        }
    }

    public void PopAll()
    {
        while (popupStack.Count > 0)
        {
            BasePopup popup = popupStack.Pop();
            popup.Hide();
        }

        currentSortingOrder = baseSortingOrder;
    }

    #endregion

    #region UTIL

    private void SetSortingOrder(BasePopup popup)
    {
        Canvas canvas = popup.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = popup.gameObject.AddComponent<Canvas>();
        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = currentSortingOrder;

        currentSortingOrder += 10;
    }

    private void SetInteractable(BasePopup popup, bool value)
    {
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = popup.gameObject.AddComponent<CanvasGroup>();
        }

        cg.interactable = value;
        cg.blocksRaycasts = value;
    }

    #endregion
}