using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    public MainMenuManager menuManager;

    public void TriggerSceneLoad()
    {
        if (menuManager != null)
        {
            menuManager.LoadTargetScene();
        }
    }
}