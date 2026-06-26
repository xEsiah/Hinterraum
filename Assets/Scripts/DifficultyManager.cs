using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float hardModePlaythroughDuration = 90f;
    public float hardModeDelayBeforeStress = 10f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.isHardMode)
        {
            hideMapPoints();

            GameManager.instance.playthroughDuration = hardModePlaythroughDuration;
            GameManager.instance.StartLayoutTimer();

            if (AtmosphereManager.instance != null)
            {
                AtmosphereManager.instance.delayBeforeStress = hardModeDelayBeforeStress;
                AtmosphereManager.instance.ResetAtmosphere();
            }
        }
    }

    public void hideMapPoints()
    {
        if (GameManager.instance == null || !GameManager.instance.isHardMode) return;

        GameObject[] mapPoints = GameObject.FindGameObjectsWithTag("mapPoint");
        foreach (GameObject obj in mapPoints)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}