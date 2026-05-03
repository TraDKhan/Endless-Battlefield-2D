using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets._01Version.Scripts
{
    public class ButtonController : MonoBehaviour, IPointerClickHandler
    {

        public enum Control
        {
            StartGame,
            LoadScene,
            ExitGame
        }

        public Control control = Control.StartGame;

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (control)
            {
                case Control.StartGame:
                    Debug.Log("Start Game");
                    //SceneManager.LoadScene("GameScene");
                    break;
                case Control.LoadScene:
                    Debug.Log("Load Scene");
                    //SceneManager.LoadScene("HeroScene");
                    break;
                case Control.ExitGame:
                    Debug.Log("Exit Game");
                    Application.Quit();
                    break;
            }
        }
    }
}