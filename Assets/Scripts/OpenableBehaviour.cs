using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class OpenableBehaviour : MonoBehaviour
{
    public enum StructureType { Chest, Door }
    public StructureType structureType;
    public GameObject contentPrefab;
    public float spawnYOffset = 1.0f;
    public GameObject originalPrefab;

    private bool isOpened = false;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TryOpen()
    {
        if (isOpened) return;

        if (structureType == StructureType.Chest)
        {
            if (GameManager.instance.hasKeyChest)
            {
                GameManager.instance.hasKeyChest = false;
                GameManager.instance.ShowItemUI("Chest unlocked !"); 
                
                OpenStructure();
                
                if (contentPrefab != null)
                {
                    StartCoroutine(SpawnContentRoutine());
                }
            }
            else
            {
                GameManager.instance.ShowItemUI("You need the chest key !");
            }
        }
        else if (structureType == StructureType.Door)
        {
            if (GameManager.instance.hasKeyDoor)
            {
                GameManager.instance.hasKeyDoor = false;
                GameManager.instance.ShowItemUI("Door unlocked !"); 
                
                GameManager.instance.StopLayoutTimer();
                OpenStructure();
                StartCoroutine(LoadCreditsRoutine());
            }
            else
            {
                GameManager.instance.ShowItemUI("You need the door key !");
            }
        }
    }

    private void OpenStructure()
    {
        isOpened = true;
        animator.SetTrigger("Opening");
    }

    private IEnumerator LoadCreditsRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Credits");
    }

    private IEnumerator SpawnContentRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 spawnPosition = transform.position + new Vector3(0f, spawnYOffset, 0f);
        
        GameObject spawnedContent = Instantiate(contentPrefab, spawnPosition, transform.rotation);
        spawnedContent.transform.SetParent(transform);
        spawnedContent.SetActive(true);
    }

    public void ResetState()
    {
        if (structureType == StructureType.Door) return;

        if (originalPrefab != null)
        {
            GameObject clone = Instantiate(originalPrefab, transform.parent);
            clone.transform.localPosition = transform.localPosition;
            clone.transform.localRotation = transform.localRotation;
            clone.transform.localScale = transform.localScale;
            clone.name = gameObject.name;
            
            Destroy(gameObject);
        }
    }
}