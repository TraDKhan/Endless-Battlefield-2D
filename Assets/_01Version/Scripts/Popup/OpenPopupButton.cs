using UnityEngine;
using UnityEngine.UI;

public class OpenPopupButton : MonoBehaviour
{
    [SerializeField] private BasePopup basePopup;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if(button != null)
            button.onClick.AddListener(Open);
    }

    public void Open()
    {
        Debug.Log("Click");
        basePopup.gameObject.SetActive(true);
        basePopup.Show();
    }
}