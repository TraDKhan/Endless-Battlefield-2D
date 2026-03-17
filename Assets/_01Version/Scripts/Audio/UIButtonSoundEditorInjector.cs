#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Events;

public class UIButtonSoundEditorInjector
{
    [MenuItem("Tools/Inject Button Sound (Persistent)")]
    static void Inject()
    {
        AudioManager audio = Object.FindFirstObjectByType<AudioManager>();
        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            UnityEditor.Events.UnityEventTools.AddPersistentListener(
                btn.onClick,
                audio.PlayClickButton
            );

            EditorUtility.SetDirty(btn);
        }

        Debug.Log("Injected persistent sound to " + buttons.Length + " buttons!");
    }

    [MenuItem("Tools/Remove Button Sound Only")]
    static void RemoveSound()
    {
        AudioManager audio = Object.FindFirstObjectByType<AudioManager>();
        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            int count = btn.onClick.GetPersistentEventCount();

            for (int i = count - 1; i >= 0; i--)
            {
                var target = btn.onClick.GetPersistentTarget(i);
                var method = btn.onClick.GetPersistentMethodName(i);

                if (target == audio && method == "PlayPickUpItem")
                {
                    UnityEventTools.RemovePersistentListener(btn.onClick, i);
                }
            }

            EditorUtility.SetDirty(btn);
        }

        Debug.Log("Removed button sound listeners!");
    }
}
#endif