using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToConfirm : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject panelConfirm;
    private void Start()
    {
        panelConfirm.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        panelConfirm.SetActive(true);
    }
}