using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    public enum SceneName
    {
        HeroScene,
        EquipScene,
        ShopScene
    }

    public SceneName sceneName = SceneName.HeroScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName.ToString());
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
