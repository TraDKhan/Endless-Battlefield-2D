using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OnClickYes()
    {
        SceneManager.LoadScene("EndlessScene");
    }

    public void OnClickNo()
    {
        panel.SetActive(false);
    }
}