using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    public enum SceneName
    {
        HeroScene,
        EquipScene,
        ShopScene,
        EndlessScene,
        EndlessScene2,
        EndlessScene3,
        EndlessScene4,
        EndlessScene5
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
