using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public static LayoutManager instance;
    public GameObject[] layouts;
    private int currentLayoutIndex = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeLayoutRandomly();
    }

    public void ChangeLayoutRandomly()
    {
        if (layouts == null || layouts.Length == 0) return;

        int randomIndex;
        if (layouts.Length > 1)
        {
            do
            {
                randomIndex = Random.Range(0, layouts.Length);
            } while (randomIndex == currentLayoutIndex);
        }
        else
        {
            randomIndex = 0;
        }

        currentLayoutIndex = randomIndex;

        for (int i = 0; i < layouts.Length; i++)
        {
            if (layouts[i] != null)
            {
                layouts[i].SetActive(i == currentLayoutIndex);
            }
        }
    }

    public void ResetMapElements()
    {
        KeyPickup[] allKeys = FindObjectsByType<KeyPickup>(FindObjectsInactive.Include);
        foreach (KeyPickup key in allKeys)
        {
            if (key.gameObject.scene.IsValid())
            {
                key.gameObject.SetActive(true);
            }
        }

        foreach (GameObject layout in layouts)
        {
            if (layout == null) continue;

            OpenableBehaviour[] openables = layout.GetComponentsInChildren<OpenableBehaviour>(true);
            foreach (OpenableBehaviour openable in openables)
            {
                openable.ResetState();
            }
        }
    }
}