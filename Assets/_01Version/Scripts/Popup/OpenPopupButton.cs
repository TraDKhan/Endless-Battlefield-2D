using UnityEngine;

public class OpenPopupButton : MonoBehaviour
{
    [SerializeField] private BasePopup basePopup;

    public void Open()
    {
        Debug.Log("Click");
        basePopup.gameObject.SetActive(true);
        basePopup.Show();
    }
}