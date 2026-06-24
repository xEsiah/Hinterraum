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
}