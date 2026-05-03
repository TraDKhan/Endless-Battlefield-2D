using System.Collections;
using UnityEngine;

namespace Assets._01Version.Scripts.Popup
{
    public class Popup_ALLPanel : BasePopup
    {

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
        }

        protected override void OnShowComplete()
        {
            Debug.Log("Popup opened!");
        }

        protected override void OnHideComplete()
        {
            Debug.Log("Popup closed!");
        }
    }
}