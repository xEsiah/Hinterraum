using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public List<GameObject> objectsToDeactivateForHardMode;
    
    public float hardModePlaythroughDuration = 60f;
    public float hardModeDelayBeforeStress = 10f;

    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.isHardMode)
        {
            foreach (GameObject obj in objectsToDeactivateForHardMode)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            GameManager.instance.playthroughDuration = hardModePlaythroughDuration;
            GameManager.instance.StartLayoutTimer();

            if (AtmosphereManager.instance != null)
            {
                AtmosphereManager.instance.delayBeforeStress = hardModeDelayBeforeStress;
                AtmosphereManager.instance.ResetAtmosphere();
            }
        }
    }
}